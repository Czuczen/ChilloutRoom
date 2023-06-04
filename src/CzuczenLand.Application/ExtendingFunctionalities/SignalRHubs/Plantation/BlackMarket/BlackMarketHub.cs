using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.BlackMarket.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Shared;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.BlackMarket;

/// <summary>
/// Hub SignalR obsługujący czarny rynek.
/// </summary>
[AbpMvcAuthorize]
public class BlackMarketHub : Hub, ITransientDependency
{
    /// <summary>
    /// Repozytorium przechowujące transakcje czarnego rynku w bazie danych.
    /// </summary>
    private readonly IRepository<BlackMarketTransaction> _blackMarketTransactionRepository;
    
    /// <summary>
    /// Repozytorium przechowujące magazyny plantacji w bazie danych.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Repozytorium przechowujące magazyny gracza w bazie danych.
    /// </summary>
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Repozytorium przechowujące typy generowane w bazie danych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium przechowujące dzielnice w bazie danych.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium przechowujące don'ów dzielnic w bazie danych.
    /// </summary>
    private readonly IRepository<DistrictDon> _districtDonRepository;
    
    /// <summary>
    /// Serwis do zarządzania magazynami plantacji.
    /// </summary>
    private readonly IPlantationStorageService _plantationStorageService;
    
    /// <summary>
    /// Serwis do zarządzania ignorowaniem zmian.
    /// </summary>
    private readonly IgnoreChangeService _ignoreChangeService;

    
    /// <summary>
    /// Właściwość pozwalająca na uzyskanie dostępu do sesji Abp, która przechowuje informacje dotyczące aktualnie zalogowanego użytkownika.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public IAbpSession AbpSession { get; set; }
        
    /// <summary>
    /// Interfejs ILogger służy do rejestrowania komunikatów z aplikacji.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public ILogger Logger { get; set; }
        
        
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="blackMarketTransactionRepository"></param>
    /// <param name="plantationStorageRepository"></param>
    /// <param name="playerStorageRepository"></param>
    /// <param name="generatedTypeRepository"></param>
    /// <param name="districtRepository"></param>
    /// <param name="districtDonRepository"></param>
    /// <param name="plantationStorageService"></param>
    /// <param name="ignoreChangeService"></param>
    public BlackMarketHub(
        IRepository<BlackMarketTransaction> blackMarketTransactionRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<PlayerStorage> playerStorageRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<District> districtRepository,
        IRepository<DistrictDon> districtDonRepository,
        IPlantationStorageService plantationStorageService,
        IgnoreChangeService ignoreChangeService
    )
    {
        _blackMarketTransactionRepository = blackMarketTransactionRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _playerStorageRepository = playerStorageRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _districtRepository = districtRepository;
        _districtDonRepository = districtDonRepository;
        _plantationStorageService = plantationStorageService;
        _ignoreChangeService = ignoreChangeService;
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
    }

