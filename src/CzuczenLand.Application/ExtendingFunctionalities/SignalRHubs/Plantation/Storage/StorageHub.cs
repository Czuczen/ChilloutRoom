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

[AbpMvcAuthorize]
public class StorageHub : Hub, ITransientDependency
{
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    private readonly IRepository<Lamp> _lampRepository;
    private readonly IRepository<Manure> _manureRepository;
    private readonly IRepository<Soil> _soilRepository;
    private readonly IRepository<Water> _waterRepository;
    private readonly IRepository<Pot> _potRepository;
    private readonly IRepository<Seed> _seedRepository;
    private readonly IRepository<ExtendingModels.Models.Products.Bonus> _bonusRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IPlantationStorageService _plantationStorageService;
    private readonly IPlayerStorageService _playerStorageService;

    /// <summary>
    /// Musi być public i musi mieć setter
    /// </summary>
    public IAbpSession AbpSession { get; set; }
    
    public ILogger Logger { get; set; }
    
    
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