using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Timing;
using Castle.Core.Logging;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator;

public class NewPlayerGenerator : INewPlayerGenerator
{
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    private readonly IRepository<Lamp> _lampRepository;
    private readonly IRepository<Manure> _manureRepository;
    private readonly IRepository<Soil> _soilRepository;
    private readonly IRepository<Water> _waterRepository;
    private readonly IRepository<Bonus> _bonusRepository;
    private readonly IRepository<Pot> _potRepository;
    private readonly IRepository<Seed> _seedRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    private readonly IRepository<Quest> _questRepository;
    private readonly IQuestService _questService;
    private readonly IPermissionChecker _permissionChecker;

    public ILogger Logger { get; set; }
        
    
    public NewPlayerGenerator(
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<Lamp> lampRepository,
        IRepository<Manure> manureRepository,
        IRepository<Soil> soilRepository,
        IRepository<Water> waterRepository,
        IRepository<Bonus> bonusRepository,
        IRepository<Pot> potRepository,
        IRepository<Seed> seedRepository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<Quest> questRepository,
        IQuestService questService,
        IPermissionChecker permissionChecker
    )
    {
        Logger = NullLogger.Instance;
        _driedFruitRepository = driedFruitRepository;
        _lampRepository = lampRepository;
        _manureRepository = manureRepository;
        _soilRepository = soilRepository;
        _waterRepository = waterRepository;
        _bonusRepository = bonusRepository;
        _potRepository = potRepository;
        _seedRepository = seedRepository;
        _districtRepository = districtRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _questRepository = questRepository;
        _questService = questService;
        _permissionChecker = permissionChecker;
    }