    /// <summary>
    /// Metoda służąca do zakupu przedmiotu na czarnym rynku.
    /// </summary>
    /// <param name="transactionId">Identyfikator transakcji.</param>
    /// <param name="useDonToken">Flaga określająca, czy należy użyć żetonu Don'a.</param>
    [UnitOfWork]
    public virtual async Task Buy(int transactionId, bool useDonToken)
    {
        try
        {
            var status = new BlackMarketTransactionStatus();
            var userId = AbpSession.GetUserId();
            var transaction = await _blackMarketTransactionRepository.GetAsync(transactionId);
            var playerPlantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(userId);
            var district = await _districtRepository.GetAsync(playerPlantationStorage.DistrictId);
            var cost = transaction.Price * transaction.Quantity;

            var itemRepo = CustomRepositoryFactory.GetRepository(transaction.ItemEntityName);
            var plantationItems = await itemRepo.GetWhereAsync(RelationFieldsNames.PlantationStorageId, playerPlantationStorage.Id);
            var playerItem = plantationItems.Cast<Product>().Single(item => item.GeneratedTypeId == transaction.GeneratedTypeId);

            if (useDonToken)
            {
                if (!TokensOperator.DonTokenFee(playerPlantationStorage, status))
                {
                    await Clients.User(userId.ToString()).buy(status);
                    return;
                }
            }
            else if (playerPlantationStorage.Gold < cost)
            {
                status = new BlackMarketTransactionStatus {Status = BlackMarketStatuses.DangerStatus, Message = "Za mało kasy"};
                await Clients.User(userId.ToString()).buy(status);
                return;
            }
            else
                playerPlantationStorage.Gold -= cost;

            TokensOperator.BlackMarketTokenProfit(district, playerPlantationStorage, cost);
            if (transaction.SellerId != null) // null jeśli worker wystawia
            {
                var sellerPlantationStorage = await _plantationStorageRepository.SingleAsync(item =>
                    item.UserId == transaction.SellerId &&
                    item.DistrictId == playerPlantationStorage.DistrictId);
                sellerPlantationStorage.Gold += cost;
                var isDon = await _districtDonRepository.FirstOrDefaultAsync(
                    item => item.DistrictId == sellerPlantationStorage.DistrictId &&
                            item.PlantationStorageId == sellerPlantationStorage.Id) != null;

                TokensOperator.BlackMarketTokenProfit(district, sellerPlantationStorage, cost);
                TokensOperator.DonTokenProfit(isDon, district, playerPlantationStorage);
            }

            playerItem.OwnedAmount += transaction.Quantity;
            status = new BlackMarketTransactionStatus
            {
                Status = BlackMarketStatuses.SuccessStatus,
                Message = "Transakcja zakończona. Kupiono - " + transaction.Quantity +
                          PlantationManagerHelper.GetMeasureUnitByEntityName(transaction.ItemEntityName) +
                          " " + transaction.ItemName
            };
                
            transaction.BuyerId = userId;
            await _blackMarketTransactionRepository.DeleteAsync(transaction.Id);

                await Clients.User(userId.ToString()).buy(status);
        }
        catch (Exception ex)
        {
            if (ex is EntityNotFoundException exception && exception.EntityType.FullName != null && exception.EntityType.FullName.Contains("BlackMarketTransaction"))
            {
                var userId = AbpSession.GetUserId();
                var data = new BlackMarketTransactionStatus{Status = BlackMarketStatuses.DangerStatus, Message = "Transakcja jest nie dostępna. Upewnij się czy jest jeszcze na liście."};
                await Clients.User(userId.ToString()).buy(data);
            }
            else
            {
                Logger.Error("Blad =======//========", ex);
                await Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);    
            }
        }
    }

    /// <summary>
    /// Metoda służąca do wystawiania przedmiotu na czarnym rynku.
    /// </summary>
    /// <param name="issueTransaction">Obiekt zawierający informacje o wystawianej transakcji na czarnym rynku.</param>
    /// <param name="useBlackMarketToken">Flaga określająca, czy należy użyć żetonu czarnego rynku.</param>
    [UnitOfWork]
    public virtual async Task Issue(IssueTransaction issueTransaction, bool useBlackMarketToken)
    {
        try
        {
            var status = new BlackMarketTransactionStatus();
                
            var userId = AbpSession.GetUserId();
            var playerName = (await _playerStorageRepository.SingleAsync(item => item.UserId == userId)).PlayerName;
        
            var repo = CustomRepositoryFactory.GetRepository(issueTransaction.ItemEntityName);
            var issuedItem = (Product) await repo.GetAsync(issueTransaction.ItemId);
            var districtId = (await _generatedTypeRepository.GetAsync(issuedItem.GeneratedTypeId)).DistrictId;
                
            if (issuedItem.OwnedAmount < issueTransaction.Quantity)
                status = new BlackMarketTransactionStatus{Status = BlackMarketStatuses.DangerStatus, Message = "Niewystarczająca ilość przedmiotu - " + issueTransaction.ItemName};
            else
            {
                var playerPlantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(userId);
                var districtDon = await _districtDonRepository.FirstOrDefaultAsync(item => item.DistrictId == districtId);

                if (districtDon != null)
                {
                    var donPlantationStorage = await _plantationStorageRepository.SingleAsync(item => item.Id == districtDon.PlantationStorageId);
                    var district = await _districtRepository.GetAsync(districtId);
                    var transactionCost = issueTransaction.Price * issueTransaction.Quantity;
                    var donTribute = district.DonCharityPercentage * transactionCost;
                        
                    if (useBlackMarketToken)
                    {
                        if (TokensOperator.DealerTokenFee(playerPlantationStorage, status))
                        {
                            if (TokensOperator.BlackMarketTokenFee(playerPlantationStorage, status))
                            {
                                donPlantationStorage.Gold += donTribute;
                                status = await IssueTransaction(issuedItem, issueTransaction, userId, playerName, districtId);   
                            }
                            else
                            {
                                playerPlantationStorage.DealerToken++;
                                await Clients.User(userId.ToString()).issue(status);
                                return;    
                            }
                        }
                        else
                        {
                            await Clients.User(userId.ToString()).issue(status);
                            return;
                        }
                    }
                    else if (playerPlantationStorage.Gold < donTribute)
                    {
                        status.Status = BlackMarketStatuses.DangerStatus;
                        status.Message = "Za mało kasy";
                        await Clients.User(userId.ToString()).issue(status);
                        return;
                    }
                    else
                    {
                        if (TokensOperator.DealerTokenFee(playerPlantationStorage, status))
                        {
                            playerPlantationStorage.Gold -= donTribute;
                            donPlantationStorage.Gold += donTribute;
                            status = await IssueTransaction(issuedItem, issueTransaction, userId, playerName, districtId);
                        }
                        else
                        {
                            await Clients.User(userId.ToString()).issue(status);
                            return;
                        }
                    }
                }
                else
                {
                    if (TokensOperator.DealerTokenFee(playerPlantationStorage, status))
                        status = await IssueTransaction(issuedItem, issueTransaction, userId, playerName, districtId);
                    else
                    {
                        await Clients.User(userId.ToString()).issue(status);
                        return;
                    }
                }
            }

            await Clients.User(userId.ToString()).issue(status);
        }
        catch (Exception ex)
        {
            Logger.Error("Blad =======//========", ex);
            await Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);
        }
    }
        
    /// <summary>
    /// Metoda służąca do anulowania wystawionej transakcji na czarnym rynku.
    /// </summary>
    /// <param name="transactionId">Identyfikator transakcji do anulowania.</param>
    [UnitOfWork]
    public virtual async Task Cancel(int transactionId)
    {
        try
        {
            var transaction = await _blackMarketTransactionRepository.GetAsync(transactionId);
            var canceledItem = (Product) await CustomRepositoryFactory.GetRepository(transaction.ItemEntityName).GetAsync(transaction.ItemId);
            canceledItem.OwnedAmount += transaction.Quantity;
            await _ignoreChangeService.Add(canceledItem);
            transaction.IsCanceled = true;
        
            await _blackMarketTransactionRepository.DeleteAsync(transactionId);
        }
        catch (Exception ex)
        {
            if (ex is EntityNotFoundException exception && exception.EntityType.FullName != null && exception.EntityType.FullName.Contains("BlackMarketTransaction"))
            {
                var userId = AbpSession.GetUserId();
                var data = new BlackMarketTransactionStatus{Status = BlackMarketStatuses.DangerStatus, Message = "Transakcja jest nie dostępna. Upewnij się czy jest jeszcze na liście."};
                await Clients.User(userId.ToString()).notCanceled(data);
            }
            else
            {
                Logger.Error("Blad =======//========", ex);
                await Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);    
            }
        }
    }

    /// <summary>
    /// Metoda pomocnicza do obsługi operacji wystawienia transakcji na czarnym rynku.
    /// </summary>
    /// <param name="issuedItem">Wystawiany przedmiot.</param>
    /// <param name="issueTransaction">Obiekt zawierający informacje o wystawianej transakcji na czarnym rynku.</param>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="playerName">Nazwa gracza.</param>
    /// <param name="districtId">Identyfikator dzielnicy.</param>
    /// <returns>Status transakcji na czarnym rynku po wystawieniu przedmiotu.</returns>
    private async Task<BlackMarketTransactionStatus> IssueTransaction(Product issuedItem, IssueTransaction issueTransaction, long userId, string playerName, int districtId)
    {
        issuedItem.OwnedAmount -= issueTransaction.Quantity;
        await _ignoreChangeService.Add(issuedItem);
    
        var newTransaction = new BlackMarketTransaction
        {
            ItemId = issueTransaction.ItemId,
            ItemName = issueTransaction.ItemName, 
            ItemEntityName = issueTransaction.ItemEntityName,
            SellerId = userId,
            GeneratedTypeId = issuedItem.GeneratedTypeId,
            SellerName = playerName,
            Price = issueTransaction.Price,
            Quantity = issueTransaction.Quantity,
            DistrictId = districtId
        };
 
        await _blackMarketTransactionRepository.InsertAsync(newTransaction);
        return new BlackMarketTransactionStatus{Status = BlackMarketStatuses.SuccessStatus, Message = "Wystawiono: " + issueTransaction.Quantity 
            + PlantationManagerHelper.GetMeasureUnitByEntityName(issueTransaction.ItemEntityName) + " " + issueTransaction.ItemName + " po " + issueTransaction.Price + "$"};
    }
}
