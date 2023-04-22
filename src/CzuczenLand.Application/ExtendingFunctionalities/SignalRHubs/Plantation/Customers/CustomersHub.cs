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

[AbpMvcAuthorize]
public class CustomersHub : Hub, ITransientDependency
{
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IPlantationStorageService _plantationStorageService;

    /// <summary>
    /// Musi być public i musi mieć setter
    /// </summary>
    public IAbpSession AbpSession { get; set; }
        
    public ILogger Logger { get; set; }
    
    
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

    [UnitOfWork]
    public virtual async Task<object> SellDriedFruit(int typeId, decimal amount, Guid offerId)
    {
        try
        {
            object ret;
            var userId = AbpSession.GetUserId();
            var playerPlantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(userId);
            var district = await _districtRepository.GetAsync(playerPlantationStorage.DistrictId);
            var generatedType = await _generatedTypeRepository.GetAsync(typeId);
            var currPlayerDriedFruit = await _driedFruitRepository.SingleAsync(item => item.PlantationStorageId == playerPlantationStorage.Id && item.GeneratedTypeId == generatedType.Id);
            
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