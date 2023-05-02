using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Storage.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Storage;

/// <summary>
/// Hub SignalR obsługujący magazyn/zasoby plantacji
/// </summary>
[AbpMvcAuthorize]
public class StorageHub : Hub, ITransientDependency
{
    /// <summary>
    /// Repozytorium przechowujące susz w bazie danych.
    /// </summary>
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    
    /// <summary>
    /// Repozytorium przechowujące lampy w bazie danych.
    /// </summary>
    private readonly IRepository<Lamp> _lampRepository;
    
    /// <summary>
    /// Repozytorium przechowujące nawozy w bazie danych.
    /// </summary>
    private readonly IRepository<Manure> _manureRepository;
    
    /// <summary>
    /// Repozytorium przechowujące gleby w bazie danych.
    /// </summary>
    private readonly IRepository<Soil> _soilRepository;
    
    /// <summary>
    /// Repozytorium przechowujące wodę w bazie danych.
    /// </summary>
    private readonly IRepository<Water> _waterRepository;
    
    /// <summary>
    /// Repozytorium przechowujące doniczki w bazie danych.
    /// </summary>
    private readonly IRepository<Pot> _potRepository;
    
    /// <summary>
    /// Repozytorium przechowujące nasiona w bazie danych.
    /// </summary>
    private readonly IRepository<Seed> _seedRepository;
    
    /// <summary>
    /// Repozytorium przechowujące bonusy w bazie danych.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.Products.Bonus> _bonusRepository;
    
    /// <summary>
    /// Repozytorium przechowujące dzielnice w bazie danych.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium przechowujące magazyny plantacji w bazie danych.
    /// </summary>
    private readonly IPlantationStorageService _plantationStorageService;
    
    /// <summary>
    /// Repozytorium przechowujące magazyny gracza w bazie danych.
    /// </summary>
    private readonly IPlayerStorageService _playerStorageService;

    
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
    /// <param name="driedFruitRepository">Repozytorium suszu.</param>
    /// <param name="lampRepository">Repozytorium lamp.</param>
    /// <param name="manureRepository">Repozytorium nawozów.</param>
    /// <param name="soilRepository">Repozytorium gleb.</param>
    /// <param name="waterRepository">Repozytorium wody.</param>
    /// <param name="potRepository">Repozytorium donic.</param>
    /// <param name="seedRepository">Repozytorium nasion.</param>
    /// <param name="bonusRepository">Repozytorium bonusów.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="plantationStorageService">Serwis magazynu plantacji.</param>
    /// <param name="playerStorageService">Serwis magazynu gracza.</param>
    public StorageHub(
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<Lamp> lampRepository,
        IRepository<Manure> manureRepository,
        IRepository<Soil> soilRepository,
        IRepository<Water> waterRepository,
        IRepository<Pot> potRepository,
        IRepository<Seed> seedRepository,
        IRepository<ExtendingModels.Models.Products.Bonus> bonusRepository,
        IRepository<District> districtRepository,
        IPlantationStorageService plantationStorageService,
        IPlayerStorageService playerStorageService
    )
    {
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;

        _driedFruitRepository = driedFruitRepository;
        _lampRepository = lampRepository;
        _manureRepository = manureRepository;
        _soilRepository = soilRepository;
        _waterRepository = waterRepository;
        _potRepository = potRepository;
        _seedRepository = seedRepository;
        _bonusRepository = bonusRepository;
        _districtRepository = districtRepository;
        _plantationStorageService = plantationStorageService;
        _playerStorageService = playerStorageService;
    }