    public async Task<Plantation> GetOrInitPlayerResources(bool heWantPayForHollow, PlayerStorage playerStorage, User user)
    {
        var ret = new Plantation();
        
        var dateTimeNow = Clock.Now;
        var aWeekBeforeStart = dateTimeNow + TimeSpan.FromDays(7); // informacje o dzielnicy będą widoczne tydzień przed jej startem
        
        var lastSelectedDistrictId = playerStorage.LastSelectedDistrictId;
        var allDistricts = await _districtRepository.GetAllListAsync();
        ret.PlayerStorage = playerStorage;
        
        ret.AvailableDistricts = allDistricts.Where(item =>
            (item.StartTime <= aWeekBeforeStart && item.IsDefined && 
             item.EndTime == null) ||
            (item.StartTime <= aWeekBeforeStart && item.IsDefined &&
             item.EndTime != null && item.EndTime > dateTimeNow)).ToList(); // jeśli opiekun dzielnicy chce testować dzielnicę musi ustawić ją na isDefined = true żeby pracowały workery ale jeśli nie chce aby użytkownicy widzieli info o dzielnicy StartTime musi ustawi co najmniej na tydzień do przodu

        var adminPermission = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_Admin);
        var wardenPermission = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_DistrictWarden);
        var wardenDistrict = allDistricts.SingleOrDefault(item => item.UserId == playerStorage.UserId);
            
        var isDistrictWarden = wardenPermission && wardenDistrict != null;
        var onlyWardenPermission = wardenPermission && wardenDistrict == null;

        var districts = adminPermission || onlyWardenPermission
            ? new List<District>()
            : isDistrictWarden
                ? new List<District> {wardenDistrict}
                : allDistricts.Where(item =>
                    (item.StartTime <= dateTimeNow && item.IsDefined &&
                     item.PlayerLevelRequirement <= playerStorage.Level &&
                     item.EndTime == null) ||
                    (item.StartTime <= dateTimeNow && item.IsDefined &&
                     item.PlayerLevelRequirement <= playerStorage.Level &&
                     item.EndTime != null && item.EndTime > dateTimeNow)).ToList();

        if (lastSelectedDistrictId == null || lastSelectedDistrictId == 0)
        {
            ret.IsNewPlayer = true;
            ret.S2DistrictsList = NewPlayerGeneratorHelper.GetS2DistrictsList(districts, new List<District>());
        }
        else
            await GetOrInitForSelectedDistrict(ret, heWantPayForHollow, isDistrictWarden, districts, allDistricts);

        return ret;
    }

    private async Task GetOrInitForSelectedDistrict(Plantation plantation, bool heWantPayForHollow, bool isDistrictWarden, List<District> districts, List<District> allDistricts)
    {
        plantation.SelectedDistrictId = plantation.PlayerStorage.LastSelectedDistrictId;
        
        try
        {
            plantation.District = await _districtRepository.GetAsync((int) plantation.SelectedDistrictId);
        }
        catch (Exception ex)
        {
            if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity"))
            {
                plantation.DistrictLoadingMessage = "Ostatnio wybrana dzielnica została usunięta";
                Logger.Warn("Ostatnio wybrana dzielnica została usunięta.", ex);
            }
            else
                Logger.Error("Blad =======//========", ex);
        }

        var district = plantation.District;
        if (district != null)
        {
            if (district.PlayerLevelRequirement <= plantation.PlayerStorage.Level)
            {
                await CreateAndSetHollowIfPossible(heWantPayForHollow, isDistrictWarden, plantation);
                await SetS2DistrictsList(districts, plantation, allDistricts);
            }
            else
            {
                plantation.TooLowLevel = true;
                plantation.DistrictLoadingMessage = "Zbyt niski poziom gracza. Wymagany poziom to " + district.PlayerLevelRequirement;
            }
        }
        else
        {
            await SetS2DistrictsList(districts, plantation, allDistricts);
        }
    }
        
    private async Task CreateAndSetHollowIfPossible(bool heWantPayForHollow, bool isDistrictWarden, Plantation plantation)
    {
        var clockNow = Clock.Now;
        var district = plantation.District;

        if (!district.IsDefined && !isDistrictWarden)
        {
            plantation.DistrictLoadingMessage = "Dzielnica wyłączona";
            return;
        }
            
        if (district.EndTime < clockNow && !isDistrictWarden)
        {
            plantation.DistrictLoadingMessage = "Czas dzielnicy minął";
            return;
        }
            
        if (district.StartTime > clockNow && !isDistrictWarden)
        {
            plantation.DistrictLoadingMessage = "Czas dzielnicy jeszcze nie nadszedł";
            return;
        }
            
        plantation.PlantationStorage = (await _plantationStorageRepository.GetAllListAsync(item =>
            item.DistrictId == district.Id && item.UserId == plantation.PlayerStorage.UserId)).SingleOrDefault();
            
        if (plantation.PlantationStorage == null && (district.HollowPrice == 0 || heWantPayForHollow))
        {
            var hollowPrice = district.HollowPrice;
            
            if (plantation.PlayerStorage.Gold >= hollowPrice)
                await CreateHollow(plantation);
            else
            {
                plantation.DistrictLoadingMessage = "Nie stać Cię";        
                plantation.HollowPrice = hollowPrice;
            }
        }
        else if (plantation.PlantationStorage == null)
        {
            var hollowCost = new DisplayStrategy().ParseValue(EnumUtils.PropTypes.Decimal, district.HollowPrice);
            
            plantation.MustBuyHollow = true;
            plantation.DistrictLoadingMessage = "Chcesz kupić dziuple w dzielnicy " + district.Name + " za " + hollowCost + "$?";
            plantation.HollowPrice = district.HollowPrice;
        }
    }

    private async Task CreateHollow(Plantation plantation)
    {
        plantation.PlayerStorage.Gold -= plantation.District.HollowPrice;
        plantation.PlantationStorage = await InitNewPlayerPlantationStorageForDistrict(plantation);
        
        if (plantation.PlantationStorage != null)
            await InitNewPlayerEntitiesDefinitions(plantation);
        else
            throw new AbpException("Tworzenie magazynu plantacji nie powiodło się");
    }
        
    private async Task<PlantationStorage> InitNewPlayerPlantationStorageForDistrict(Plantation plantation)
    {
        var plantationStorageToCreate = new PlantationStorage
        {
            Name = "Plantacja użytkownika - " + plantation.PlayerStorage.PlayerName + " Dzielnica - " + plantation.District.Name,
            Gold = plantation.District.StartGold,
            Level = 0,
            CurrExp = 0,
            ExpToNextLevel = plantation.District.StartExpToNextLevel,
            GainedExperience = 0,
            UserId = plantation.PlayerStorage.UserId,
            DistrictId = plantation.District.Id,
            UnlockedDailyQuestsCount = plantation.District.UnlockedDailyQuestsOnStart,
            UnlockedWeeklyQuestsCount = plantation.District.UnlockedWeeklyQuestsOnStart,
            UnlockedBuffsSlots = plantation.District.UnlockedBuffsSlotsOnStart,
            UnlockedArtifactSlots = plantation.District.UnlockedArtifactSlotsOnStart,
            MaxDailyQuestsCount = plantation.District.MaxDailyQuestsCount,
            MaxArtifactSlots = plantation.District.MaxArtifactSlots,
            MaxWeeklyQuestsCount = plantation.District.MaxWeeklyQuestsCount,
            MaxBuffsSlots = plantation.District.MaxBuffsSlots
        };

        var id = await _plantationStorageRepository.InsertAndGetIdAsync(plantationStorageToCreate);
        var plantationStorage = await _plantationStorageRepository.GetAsync(id);

        return plantationStorage;
    }
        
    private async Task InitNewPlayerEntitiesDefinitions(Plantation plantation)
    {
        var districtTypes = await _generatedTypeRepository.GetAllListAsync(currItem => currItem.DistrictId == plantation.District.Id);
            
        var driedFruitsDefinitions = (await _driedFruitRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        await GenerateAllDefinitionsForNewPlayerPlantationStorage(driedFruitsDefinitions, EnumUtils.Entities.DriedFruit, plantation, _driedFruitRepository);
            
        var lampsDefinitions = (await _lampRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        await GenerateAllDefinitionsForNewPlayerPlantationStorage(lampsDefinitions,  EnumUtils.Entities.Lamp, plantation, _lampRepository);
            
        var manuresDefinitions = (await _manureRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        await GenerateAllDefinitionsForNewPlayerPlantationStorage(manuresDefinitions, EnumUtils.Entities.Manure, plantation, _manureRepository);
            
        var potsDefinitions = (await _potRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        await GenerateAllDefinitionsForNewPlayerPlantationStorage(potsDefinitions, EnumUtils.Entities.Pot, plantation, _potRepository);
            
        var seedsDefinitions = (await _seedRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        await GenerateAllDefinitionsForNewPlayerPlantationStorage(seedsDefinitions, EnumUtils.Entities.Seed, plantation, _seedRepository);
            
        var soilsDefinitions = (await _soilRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        await GenerateAllDefinitionsForNewPlayerPlantationStorage(soilsDefinitions, EnumUtils.Entities.Soil, plantation, _soilRepository);
            
        var watersDefinitions = (await _waterRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        await GenerateAllDefinitionsForNewPlayerPlantationStorage(watersDefinitions, EnumUtils.Entities.Water, plantation, _waterRepository);
            
        var bonusesDefinitions = (await _bonusRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        await GenerateAllDefinitionsForNewPlayerPlantationStorage(bonusesDefinitions, EnumUtils.Entities.Bonus, plantation, _bonusRepository);
            
        var questsDefinitions = (await _questRepository.GetAllListAsync()).Where(item => 
            item.PlantationStorageId == null && districtTypes.Any(type => type.Id == item.GeneratedTypeId)).ToList();
        var createdDefinitionsForUser = await GenerateAllDefinitionsForNewPlayerPlantationStorage(questsDefinitions,
            EnumUtils.Entities.Quest, plantation, _questRepository, true);

        var playerQuestsDefinitions = createdDefinitionsForUser.Cast<Quest>().ToList();
        await _questService.CreatePlayerQuestsRequirementsProgress(questsDefinitions, playerQuestsDefinitions);
        await _questService.SetStartValuesAsync(playerQuestsDefinitions);
    }

    private static async Task<List<object>> GenerateAllDefinitionsForNewPlayerPlantationStorage<TEntity>(object definitions, 
        EnumUtils.Entities? entityEnum, Plantation plantation, IRepository<TEntity, int> repo, bool needDefinitions = false)
        where TEntity : class, IEntity<int>
    {
        var ret = new List<object>();
            
        var defsAsList = ((IEnumerable) definitions).Cast<object>().ToList();
        foreach (var definition in defsAsList)
        {
            var serializeForCreateNewObject = JsonConvert.SerializeObject(definition);
            var newObject = NewPlayerGeneratorHelper.GetNewObjectByDefinition<TEntity>(serializeForCreateNewObject,
                plantation.PlantationStorage.Id, entityEnum, plantation.PlayerStorage.Id);

            if (needDefinitions)
            {
                var id = await repo.InsertAndGetIdAsync(newObject);
                var newItem = await repo.GetAsync(id);
                ret.Add(newItem);   
            }
            else
                await repo.InsertAsync(newObject);
        }

        return ret;
    }

    private async Task SetS2DistrictsList(List<District> districts, Plantation plantation, List<District> allDistricts)
    {
        var playerDistricts = new List<District>();
        var playerPlantationStorages = await _plantationStorageRepository.GetAllListAsync(currPlantationStorage =>
            currPlantationStorage.UserId == plantation.PlayerStorage.UserId);

        foreach (var storage in playerPlantationStorages)
        {
            try
            {
                var district = allDistricts.SingleOrDefault(currDistrict => currDistrict.Id == storage.DistrictId && currDistrict.PlayerLevelRequirement <= plantation.PlayerStorage.Level);
                if (district != null)
                    playerDistricts.Add(district);
            }
            catch (Exception ex)
            {
                Logger.Error("Blad======//=======", ex);
            }
        }

        plantation.S2DistrictsList = NewPlayerGeneratorHelper.GetS2DistrictsList(districts, playerDistricts);
    }
}