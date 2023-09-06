using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Customers.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Customers;

/// <summary>
/// Hub SignalR obsługujący strefę klienta.
/// [AbpAuthorize] dawało wyjątek - Abp.Authorization.AbpAuthorizationException: Current user did not login to the application!
/// Nic nie psuł ale jednak wyskakiwał. W każdym hub'ie wyskakiwał. Użycie [AbpMvcAuthorize] rozwiązało problem.
/// </summary>
[AbpMvcAuthorize]
public class CustomersHub : Hub, ITransientDependency
{
    /// <summary>
    /// Repozytorium przechowujące typ generowany w bazie danych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium przechowujące susz w bazie danych.
    /// </summary>
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    
    /// <summary>
    /// Repozytorium przechowujące dzielnice w bazie danych.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Serwis do zarządzania magazynami plantacji.
    /// </summary>
    private readonly IPlantationStorageService _plantationStorageService;

    
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
    /// <param name="generatedTypeRepository">Repozytorium przechowujące typ generowany w bazie danych.</param>
    /// <param name="driedFruitRepository">Repozytorium przechowujące susz w bazie danych.</param>
    /// <param name="districtRepository">Repozytorium przechowujące dzielnice w bazie danych.</param>
    /// <param name="plantationStorageService">Serwis do zarządzania magazynami plantacji.</param>
    public CustomersHub(
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<District> districtRepository,
        IPlantationStorageService plantationStorageService
    )
    {
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
            
        _generatedTypeRepository = generatedTypeRepository;
        _driedFruitRepository = driedFruitRepository;
        _districtRepository = districtRepository;
        _plantationStorageService = plantationStorageService;
    }

    /// <summary>
    /// Metoda pozwalająca na sprzedaż suszu dla danego użytkownika na podstawie id typu suszu i ilości.
    /// Sprzedaż suszu klientowi przez użytkownika. 
    /// </summary>
    /// <param name="typeId">Id typu generowanego suszu.</param>
    /// <param name="amount">Ilość sprzedawanego suszu.</param>
    /// <param name="offerId">Id oferty kupna.</param>
    /// <returns>Obiekt zawierający informacje o sprzedaży.</returns>
    [UnitOfWork]
    public virtual async Task<object> SellDriedFruit(int typeId, decimal amount, Guid offerId)
    {
        try
        {
            object ret;
            var userId = AbpSession.GetUserId();
            var playerPlantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(userId);
            var district = await _districtRepository.GetAsync(playerPlantationStorage.DistrictId);
            var currPlayerDriedFruit = await _driedFruitRepository.SingleAsync(item =>
                item.PlantationStorageId == playerPlantationStorage.Id && item.GeneratedTypeId == typeId);
            
            if (currPlayerDriedFruit.OwnedAmount >= amount)
            {
                currPlayerDriedFruit.OwnedAmount -= amount;
                var earnedCash = amount * (decimal) currPlayerDriedFruit.CustomerZonePrice;
                playerPlantationStorage.Gold += decimal.Parse(earnedCash.ToString("0.##"));

                TokensOperator.DealerTokenProfit(district, playerPlantationStorage, amount);
                var driedFruitAmount = currPlayerDriedFruit.OwnedAmount;
                var driedFruitId = currPlayerDriedFruit.Id;
                var plantationGold = playerPlantationStorage.Gold;
                var sellMessage = "Sprzedano " + amount + "g " + currPlayerDriedFruit.Name;

                ret = new CustomersSellData
                {
                    SellMessage = sellMessage, DriedFruitAmount = driedFruitAmount,
                    PlantationGold = plantationGold, DriedFruitId = driedFruitId, Status = "Success"
                };
                    
                Clients.All.removeOffer(offerId);
            }
            else
            {
                ret = new CustomersSellData{ SellMessage = "Za mało suszu", Status = "Error" };
            }
            
            return ret;
        }
        catch (Exception ex)
        {
            Logger.Error("Blad =======//========", ex);
            Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);
            return null;
        }
    }
}