    /// <summary>
    /// Metoda służąca do przeprowadzania transakcji zasobów w sklepie.
    /// </summary>
    /// <param name="productId">Identyfikator produktu.</param>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="howMany">Ilość przetwarzanych zasobów.</param>
    /// <param name="action">Typ akcji (np. "buy" lub "sell").</param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task<ShopTransaction> ResourcesTransaction(int productId, string entity, decimal howMany, string action)
    {
        try
        {
            var ret = new ShopTransaction();
            var userId = AbpSession.GetUserId();
            var playerStorage = await _playerStorageService.GetPlayerStorage(userId);
            var playerPlantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(userId);
            var district = await _districtRepository.GetAsync(playerPlantationStorage.DistrictId);
                
            if (PlantationStorageObservedFields.CurrencyExchanges.Any(item => item == entity))
                StorageHubHelper.ProcessCurrencyExchange(district, playerPlantationStorage, playerStorage, entity, action, howMany, ret);
            else
                await ProcessProduct(playerPlantationStorage, action, howMany, ret, productId, entity);

            return ret;
        }
        catch (Exception ex)
        {
            Logger.Error("Blad =======//========", ex);
            await Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Metoda służąca do przeprowadzania transakcji produktu w sklepie.
    /// </summary>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="action">Typ akcji (kupna/sprzedaży).</param>
    /// <param name="amount">Ilość produktu.</param>
    /// <param name="shopTransaction">Obiekt transakcji sklepu.</param>
    /// <param name="productId">Identyfikator produktu.</param>
    /// <param name="entity">Nazwa encji.</param>
    private async Task ProcessProduct(PlantationStorage playerPlantationStorage, string action, decimal amount, 
        ShopTransaction shopTransaction, int productId, string entity)
    {
        var entityEnum = SelectListLoaderHelper.GetEntityEnum(entity);
        switch (entityEnum)
        {
            case EnumUtils.Entities.DriedFruit:
                var driedFruit = await _driedFruitRepository.GetAsync(productId);
                switch (action)
                {
                    case StorageHubHelper.BuyActionName:
                        StorageHubHelper.BuyAction<DriedFruit>(shopTransaction, action, playerPlantationStorage, amount, driedFruit);
                        break;
                    case StorageHubHelper.SellActionName:
                        StorageHubHelper.SellAction<DriedFruit>(shopTransaction, action, playerPlantationStorage, amount, driedFruit);
                        break;
                }
                break;
            case EnumUtils.Entities.Lamp:
                var lamp = await _lampRepository.GetAsync(productId);
                switch (action)
                {
                    case StorageHubHelper.BuyActionName:
                        StorageHubHelper.BuyAction<Lamp>(shopTransaction, action, playerPlantationStorage, amount, lamp);
                        break;
                    case StorageHubHelper.SellActionName:
                        StorageHubHelper.SellAction<Lamp>(shopTransaction, action, playerPlantationStorage, amount, lamp);
                        break;
                }
                break;
            case EnumUtils.Entities.Manure:
                var manure = await _manureRepository.GetAsync(productId);
                switch (action)
                {
                    case StorageHubHelper.BuyActionName:
                        StorageHubHelper.BuyAction<Manure>(shopTransaction, action, playerPlantationStorage, amount, manure);
                        break;
                    case StorageHubHelper.SellActionName:
                        StorageHubHelper.SellAction<Manure>(shopTransaction, action, playerPlantationStorage, amount, manure);
                        break;
                }
                break;
            case EnumUtils.Entities.Pot:
                var pot = await _potRepository.GetAsync(productId);
                switch (action)
                {
                    case StorageHubHelper.BuyActionName:
                        StorageHubHelper.BuyAction<Pot>(shopTransaction, action, playerPlantationStorage, amount, pot);
                        break;
                    case StorageHubHelper.SellActionName:
                        StorageHubHelper.SellAction<Pot>(shopTransaction, action, playerPlantationStorage, amount, pot);
                        break;
                }
                break;
            case EnumUtils.Entities.Seed:
                var seed = await _seedRepository.GetAsync(productId);
                switch (action)
                {
                    case StorageHubHelper.BuyActionName:
                        StorageHubHelper.BuyAction<Seed>(shopTransaction, action, playerPlantationStorage, amount, seed);
                        break;
                    case StorageHubHelper.SellActionName:
                        StorageHubHelper.SellAction<Seed>(shopTransaction, action, playerPlantationStorage, amount, seed);
                        break;
                }
                break;
            case EnumUtils.Entities.Soil:
                var soil = await _soilRepository.GetAsync(productId);
                switch (action)
                {
                    case StorageHubHelper.BuyActionName:
                        StorageHubHelper.BuyAction<Soil>(shopTransaction, action, playerPlantationStorage, amount, soil);
                        break;
                    case StorageHubHelper.SellActionName:
                        StorageHubHelper.SellAction<Soil>(shopTransaction, action, playerPlantationStorage, amount, soil);
                        break;
                }
                break;
            case EnumUtils.Entities.Water:
                var water = await _waterRepository.GetAsync(productId);
                switch (action)
                {
                    case StorageHubHelper.BuyActionName:
                        StorageHubHelper.BuyAction<Water>(shopTransaction, action, playerPlantationStorage, amount, water);
                        break;
                    case StorageHubHelper.SellActionName:
                        StorageHubHelper.SellAction<Water>(shopTransaction, action, playerPlantationStorage, amount, water);
                        break;
                }
                break;
            case EnumUtils.Entities.Bonus:
                var bonus = await _bonusRepository.GetAsync(productId);
                switch (action)
                {
                    case StorageHubHelper.BuyActionName:
                        StorageHubHelper.BuyAction<ExtendingModels.Models.Products.Bonus>(shopTransaction, action, playerPlantationStorage, amount, bonus);
                        break;
                    case StorageHubHelper.SellActionName:
                        StorageHubHelper.SellAction<ExtendingModels.Models.Products.Bonus>(shopTransaction, action, playerPlantationStorage, amount, bonus);
                        break;
                }
                break;
        }
    }
}