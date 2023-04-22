using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Castle.Core.Logging;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Comparers;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.ExtendingModels.Models.Shared;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests;

public class StructureTests : IStructureTests
{
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    private readonly IRepository<Lamp> _lampRepository;
    private readonly IRepository<Manure> _manureRepository;
    private readonly IRepository<Pot> _potRepository;
    private readonly IRepository<Seed> _seedRepository;
    private readonly IRepository<Soil> _soilRepository;
    private readonly IRepository<Water> _waterRepository;
    private readonly IRepository<Bonus> _bonusRepository;
    private readonly IRepository<Quest> _questRepository;
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    private readonly IRepository<Plant> _plantRepository;
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IRepository<Requirement> _requirementRepository;
    private readonly IRepository<Drop> _dropRepository;
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    private readonly IRepository<DropQuest> _dropQuestRepository;
    private readonly IRepository<IgnoreChange> _ignoreChangeRepository;
    private readonly IRepository<DistrictDon> _districtDonRepository;
    private readonly IRepository<BlackMarketTransaction> _blackMarketTransactionRepository;
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    private readonly IPermissionChecker _permissionChecker;
    private readonly IRepository<User, long> _userRepository;
    
    private List<User> AllUsers { get; set; }
    private List<DriedFruit> AllDriedFruits  { get; set; }
    private List<Lamp> AllLamps { get; set; }
    private List<Manure> AllManures { get; set; }
    private List<Pot> AllPots { get; set; }
    private List<Seed> AllSeeds { get; set; }
    private List<Soil> AllSoils { get; set; }
    private List<Water> AllWaters { get; set; }
    private List<Quest> AllQuests { get; set; }
    private List<Bonus> AllBonuses { get; set; }
    private List<PlantationStorage> AllPlantationStorages { get; set; }
    private List<Plant> AllPlants { get; set; }
    private List<PlayerStorage> AllPlayerStorages { get; set; }
    private List<District> AllDistricts { get; set; }
    private List<Requirement> AllRequirements { get; set; }
    private List<Drop> AllDrops { get; set; }
    private List<GeneratedType> AllGeneratedTypes { get; set; }
    private List<DropQuest> AllDropsQuests { get; set; }
    private List<QuestRequirementsProgress> AllQuestsRequirementsProgress { get; set; }
    private List<IgnoreChange> AllIgnoreChanges { get; set; }
    private List<BlackMarketTransaction> AllBlackMarketTransactions { get; set; }
    private List<DistrictDon> AllDistrictDons { get; set; }
    
    private List<User> Users { get; set; }
    private List<DriedFruit> DriedFruits  { get; set; }
    private List<Lamp> Lamps { get; set; }
    private List<Manure> Manures { get; set; }
    private List<Pot> Pots { get; set; }
    private List<Seed> Seeds { get; set; }
    private List<Soil> Soils { get; set; }
    private List<Water> Waters { get; set; }
    private List<Quest> Quests { get; set; }
    private List<Bonus> Bonuses { get; set; }
    private List<PlantationStorage> PlantationStorages { get; set; }
    private List<Plant> Plants { get; set; }
    private List<PlayerStorage> PlayerStorages { get; set; }
    private List<District> Districts { get; set; }
    private List<Requirement> Requirements { get; set; }
    private List<Drop> Drops { get; set; }
    private List<GeneratedType> GeneratedTypes { get; set; }
    private List<DropQuest> DropsQuests { get; set; }
    private List<QuestRequirementsProgress> QuestsRequirementsProgress { get; set; }
    private List<IgnoreChange> IgnoreChanges { get; set; }
    private List<BlackMarketTransaction> BlackMarketTransactions { get; set; }
    private List<DistrictDon> DistrictDons { get; set; }
        
    public ILogger Logger { get; set; }
        

    public StructureTests(
        IRepository<User, long> userRepository,
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<Lamp> lampRepository,
        IRepository<Manure> manureRepository,
        IRepository<Pot> potRepository,
        IRepository<Seed> seedRepository,
        IRepository<Soil> soilRepository,
        IRepository<Water> waterRepository,
        IRepository<Bonus> bonusRepository,
        IRepository<Quest> questRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<Plant> plantRepository,
        IRepository<PlayerStorage> playerStorageRepository,
        IRepository<District> districtRepository,
        IRepository<Requirement> requirementRepository,
        IRepository<Drop> dropRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<DropQuest> dropQuestRepository,
        IRepository<IgnoreChange> ignoreChangeRepository,
        IRepository<DistrictDon> districtDonRepository,
        IRepository<BlackMarketTransaction> blackMarketTransactionRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository,
        IPermissionChecker permissionChecker
    )
    {
        Logger = NullLogger.Instance;
        _userRepository = userRepository;
        _driedFruitRepository = driedFruitRepository;
        _lampRepository = lampRepository;
        _manureRepository = manureRepository;
        _potRepository = potRepository;
        _seedRepository = seedRepository;
        _soilRepository = soilRepository;
        _waterRepository = waterRepository;
        _bonusRepository = bonusRepository;
        _questRepository = questRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _plantRepository = plantRepository;
        _playerStorageRepository = playerStorageRepository;
        _districtRepository = districtRepository;
        _requirementRepository = requirementRepository;
        _dropRepository = dropRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _dropQuestRepository = dropQuestRepository;
        _ignoreChangeRepository = ignoreChangeRepository;
        _districtDonRepository = districtDonRepository;
        _blackMarketTransactionRepository = blackMarketTransactionRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _permissionChecker = permissionChecker;
    }
    
    public async Task<List<StructureTest>> BeginTests(bool isAdmin, long userId)
    {
        var ret =  new List<StructureTest>();

        await LoadContext(isAdmin, userId);

        if (isAdmin)
        {
            MyAppConstsList(ret);
            IgnoreChangeIsNotToBigAndEntitiesExist(ret);
        }

        UpdatePlayerDefinitionDtoFieldsList(ret);
        await WardenPermissionForDistrictsExistenceCheck(ret);
        DistrictsWardensExist(ret);
        GeneratedTypeHasConnectedOneProductDefinition(ret);
        SeedAndDriedFruitHeaveSameType(ret);
        GeneratedTypeDistrictExist(ret);
        ProductGeneratedTypeExist(ret);
        AllUsersPlantationStoragesHasProductsDefinitions(ret);
        UsersHasOnePlayerStorage(ret);
        await UsersNotHasMoreThanOnePlantationStorageForOneDistrict(ret);
        PlantationsStoragesDistrictsExists(ret);
        PlantsConnectedEntitiesExist(ret);
        CheckInUseProductsWithAllPlantsCount(ret);
        UserPlantsCountIsSameWithPotAndLampInUseCount(ret);
        AllUsersGeneratedTypesHasPlantationStorage(ret);
        QuestsRequirementsExist(ret);
        AllQuestsHasRequirementsProgress(ret);
        QuestInQuestRequirementsProgressExist(ret);
        QuestHaveOneRequirementAndDrop(ret);
        DropQuestRelationEntitiesExist(ret);
        EntitiesConnectedToBlackMarketTransactionsExist(ret);
        DonsAndDistrictExist(ret);
        DistrictsDonsCorrectlySet(ret);
        AnyDropsAndRequirementsAreAssigned(ret);    

        // zależności pól 
        RequirementFieldDependenciesAreFine(ret);
        DropFieldDependenciesAreFine(ret);
        QuestFieldDependenciesAreFine(ret);
        DriedFruitFieldDependenciesAreFine(ret);
        BonusFieldDependenciesAreFine(ret);
        ProductFieldDependenciesAreFine(ret);

        return ret;
    }

    private async Task LoadContext(bool isAdmin, long userId)
    {
        AllDistricts = await _districtRepository.GetAllListAsync();
        AllGeneratedTypes = await _generatedTypeRepository.GetAllListAsync();
        AllUsers = await _userRepository.GetAllListAsync();
        AllDriedFruits = await _driedFruitRepository.GetAllListAsync();
        AllLamps = await _lampRepository.GetAllListAsync();
        AllManures = await _manureRepository.GetAllListAsync();
        AllPots = await _potRepository.GetAllListAsync();
        AllSeeds = await _seedRepository.GetAllListAsync();
        AllSoils = await _soilRepository.GetAllListAsync();
        AllWaters = await _waterRepository.GetAllListAsync();
        AllBonuses = await _bonusRepository.GetAllListAsync();
        AllQuests = await _questRepository.GetAllListAsync();
        AllPlantationStorages = await _plantationStorageRepository.GetAllListAsync();
        AllPlants = await _plantRepository.GetAllListAsync();
        AllPlayerStorages = await _playerStorageRepository.GetAllListAsync();
        AllRequirements = await _requirementRepository.GetAllListAsync();
        AllDrops = await _dropRepository.GetAllListAsync();
        AllDropsQuests = await _dropQuestRepository.GetAllListAsync();
        AllQuestsRequirementsProgress = await _questRequirementsProgressRepository.GetAllListAsync();
        AllIgnoreChanges = await _ignoreChangeRepository.GetAllListAsync();
        AllBlackMarketTransactions = await _blackMarketTransactionRepository.GetAllListAsync();
        AllDistrictDons = await _districtDonRepository.GetAllListAsync();
            
        if (!isAdmin)
        {
            var district = AllDistricts.Single(item => item.UserId == userId);
            Districts = new List<District> {district};
            DistrictDons = AllDistrictDons.Where(item => item.DistrictId == district.Id).ToList();
            PlantationStorages = AllPlantationStorages.Where(item => item.DistrictId == district.Id).ToList();
            Users = AllUsers.Where(item => PlantationStorages.Any(storage => storage.UserId == item.Id) || item.Id == userId).ToList();
            PlayerStorages = AllPlayerStorages.Where(item => Users.Any(user => user.Id == item.UserId)).ToList();
            GeneratedTypes = AllGeneratedTypes.Where(item => item.DistrictId == district.Id).ToList();
            Requirements = AllRequirements.Where(item => item.DistrictId == district.Id).ToList();
            Drops = AllDrops.Where(item => item.DistrictId == district.Id).ToList();
            BlackMarketTransactions = AllBlackMarketTransactions.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || item.DistrictId == district.Id).ToList();

            Plants = AllPlants.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            DriedFruits = AllDriedFruits.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            Lamps = AllLamps.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            Manures = AllManures.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            Pots = AllPots.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            Seeds = AllSeeds.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            Soils = AllSoils.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            Waters = AllWaters.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            Bonuses = AllBonuses.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
            Quests = AllQuests.Where(item => GeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) || PlantationStorages.Any(type => type.Id == item.PlantationStorageId)).ToList();
                
            QuestsRequirementsProgress = AllQuestsRequirementsProgress.Where(item => Quests.Any(quest => quest.Id == item.QuestId)).ToList();
            DropsQuests = AllDropsQuests.Where(item => Quests.Any(quest => quest.Id == item.QuestId) || Drops.Any(drop => drop.Id == item.DropId)).ToList();
        }
        else
        {
            Districts = AllDistricts;
            GeneratedTypes = AllGeneratedTypes;
            Users = AllUsers;
            DriedFruits = AllDriedFruits;
            Lamps = AllLamps;
            Manures = AllManures;
            Pots = AllPots;
            Seeds = AllSeeds;
            Soils = AllSoils;
            Waters = AllWaters;
            Bonuses = AllBonuses;
            Quests = AllQuests;
            PlantationStorages = AllPlantationStorages;
            Plants = AllPlants;
            PlayerStorages = AllPlayerStorages;
            Requirements = AllRequirements;
            Drops = AllDrops;
            DropsQuests = AllDropsQuests;
            QuestsRequirementsProgress = AllQuestsRequirementsProgress;
            IgnoreChanges = AllIgnoreChanges;
            BlackMarketTransactions = AllBlackMarketTransactions;
            DistrictDons = AllDistrictDons;
        }
    }

    private void UpdatePlayerDefinitionDtoFieldsList(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Lista pól aktualizowanych u użytkowników podczas aktualizacji definicji";
        var typesCount = 0;
        try
        {
            const string nameSpace = "CzuczenLand.ExtendingFunctionalities.Services";
            var types = (from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && !string.IsNullOrWhiteSpace(t.Namespace) && t.Namespace.StartsWith(nameSpace) &&
                      !t.Namespace.Contains("SharedDto") && t.Name.EndsWith("UpdateDefinitionDto")
                select t).ToList();

            typesCount = types.Count;
            foreach (var type in types)
            {
                var minorTest = new MinorTest();
                    
                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest.Description = type.GetProperties().Aggregate(type.Name + "</br>",
                    (current, prop) => current + "</br>" + prop.Name + " - " + PlantationManagerHelper.GetHrPropName(prop));
                    
                structureTest.MinorTests.Add(minorTest);
            }
        }
        catch (Exception ex)
        {
            StructureTestsHelper.SetErrorInTest(structureTest, ex);
            Logger.Error("Blad====!!!!====", ex);
        }

        structureTest.AdditionalInfos = "Ilość klas aktualizowanych u użytkowników na zmianę definicji: " + typesCount;
        model.Add(structureTest);
    }
        
    private void MyAppConstsList(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Lista stałych aplikacji";
        var typesCount = 0;
        try
        {
            const string nameSpace = "CzuczenLand.ExtendingFunctionalities.Consts";
            var types = (from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && !string.IsNullOrWhiteSpace(t.Namespace) && t.Namespace.StartsWith(nameSpace)
                select t).ToList();

            typesCount = types.Count;
            foreach (var type in types)
            {
                var minorTest = new MinorTest();
                    
                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest.Description = type.GetFields().Aggregate(type.Name + "</br>",
                    (current, filed) => current + "</br>" + filed.Name + ": " + filed.GetValue(null));
                    
                structureTest.MinorTests.Add(minorTest);
            }
        }
        catch (Exception ex)
        {
            StructureTestsHelper.SetErrorInTest(structureTest, ex);
            Logger.Error("Blad====!!!!====", ex);
        }

        structureTest.AdditionalInfos = "Ilość klas ze stałymi: " + typesCount;
        model.Add(structureTest);
    }
        
    private void GeneratedTypeHasConnectedOneProductDefinition(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każdy typ jest zdefiniowany na jednej definicji typu generowanego";

        foreach (var generatedType in GeneratedTypes)
        {
            try
            {
                var minorTest = new MinorTest();
                    
                var seedTypeDefinitions = new List<Seed>();
                var lampTypeDefinitions = new List<Lamp>();
                var manureTypeDefinitions = new List<Manure>();
                var potTypeDefinitions = new List<Pot>();
                var soilTypeDefinitions = new List<Soil>();
                var waterTypeDefinitions = new List<Water>();
                var bonusTypeDefinitions = new List<Bonus>();
                var questTypeDefinitions = new List<Quest>();

                var entityEnum = SelectListLoaderHelper.GetEntityEnum(generatedType.EntityName);
                switch (entityEnum)
                {
                    case EnumUtils.Entities.Seed:
                        seedTypeDefinitions = Seeds.Where(item => item.PlantationStorageId == null && item.GeneratedTypeId == generatedType.Id).ToList();
                        break;
                    case EnumUtils.Entities.Lamp:
                        lampTypeDefinitions = Lamps.Where(item => item.PlantationStorageId == null && item.GeneratedTypeId == generatedType.Id).ToList();
                        break;
                    case EnumUtils.Entities.Manure:
                        manureTypeDefinitions = Manures.Where(item => item.PlantationStorageId == null && item.GeneratedTypeId == generatedType.Id).ToList();
                        break;
                    case EnumUtils.Entities.Pot:
                        potTypeDefinitions = Pots.Where(item => item.PlantationStorageId == null && item.GeneratedTypeId == generatedType.Id).ToList();
                        break;
                    case EnumUtils.Entities.Soil:
                        soilTypeDefinitions = Soils.Where(item => item.PlantationStorageId == null && item.GeneratedTypeId == generatedType.Id).ToList();
                        break;
                    case EnumUtils.Entities.Water:
                        waterTypeDefinitions = Waters.Where(item => item.PlantationStorageId == null && item.GeneratedTypeId == generatedType.Id).ToList();
                        break;
                    case EnumUtils.Entities.Quest:
                        questTypeDefinitions = Quests.Where(item => item.PlantationStorageId == null && item.GeneratedTypeId == generatedType.Id).ToList();
                        break;
                    case EnumUtils.Entities.Bonus:
                        bonusTypeDefinitions = Bonuses.Where(item => item.PlantationStorageId == null && item.GeneratedTypeId == generatedType.Id).ToList();
                        break;
                }

                var typeDefinitionsCount = seedTypeDefinitions.Count + lampTypeDefinitions.Count +
                                           manureTypeDefinitions.Count +
                                           potTypeDefinitions.Count + soilTypeDefinitions.Count +
                                           waterTypeDefinitions.Count + questTypeDefinitions.Count +
                                           bonusTypeDefinitions.Count;

                if (typeDefinitionsCount > 0)
                {
                    if (typeDefinitionsCount > 1)
                    {
                        minorTest.Description = "Typ generowany jest przypisany do więcej niż jednej definicji. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Error;

                    }
                    else
                    {
                        minorTest.Description = "Typ generowany jest przypisany do jednej definicji. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }    
                }
                else
                {
                    minorTest.Description = "Typ generowany nie jest przypisany do żadnej definicji. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                }
                    
                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość typów generowanych: " + GeneratedTypes.Count;
        model.Add(structureTest);
    }

    private void SeedAndDriedFruitHeaveSameType(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każda definicja nasiona ma jedną definicje suszu o takim samym typie";

        foreach (var seed in Seeds)
        {
            try
            {
                var minorTest = new MinorTest();
                var driedFruitsWithSameGeneratedType = DriedFruits.Where(item => item.GeneratedTypeId == seed.GeneratedTypeId && item.PlantationStorageId == null).ToList();

                if (driedFruitsWithSameGeneratedType.Count > 0)
                {
                    if (driedFruitsWithSameGeneratedType.Count > 1)
                    {
                        minorTest.Description = "Nasiono ma więcej niż jeden susz o takim samym typie. " + "Nasiono - " + seed.Name + ". Id: " + seed.Id + ". Ilość suszu o takim samym typie: " + driedFruitsWithSameGeneratedType.Count;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    }
                    else
                    {
                        minorTest.Description = "Nasiono ma jeden susz o takim samym typie. " + "Nasiono - " + seed.Name + ". Id: " + seed.Id + ". Ilość suszu o takim samym typie: " + driedFruitsWithSameGeneratedType.Count;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }
                }
                else
                {
                    minorTest.Description = "Nasiono nie ma żadnego suszu o takim samym typie. " + "Nasiono - " + seed.Name + ". Id: " + seed.Id + ". Ilość suszu o takim samym typie: " + driedFruitsWithSameGeneratedType.Count;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                    
                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość wszystkich nasion: " + Seeds.Count;
        model.Add(structureTest);
    }

    private async Task WardenPermissionForDistrictsExistenceCheck(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Test sprawdzający uprawnienia opiekuna dla dzielnic";
        try
        {
            foreach (var user in Users)
            {
                try
                {
                    var minorTest = new MinorTest();
                    var userName = user.UserName;
                    var userDistricts = Districts.Where(item => item.UserId == user.Id).ToList();
                    var isAdmin = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_Admin);
                    var isDistrictWarden = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_DistrictWarden);
                        
                    if (userDistricts.Count > 0)
                    {
                        if (userDistricts.Count > 1)
                        {
                            var userDistrictsMessage = userDistricts.Aggregate(" Dzielnice:", (current, district) => current + "</br>" + district.Name);
                            minorTest.Description = "Użytkownik posiada więcej niż jedną dzielnicę. " + "Użytkownik - " + userName + userDistrictsMessage;
                            minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        }
                        else
                        {
                            if (isDistrictWarden)
                            {
                                minorTest.Description = "Użytkownik posiada jedną dzielnicę. " + "Użytkownik - " + userName + ". Dzielnica - " + userDistricts[0].Name;
                                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                            }
                            else
                            {
                                minorTest.Description = "Użytkownik posiada jedną dzielnicę ale nie ma przypisanej roli opiekuna dzielnicy. " + "Użytkownik - " + userName + ". Dzielnica - " + userDistricts[0].Name;
                                minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                            }
                        }
                    }
                    else
                    {
                        if (isAdmin)
                        {
                            minorTest.Description = "Użytkownik nie posiada żadnej dzielnicy. " + "Użytkownik - " + userName + ". Jest administratorem";
                            minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;    
                        }
                        else if (isDistrictWarden)
                        {
                            minorTest.Description = "Użytkownik nie posiada żadnej dzielnicy. " + "Użytkownik - " + userName + ". Jest opiekunem";
                            minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        }
                        else
                        {
                            minorTest.Description = "Użytkownik nie posiada żadnej dzielnicy. " + "Użytkownik - " + userName;
                            minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;    
                        }
                    }

                    structureTest.MinorTests.Add(minorTest);
                }
                catch (Exception ex)
                {
                    StructureTestsHelper.SetErrorInTest(structureTest, ex);
                    Logger.Error("Blad====!!!!====", ex);
                }
            }

            var minorTest2 = new MinorTest();
            var districtsWithoutWarden = Districts.Where(item => item.UserId == 0).ToList();

            if (Districts.Count > 0)
            {
                if (districtsWithoutWarden.Count > 0)
                {
                    var districtsMessage = districtsWithoutWarden.Aggregate("Dzielnice:", (current, district) => current + "</br>" + district.Name);
                    minorTest2.Description = "Nie wszystkie dzielnice mają opiekunów. " + districtsMessage;
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Warn;
                }
                else
                {
                    minorTest2.Description = "Wszystkie dzielnice mają opiekunów";
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
            }
            else
            {
                minorTest2.Description = "Brak dzielnic";
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Warn;
            }

            structureTest.MinorTests.Add(minorTest2);
            structureTest.AdditionalInfos = "Ilość wszystkich dzielnic: " + Districts.Count;
        }
        catch (Exception ex)
        {
            StructureTestsHelper.SetErrorInTest(structureTest, ex);
            Logger.Error("Blad====!!!!====", ex);
        }

        model.Add(structureTest);
    }

    private void GeneratedTypeDistrictExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy dzielnice typów generowanych istnieją";

        foreach (var generatedType in GeneratedTypes)
        {
            try
            {
                var minorTest = new MinorTest();
                var generatedTypeDistrict = Districts.SingleOrDefault(item => item.Id == generatedType.DistrictId);
                if (generatedTypeDistrict != null)
                {
                    minorTest.Description = "Dzielnica istnieje. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id + ". Dzielnica - " + generatedTypeDistrict.Name;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    minorTest.Description = "Dzielnica nie istnieje. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                }
                
                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość wszystkich typów generowanych: " + GeneratedTypes.Count;
        model.Add(structureTest);
    }

    private void ProductGeneratedTypeExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy typ generowany zdefiniowany na produktach istnieje";
            
        try
        {
            StructureTestsHelper.TypeOnProductExist(structureTest, Seeds.Cast<IPlantationGeneratedEntity>().ToList(), GeneratedTypes);
            StructureTestsHelper.TypeOnProductExist(structureTest, Lamps.Cast<IPlantationGeneratedEntity>().ToList(), GeneratedTypes);
            StructureTestsHelper.TypeOnProductExist(structureTest, Manures.Cast<IPlantationGeneratedEntity>().ToList(), GeneratedTypes);
            StructureTestsHelper.TypeOnProductExist(structureTest, Pots.Cast<IPlantationGeneratedEntity>().ToList(), GeneratedTypes);
            StructureTestsHelper.TypeOnProductExist(structureTest, Soils.Cast<IPlantationGeneratedEntity>().ToList(), GeneratedTypes);
            StructureTestsHelper.TypeOnProductExist(structureTest, Waters.Cast<IPlantationGeneratedEntity>().ToList(), GeneratedTypes);
            StructureTestsHelper.TypeOnProductExist(structureTest, Quests.Cast<IPlantationGeneratedEntity>().ToList(), GeneratedTypes);
            StructureTestsHelper.TypeOnProductExist(structureTest, Bonuses.Cast<IPlantationGeneratedEntity>().ToList(), GeneratedTypes);

            var additionalMessage = "Ilość wszystkich nasion: " + Seeds.Count 
                                                                + ". Ilość wszystkich lamp: " + Lamps.Count
                                                                + ". Ilość wszystkich nawozów: " + Manures.Count 
                                                                + ". </br>Ilość wszystkich doniczek: " + Pots.Count
                                                                + ". Ilość wszystkich gleb: " + Soils.Count 
                                                                + ". Ilość całej wody: " + Waters.Count
                                                                + ". Ilość wszystkich zadań: " + Quests.Count 
                                                                + ". Ilość wszystkich bonusów: " + Bonuses.Count;
            
            structureTest.AdditionalInfos = additionalMessage;
        }
        catch (Exception ex)
        {
            StructureTestsHelper.SetErrorInTest(structureTest, ex);
            Logger.Error("Blad====!!!!====", ex);
        }
         
        model.Add(structureTest);
    }

    private void AllUsersPlantationStoragesHasProductsDefinitions(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy magazyny plantacji wszystkich użytkowników posiadają definicje typów generowanych dzielnicy";
            
        foreach (var playerStorage in PlayerStorages)
        {
            try
            {
                var userName = playerStorage.PlayerName;
                var playerPlantationStorages = PlantationStorages.Where(item => item.UserId == playerStorage.UserId);
                foreach (var plantationStorage in playerPlantationStorages)
                {
                    var district = Districts.Single(item => item.Id == plantationStorage.DistrictId);
                    var districtTypes = GeneratedTypes.Where(item => item.DistrictId == plantationStorage.DistrictId);
                        
                    foreach (var generatedType in districtTypes)
                    {
                        try
                        {
                            switch (generatedType.EntityName)
                            {
                                case EntitiesDbNames.Seed:
                                    var seedDefExist = Seeds.FirstOrDefault(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null) != null;
                                    var seeds = Seeds.Where(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == plantationStorage.Id);
                                    StructureTestsHelper.CheckUserProductsExistence(structureTest, seeds.Cast<INamedEntity>().ToList(), userName, district, generatedType, seedDefExist);
                                    break;
                                case EntitiesDbNames.Lamp:
                                    var lampDefExist = Lamps.FirstOrDefault(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null) != null;
                                    var lamps = Lamps.Where(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == plantationStorage.Id);
                                    StructureTestsHelper.CheckUserProductsExistence(structureTest, lamps.Cast<INamedEntity>().ToList(), userName, district, generatedType, lampDefExist);
                                    break;
                                case EntitiesDbNames.Manure:
                                    var manureDefExist = Manures.FirstOrDefault(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null) != null;
                                    var manures = Manures.Where(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == plantationStorage.Id);
                                    StructureTestsHelper.CheckUserProductsExistence(structureTest, manures.Cast<INamedEntity>().ToList(), userName, district, generatedType, manureDefExist);
                                    break;
                                case EntitiesDbNames.Pot:
                                    var potDefExist = Pots.FirstOrDefault(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null) != null;
                                    var pots = Pots.Where(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == plantationStorage.Id);
                                    StructureTestsHelper.CheckUserProductsExistence(structureTest, pots.Cast<INamedEntity>().ToList(), userName, district, generatedType, potDefExist);
                                    break;
                                case EntitiesDbNames.Soil:
                                    var soilDefExist = Soils.FirstOrDefault(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null) != null;
                                    var soils = Soils.Where(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == plantationStorage.Id);
                                    StructureTestsHelper.CheckUserProductsExistence(structureTest, soils.Cast<INamedEntity>().ToList(), userName, district, generatedType, soilDefExist);
                                    break;
                                case EntitiesDbNames.Water:
                                    var waterDefExist = Waters.FirstOrDefault(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null) != null;
                                    var waters = Waters.Where(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == plantationStorage.Id);
                                    StructureTestsHelper.CheckUserProductsExistence(structureTest, waters.Cast<INamedEntity>().ToList(), userName, district, generatedType, waterDefExist);
                                    break;
                                case EntitiesDbNames.Quest:
                                    var questDefExist = Quests.FirstOrDefault(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null) != null;
                                    var quests = Quests.Where(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == plantationStorage.Id);
                                    StructureTestsHelper.CheckUserProductsExistence(structureTest, quests.Cast<INamedEntity>().ToList(), userName, district, generatedType, questDefExist);
                                    break;
                                case EntitiesDbNames.Bonus:
                                    var bonusDefExist = Bonuses.FirstOrDefault(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null) != null;
                                    var bonuses = Bonuses.Where(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == plantationStorage.Id);
                                    StructureTestsHelper.CheckUserProductsExistence(structureTest, bonuses.Cast<INamedEntity>().ToList(), userName, district, generatedType, bonusDefExist);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(generatedType.EntityName);
                            }
                        }
                        catch (Exception ex)
                        {
                            StructureTestsHelper.SetErrorInTest(structureTest, ex);
                            Logger.Error("Blad====!!!!====", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość magazynów użytkownika: " + PlayerStorages.Count;
        model.Add(structureTest);
    }

    private void UsersHasOnePlayerStorage(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każdy użytkownik posiada jeden magazyn gracza";
            
        foreach (var user in Users)
        {
            try
            {
                var minorTest = new MinorTest();
                var userName = user.UserName;
                var playerStorages = PlayerStorages.Where(item => item.UserId == user.Id).ToList();

                if (playerStorages.Count > 0)
                {
                    if (playerStorages.Count > 1)
                    {
                        var storagesMessage = playerStorages.Aggregate(". Magazyny: ", (current, storage) => current + "</br>" + storage.Name);
                        minorTest.Description = "Użytkownik ma więcej niż jeden magazyn gracza. " + "Użytkownik - " + userName + storagesMessage;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    }
                    else
                    {
                        minorTest.Description = "Użytkownik posiada jeden magazyn gracza. " + "Użytkownik - " + userName;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }
                }
                else
                {
                    minorTest.Description = "Użytkownik nie posiada magazynu gracza. " + "Użytkownik - " + userName;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                }

                structureTest.MinorTests.Add(minorTest);    
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość wszystkich użytkowników: " + Users.Count;
        model.Add(structureTest);
    }

    private async Task UsersNotHasMoreThanOnePlantationStorageForOneDistrict(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każdy użytkownik nie posiada więcej niż jeden magazyn plantacji dla jednej dzielnicy";

        foreach (var user in Users)
        {
            try
            {
                var minorTest = new MinorTest();
                var userName = user.UserName;
                var userPlantationStorages = PlantationStorages.Where(item => item.UserId == user.Id).ToList();
                var withoutDuplicates = userPlantationStorages.GroupBy(item => item.DistrictId).Select(currGroup => currGroup.First()).ToList();

                if (userPlantationStorages.Count > 0)
                {
                    var storagesMessage = userPlantationStorages.Aggregate(". Magazyny plantacji użytkownika: ", (current, storage) => current + "</br>" + storage.Name);
                    if (userPlantationStorages.Count == withoutDuplicates.Count)
                    {
                        minorTest.Description = "Użytkownik nie posiada więcej niż jeden magazyn plantacji dla jednej dzielnicy. " + "Użytkownik - " + userName + storagesMessage;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }
                    else
                    {
                        var redundantStoragesCount = userPlantationStorages.Count - withoutDuplicates.Count;
                        minorTest.Description = "Użytkownik posiada więcej niż jeden magazyn plantacji dla jednej dzielnicy. " + "Użytkownik - " + userName + ". Ilość nadmiarowych magazynów plantacji: " + redundantStoragesCount + storagesMessage;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    }
                }
                else
                {
                    var isAdmin = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_Admin);
                    if (isAdmin)
                    {
                        minorTest.Description = "Użytkownik nie posiada żadnych magazynów plantacji. " + "Użytkownik - " + userName + ". Jest administratorem";
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }
                    else
                    {
                        minorTest.Description = "Użytkownik nie posiada żadnych magazynów plantacji. " + "Użytkownik - " + userName;
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Warn;   
                    }
                }
                    
                structureTest.MinorTests.Add(minorTest);    
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
           
        }

        structureTest.AdditionalInfos = "Ilość wszystkich użytkowników: " + Users.Count;
        model.Add(structureTest);
    }

    private void PlantationsStoragesDistrictsExists(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy dzielnice magazynów plantacji istnieją";

        foreach (var storage in PlantationStorages)
        {
            try
            {
                var minorTest = new MinorTest();
                var storageDistrict = Districts.SingleOrDefault(item => item.Id == storage.DistrictId);
                    
                if (storageDistrict != null)
                {
                    minorTest.Description = "Dzielnica magazynu plantacji istnieje. " + "Magazyn plantacji - " + storage.Name;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    minorTest.Description = "Dzielnica magazynu plantacji nie istnieje. " + "Magazyn plantacji - " + storage.Name;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                }
                    
                structureTest.MinorTests.Add(minorTest);   
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość wszystkich magazynów plantacji: " + PlantationStorages.Count;
        model.Add(structureTest);
    }

    private void PlantsConnectedEntitiesExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy encje połączone z roślinami istnieją";

        foreach (var plant in Plants)
        {
            try
            {
                var plantPlantationStorage = PlantationStorages.SingleOrDefault(item => item.Id == (int) plant.PlantationStorageId);
                var plantGeneratedType = GeneratedTypes.SingleOrDefault(item => item.Id == plant.GeneratedTypeId);
                var plantLamp = Lamps.SingleOrDefault(item => item.Id == plant.LampId);
                var plantSoil = Soils.SingleOrDefault(item => item.Id == plant.SoilId);
                var plantPot = Pots.SingleOrDefault(item => item.Id == plant.PotId);
                var plantSeed = Seeds.SingleOrDefault(item => item.Id == plant.SeedId);
                var plantWater = Waters.SingleOrDefault(item => item.Id == plant.WaterId);
                var plantManure = Manures.SingleOrDefault(item => item.Id == plant.ManureId);

                StructureTestsHelper.CheckPlantConnectedEntity<PlantationStorage>(structureTest, plantPlantationStorage, plant);
                StructureTestsHelper.CheckPlantConnectedEntity<GeneratedType>(structureTest, plantGeneratedType, plant);
                StructureTestsHelper.CheckPlantConnectedEntity<Lamp>(structureTest, plantLamp, plant);
                StructureTestsHelper.CheckPlantConnectedEntity<Soil>(structureTest, plantSoil, plant);
                StructureTestsHelper.CheckPlantConnectedEntity<Pot>(structureTest, plantPot, plant);
                StructureTestsHelper.CheckPlantConnectedEntity<Seed>(structureTest, plantSeed, plant);
                StructureTestsHelper.CheckPlantConnectedEntity<Water>(structureTest, plantWater, plant);
                StructureTestsHelper.CheckPlantConnectedEntity<Manure>(structureTest, plantManure, plant);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość wszystkich roślin: " + Plants.Count;
        model.Add(structureTest);
    }

    private void CheckInUseProductsWithAllPlantsCount(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy produkty w użyciu są równe ilości roślin";
        try
        {
            var minorTest1 = new MinorTest();
            var minorTest2 = new MinorTest();
            
            var allUsedPots = Pots.Sum(usedPot => usedPot.InUseCount);
            var allUsedLamps = Lamps.Sum(usedLamp => usedLamp.InUseCount);

            if (allUsedPots == Plants.Count)
            {
                minorTest1.Description = "Ilość doniczek w użyciu jest równa ilości roślin. " + " Ilość roślin: " + Plants.Count + ". Ilość doniczek w użyciu: " + allUsedPots;
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
            }
            else
            {
                minorTest1.Description = "Ilość doniczek w użyciu nie jest równa ilości roślin. " + " Ilość roślin: " + Plants.Count + ". Ilość doniczek w użyciu: " + allUsedPots;
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
            }

            if (allUsedLamps == Plants.Count)
            {
                minorTest2.Description = "Ilość lamp w użyciu jest równa ilości roślin. " + " Ilość roślin: " + Plants.Count + ". Ilość lamp w użyciu: " + allUsedLamps;
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
            }
            else
            {
                minorTest2.Description = "Ilość lamp w użyciu nie jest równa ilości roślin. " + " Ilość roślin: " + Plants.Count + ". Ilość lamp w użyciu: " + allUsedLamps;
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
            }

            structureTest.AdditionalInfos = "";
            structureTest.MinorTests.Add(minorTest1);
            structureTest.MinorTests.Add(minorTest2);
        }
        catch (Exception ex)
        {
            StructureTestsHelper.SetErrorInTest(structureTest, ex);
            Logger.Error("Blad====!!!!====", ex);
        }
       
        model.Add(structureTest);
    }
            
    private void UserPlantsCountIsSameWithPotAndLampInUseCount(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy rośliny gracza są równe ilości lamp i doniczek w użyciu";
  
        foreach (var user in Users)
        {
            try
            {
                var minorTest1 = new MinorTest();
                var minorTest2 = new MinorTest();
                    
                var userPlants = new List<Plant>();
                var userPots = new List<Pot>();
                var userLamps = new List<Lamp>();
                    
                var playerPlantationStorages = PlantationStorages.Where(currPlantationStorage => currPlantationStorage.UserId == user.Id);
                foreach (var storage in playerPlantationStorages)
                {
                    var storagePlants = Plants.Where(item => item.PlantationStorageId == storage.Id);
                    var storagePots = Pots.Where(item => item.PlantationStorageId == storage.Id);
                    var storageLamps = Lamps.Where(item => item.PlantationStorageId == storage.Id);
                    
                    userPlants.AddRange(storagePlants);
                    userPots.AddRange(storagePots);
                    userLamps.AddRange(storageLamps);
                }

                var potsInUseCount = userPots.Sum(pot => pot.InUseCount);
                var lampsInUseCount = userLamps.Sum(lamp => lamp.InUseCount);

                if (userPlants.Count == potsInUseCount)
                {
                    minorTest1.Description = "Ilość roślin użytkownika jest równa ilości doniczek w użyciu. " + "Użytkownik - " + user.UserName + 
                                             ". Ilość roślin: " + userPlants.Count + ". Ilość doniczek w użyciu: " + potsInUseCount;
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    minorTest1.Description = "Ilość roślin użytkownika nie jest równa ilości doniczek w użyciu. " + "Użytkownik - " + user.UserName + 
                                             ". Ilość roślin: " + userPlants.Count + ". Ilość doniczek w użyciu: " + potsInUseCount;
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                }
                    
                if (userPlants.Count == lampsInUseCount)
                {
                    minorTest2.Description = "Ilość roślin użytkownika jest równa ilości lamp w użyciu. " + "Użytkownik - " + user.UserName + 
                                             ". Ilość roślin: " + userPlants.Count + ". Ilość lamp w użyciu: " + lampsInUseCount;
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    minorTest2.Description = "Ilość roślin użytkownika nie jest równa ilości lamp w użyciu. " + "Użytkownik - " + user.UserName + 
                                             ". Ilość roślin: " + userPlants.Count + ". Ilość lamp w użyciu: " + lampsInUseCount;
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                }

                structureTest.MinorTests.Add(minorTest1);
                structureTest.MinorTests.Add(minorTest2);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość wszystkich użytkowników: " + Users.Count;
        model.Add(structureTest);
    }
        
    private void AllUsersGeneratedTypesHasPlantationStorage(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy wszystkie typy generowane użytkowników posiadają magazyn plantacji";
        var allTypesCount = 0;
        try
        {
            var allGeneratedEntities = new List<IPlantationGeneratedEntity>();
            allGeneratedEntities.AddRange(Seeds);
            allGeneratedEntities.AddRange(Lamps);
            allGeneratedEntities.AddRange(Manures);
            allGeneratedEntities.AddRange(Pots);
            allGeneratedEntities.AddRange(Soils);
            allGeneratedEntities.AddRange(Waters);
            allGeneratedEntities.AddRange(Quests);
            allGeneratedEntities.AddRange(Bonuses);
                
            foreach (var entity in allGeneratedEntities.Where(item => item.PlantationStorageId != null))
            {
                var minorTest = new MinorTest();

                var generatedType = GeneratedTypes.Single(item => item.Id == entity.GeneratedTypeId);
                var district = Districts.Single(item => item.Id == generatedType.DistrictId);
                var plantationStorageExist = PlantationStorages.FirstOrDefault(item => item.Id == entity.PlantationStorageId);
                    
                if (plantationStorageExist != null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest.Description = "Encja posiada magazyn plantacji. " + "Encja - " + entity.Name + ". Id: " + entity + ". Magazyn plantacji - " + plantationStorageExist.Name;    
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Encja nie posiada magazynu plantacji. " + "Encja - " + entity.Name + ". Id: " + entity + ". Dzielnica - " + district.Name;
                }
                    
                structureTest.MinorTests.Add(minorTest);
            }

            allTypesCount = allGeneratedEntities.Count;
        }
        catch (Exception ex)
        {
            StructureTestsHelper.SetErrorInTest(structureTest, ex);
            Logger.Error("Blad====!!!!====", ex);
        }

        structureTest.AdditionalInfos = "Ilość wszystkich typów razem: " + allTypesCount;
        model.Add(structureTest);
    }
        
    private void QuestsRequirementsExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy wymagania progresu wymagań zadania istnieją";
            
        foreach (var quest in Quests)
        {
            try
            {
                var questProgress = QuestsRequirementsProgress.Single(item => item.QuestId == quest.Id);
                var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
                foreach (var reqProgress in requirementsProgress)
                {
                    var minorTest = new MinorTest();
                    var req = Requirements.FirstOrDefault(item => item.Id == reqProgress.Key);
                    if (req == null)
                    {
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        minorTest.Description = "Wymaganie nie istnieje. Zadanie - " + quest.Name + ". Id wymagania: " + reqProgress.Key;
                    }
                    else
                    {
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                        minorTest.Description = "Wymaganie istnieje. Zadanie - " + quest.Name + ". Wymaganie - " + req.Name + ". Id wymagania: " + reqProgress.Key;
                    }
                    
                    structureTest.MinorTests.Add(minorTest);
                }
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość wszystkich zadań: " + Quests.Count;
        model.Add(structureTest);
    }
        
    private void AllQuestsHasRequirementsProgress(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każde zadanie ma swój progres wymagań";
            
        foreach (var quest in Quests)
        {
            try
            {
                var minorTest = new MinorTest();
                    
                var questProgresses = QuestsRequirementsProgress.Where(item => item.QuestId == quest.Id).ToList();
                if (questProgresses.Count == 0)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Zadanie nie posiada progresu wymagań. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else if (questProgresses.Count == 1)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest.Description = "Zadanie posiada jeden progres wymagań. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Zadanie posiada więcej niż jeden progres wymagań. Zadanie - " + quest.Name + ". Id: " + quest.Id + ". Ilość progresu wymagań: " + questProgresses.Count;
                }
                    
                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość wszystkich zadań: " + Quests.Count;
        model.Add(structureTest);
    }
        
    private void QuestInQuestRequirementsProgressExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy zadania progresu wymagań istnieją";
            
        foreach (var questProgress in QuestsRequirementsProgress)
        {
            try
            {
                var minorTest = new MinorTest();

                var quest = Quests.FirstOrDefault(item => item.Id == questProgress.QuestId);
                if (quest == null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Zadanie nie istnieje. Id zadania: " + questProgress.QuestId + ". Id progresu: " + questProgress.Id;
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest.Description = "Zadanie istnieje. Zadanie - " + quest.Name + ". Id zadania: " + questProgress.QuestId + ". Id progresu: " + questProgress.Id;
                }

                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość wszystkich zadań: " + Quests.Count;
        model.Add(structureTest);
    }

    private void QuestHaveOneRequirementAndDrop(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każde zadanie posiada chociaż jedno wymaganie i nagrodę";
            
        foreach (var quest in Quests)
        {
            try
            {
                var minorTest1 = new MinorTest();
                var minorTest2 = new MinorTest();
                var questProgress = QuestsRequirementsProgress.Single(item => item.QuestId == quest.Id);
                var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
                var questDefinition = Quests.Single(item => item.GeneratedTypeId == quest.GeneratedTypeId && item.PlantationStorageId == null);
                var relationsToDrop = DropsQuests.Where(item => item.QuestId == questDefinition.Id).ToList();
                    
                if (requirementsProgress == null)
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest1.Description = "Zadanie nie posiada ustawionego progresu wymagań. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else if (requirementsProgress.Keys.Count == 0)
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest1.Description = "Zadanie nie posiada żadnego wymagania. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest1.Description = "Zadanie posiada wymagania. Zadanie - " + quest.Name + ". Id: " + quest.Id + ". Ilość wymagań: " + requirementsProgress.Keys.Count;
                }
                    
                    
                if (relationsToDrop.Count == 0)
                {
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest2.Description = "Zadanie nie posiada żadnej nagrody. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else
                {
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest2.Description = "Zadanie posiada nagrody. Zadanie - " + quest.Name + ". Id: " + quest.Id + ". Ilość nagród: " + relationsToDrop.Count;
                }
                    
                structureTest.MinorTests.Add(minorTest1);
                structureTest.MinorTests.Add(minorTest2);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }

        }

        structureTest.AdditionalInfos = "Ilość wszystkich zadań: " + Quests.Count;
        model.Add(structureTest);
    }
        
    private void DropQuestRelationEntitiesExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy zadania i nagrody w relacji do siebie istnieją";
            
        foreach (var rel in DropsQuests)
        {
            try
            {
                var minorTest1 = new MinorTest();
                var minorTest2 = new MinorTest();

                var drop = Drops.FirstOrDefault(item => item.Id == rel.DropId);
                if (drop == null)
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest1.Description = "Nagroda nie istnieje. Id relacji: " + rel.Id + ". Id nagrody: " + rel.DropId;
                }
                else
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest1.Description = "Nagroda istnieje. Nagroda - " + drop.Name + ". Id relacji: " + rel.Id + ". Id nagrody: " + rel.DropId;
                }
                    
                var quest = Quests.FirstOrDefault(item => item.Id == rel.QuestId);
                if (quest == null)
                {
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest2.Description =  "Zadanie nie istnieje. Id relacji: " + rel.Id + ". Id nagrody: " + rel.DropId;
                }
                else
                {
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest2.Description = "Zadanie istnieje. Zadanie - " + quest.Name + ". Id: " + quest.Id + ". Id relacji: " + rel.Id + ". Id nagrody: " + rel.DropId;
                }
                 
                structureTest.MinorTests.Add(minorTest1);
                structureTest.MinorTests.Add(minorTest2);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość wszystkich relacji: " + DropsQuests.Count;
        model.Add(structureTest);
    }
        
    private void IgnoreChangeIsNotToBigAndEntitiesExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy ignorowane zmiany nie są zbyt duże i czy encje istnieją";

        var minorTest = new MinorTest();
        if (IgnoreChanges.Count == 0)
        {
            minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            minorTest.Description = "Brak zmian do zignorowania";
            structureTest.MinorTests.Add(minorTest);
        }
        else if (IgnoreChanges.Count > 5000)
        {
            minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
            minorTest.Description = "Za dużo zmian do zignorowania. Ilość: " + IgnoreChanges.Count;
            structureTest.MinorTests.Add(minorTest);
        }
        else
        {
            foreach (var ignoreChange in IgnoreChanges)
            {
                try
                {
                    var minorTest1 = new MinorTest();

                    Product obj;
                    switch (ignoreChange.EntityName)
                    {
                        case EntitiesDbNames.DriedFruit:
                            obj = DriedFruits.FirstOrDefault(item => item.Id == ignoreChange.EntityId);
                            break;
                        case EntitiesDbNames.Seed:
                            obj = Seeds.FirstOrDefault(item => item.Id == ignoreChange.EntityId);
                            break;
                        case EntitiesDbNames.Lamp:
                            obj = Lamps.FirstOrDefault(item => item.Id == ignoreChange.EntityId);
                            break;
                        case EntitiesDbNames.Manure:
                            obj = Manures.FirstOrDefault(item => item.Id == ignoreChange.EntityId);
                            break;
                        case EntitiesDbNames.Pot:
                            obj = Pots.FirstOrDefault(item => item.Id == ignoreChange.EntityId);
                            break;
                        case EntitiesDbNames.Soil:
                            obj = Soils.FirstOrDefault(item => item.Id == ignoreChange.EntityId);
                            break;
                        case EntitiesDbNames.Water:
                            obj = Waters.FirstOrDefault(item => item.Id == ignoreChange.EntityId);
                            break;
                        case EntitiesDbNames.Bonus:
                            obj = Bonuses.FirstOrDefault(item => item.Id == ignoreChange.EntityId);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(ignoreChange.EntityName);
                    }

                    if (obj == null)
                    {
                        minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                        minorTest1.Description = "Encja nie istnieje. Id encji: " + ignoreChange.EntityId + ". Nazwa encji - " + ignoreChange.EntityName + ". Id ignorowania: " + ignoreChange.Id;   
                    }
                    else
                    {
                        minorTest1.Status = EnumUtils.StructureTestsStatuses.Warn;
                        minorTest1.Description = "Encja istnieje. Id encji: " + ignoreChange.EntityId + ". Nazwa encji - " + ignoreChange.EntityName + ". Id ignorowania: " + ignoreChange.Id;
                    }

                    structureTest.MinorTests.Add(minorTest1);
                }
                catch (Exception ex)
                {
                    StructureTestsHelper.SetErrorInTest(structureTest, ex);
                    Logger.Error("Blad====!!!!====", ex);
                }
            }
        }
            
        structureTest.AdditionalInfos = "Ilość zmian do zignorowania: " + IgnoreChanges.Count;
        model.Add(structureTest);
    }
        
    private void DistrictsWardensExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy opiekunowie dzielnic istnieją";

        foreach (var district in Districts)
        {
            var minorTest = new MinorTest();

            try
            {
                var warden = Users.SingleOrDefault(item => item.Id == district.UserId);
                if (warden == null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Opiekun dzielnicy nie istnieje. Dzielnica - " + district.Name + ". Id: " + district.Id + ". Id użytkownika: " + district.UserId;
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest.Description = "Opiekun dzielnicy istnieje. Dzielnica - " + district.Name + ". Id: " + district.Id + ". Użytkownik - " + warden.Name + ". Id użytkownika: " + district.UserId;
                }
                
                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość wszystkich dzielnic: " + Districts.Count;
        model.Add(structureTest);
    }

    private void EntitiesConnectedToBlackMarketTransactionsExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy encje połączone do transakcji czarnego rynku istnieją";

        foreach (var transaction in BlackMarketTransactions)
        {
            try
            {
                var minorTest1 = new MinorTest();
                var minorTest2 = new MinorTest();
                var minorTest3 = new MinorTest();
                var minorTest4 = new MinorTest();
                var minorTest5 = new MinorTest();

                var transactionDistrict = Districts.FirstOrDefault(item => item.Id == transaction.DistrictId);
                var transactionGeneratedType = GeneratedTypes.FirstOrDefault(item => item.Id == transaction.GeneratedTypeId);
                var buyer = Users.FirstOrDefault(item => item.Id == transaction.BuyerId);
                var seller = Users.FirstOrDefault(item => item.Id == transaction.SellerId);

                Product obj;
                switch (transaction.ItemEntityName)
                {
                    case EntitiesDbNames.DriedFruit:
                        obj = DriedFruits.FirstOrDefault(item => item.Id == transaction.ItemId);
                        break;
                    case EntitiesDbNames.Seed:
                        obj = Seeds.FirstOrDefault(item => item.Id == transaction.ItemId);
                        break;
                    case EntitiesDbNames.Lamp:
                        obj = Lamps.FirstOrDefault(item => item.Id == transaction.ItemId);
                        break;
                    case EntitiesDbNames.Manure:
                        obj = Manures.FirstOrDefault(item => item.Id == transaction.ItemId);
                        break;
                    case EntitiesDbNames.Pot:
                        obj = Pots.FirstOrDefault(item => item.Id == transaction.ItemId);
                        break;
                    case EntitiesDbNames.Soil:
                        obj = Soils.FirstOrDefault(item => item.Id == transaction.ItemId);
                        break;
                    case EntitiesDbNames.Water:
                        obj = Waters.FirstOrDefault(item => item.Id == transaction.ItemId);
                        break;
                    case EntitiesDbNames.Bonus:
                        obj = Bonuses.FirstOrDefault(item => item.Id == transaction.ItemId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(transaction.ItemEntityName);
                }

                if (transactionDistrict == null)
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest1.Description = "Dzielnica transakcji nie istnieje. Id transakcji: " + transaction.Id + ". Id dzielnicy: " + transaction.DistrictId;    
                }
                else
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest1.Description = "Dzielnica transakcji istnieje. Id transakcji: " + transaction.Id + ". Id dzielnicy: " + transaction.DistrictId + ". Dzielnica - " + transactionDistrict.Name;
                }
                    
                if (transactionGeneratedType == null)
                {
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest2.Description = "Typ generowany transakcji nie istnieje. Id transakcji: " + transaction.Id + ". Id typu generowanego: " + transaction.GeneratedTypeId;    
                }
                else
                {
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest2.Description = "Typ generowany transakcji nie istnieje. Id transakcji: " + transaction.Id + ". Id typu generowanego: " + transaction.GeneratedTypeId + ". Typ generowany - " + transactionGeneratedType.Name;
                }
                    
                if (buyer == null)
                {
                    if (transaction.BuyerId == null)
                    {
                        minorTest3.Status = EnumUtils.StructureTestsStatuses.Ok;
                        minorTest3.Description = "Kupujący transakcję nie ustawiony. Id transakcji: " + transaction.Id;
                    }
                    else
                    {
                        minorTest3.Status = EnumUtils.StructureTestsStatuses.Error;
                        minorTest3.Description = "Kupujący transakcję ustawiony i nie znaleziony. Id transakcji: " + transaction.Id + ". Id kupującego: " + transaction.BuyerId;
                    }
                }
                else
                {
                    minorTest3.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest3.Description = "Kupujący transakcję ustawiony i znaleziony. Id transakcji: " + transaction.Id + ". Id kupującego: " + transaction.BuyerId + ". Kupujący - " + buyer.Name;
                }
                    
                if (seller == null)
                {
                    if (transaction.SellerId == null)
                    {
                        minorTest4.Status = EnumUtils.StructureTestsStatuses.Ok;
                        minorTest4.Description = "Sprzedający transakcję nie ustawiony. Id transakcji: " + transaction.Id;
                    }
                    else
                    {
                        minorTest4.Status = EnumUtils.StructureTestsStatuses.Error;
                        minorTest4.Description = "Sprzedający transakcję ustawiony i nie znaleziony. Id transakcji: " + transaction.Id + ". Id sprzedającego: " + transaction.SellerId;  
                    }
                }
                else
                {
                    minorTest4.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest4.Description = "Sprzedający transakcję ustawiony i znaleziony. Id transakcji: " + transaction.Id + ". Id sprzedającego: " + transaction.SellerId + ". Sprzedający - " + seller.Name;
                }

                if (obj == null)
                {
                    minorTest5.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest5.Description = "Przedmiot transakcji nie istnieje. Id transakcji: " + transaction.Id + ". Id przedmiotu: " + transaction.ItemId + ". Encja przedmiotu - " + transaction.ItemEntityName + ". Nazwa przedmiotu - " + transaction.ItemName;
                }
                else
                {
                    minorTest5.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest5.Description = "Przedmiot transakcji istnieje. Id transakcji: " + transaction.Id + ". Id przedmiotu: " + transaction.ItemId + ". Encja przedmiotu - " + transaction.ItemEntityName + ". Nazwa przedmiotu - " + transaction.ItemName;
                }
                    
                structureTest.MinorTests.Add(minorTest1);
                structureTest.MinorTests.Add(minorTest2);
                structureTest.MinorTests.Add(minorTest3);
                structureTest.MinorTests.Add(minorTest4);
                structureTest.MinorTests.Add(minorTest5);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość wszystkich transakcji czarnego rynku: " + BlackMarketTransactions.Count;
        model.Add(structureTest);
    }
        
    private void DonsAndDistrictExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy don'owie dzielnic istnieją i czy dzielnice don'ów istnieją";

        foreach (var don in DistrictDons)
        {
            try
            {
                var minorTest1 = new MinorTest();
                var minorTest2 = new MinorTest();

                var district = Districts.FirstOrDefault(item => item.Id == don.DistrictId);
                var donStorage = PlantationStorages.FirstOrDefault(item => item.Id == don.PlantationStorageId);

                if (district == null)
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest1.Description = "Dzielnica don'a nie istnieje. Id dzielnicy: " + don.DistrictId + ". Id magazynu plantacji: " + don.PlantationStorageId;
                }
                else
                {
                    minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest1.Description = "Dzielnica don'a istnieje. Dzielnica - " + district.Name + ". Id dzielnicy: " + don.DistrictId + ". Id magazynu plantacji: " + don.PlantationStorageId;
                }

                if (donStorage == null)
                {
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest2.Description = "Magazyn plantacji don'a nie istnieje. Id dzielnicy: " + don.DistrictId + ". Id magazynu plantacji: " + don.PlantationStorageId;
                }
                else
                {
                    minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest2.Description = "Magazyn plantacji don'a istnieje. Magazyn plantacji - " + donStorage.Name + ". Id dzielnicy: " + don.DistrictId + ". Id magazynu plantacji: " + don.PlantationStorageId;
                }
                
                structureTest.MinorTests.Add(minorTest1);
                structureTest.MinorTests.Add(minorTest2);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość don'ów dzielnic: " + DistrictDons.Count;
        model.Add(structureTest);
    }
        
    private void DistrictsDonsCorrectlySet(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy don'owie dzielnic są prawidłowo ustawieni";

        foreach (var district in Districts)
        {
            try
            {
                var minorTest = new MinorTest();
                    
                var districtPlantationStorages = PlantationStorages.Where(item => item.DistrictId == district.Id).ToList();
                if (!districtPlantationStorages.Any())
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                    minorTest.Description = "Brak magazynów plantacji";
                }
                else
                {
                    var bestPlantationStorage = districtPlantationStorages.Aggregate((i1, i2) => i1.Prestige > i2.Prestige ? i1 : i2);
                    var districtDons = DistrictDons.Where(item => item.DistrictId == district.Id).ToList();
                    var districtDonByDistrict = DistrictDons.FirstOrDefault(item => item.DistrictId == district.Id);
                    var districtDon = DistrictDons.FirstOrDefault(item => item.PlantationStorageId == bestPlantationStorage.Id && item.DistrictId == district.Id);

                    if (districtDons.Count > 1)
                    {
                        var donsMessage = districtDons.Aggregate("</br>Id magazynów plantacji nadmiarowych don'ów: ", (current, obj) => current + "</br>" + obj.PlantationStorageId);
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        minorTest.Description = "Dzielnica posiada więcej niż jednego don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + donsMessage;
                    }
                    else if (districtDon == null && districtDonByDistrict != null)
                    {
                        minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        minorTest.Description = "Gracz dzielnicy który ma najwiecej prestiżu nie jest don'em. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id plantacji ustawionej jako najlepsza: " + districtDonByDistrict.PlantationStorageId;
                    }
                    else
                    {
                        if (bestPlantationStorage.Prestige < district.PrestigeToBecomeDon)
                        {
                            if (districtDon == null)
                            {
                                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                                minorTest.Description = "Dzielnica nie posiada don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Prestiż najlepszej plantacji: " + bestPlantationStorage.Prestige;
                            }
                            else
                            {
                                minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                                minorTest.Description = "Dzielnica posiada don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Prestiż najlepszej plantacji: " + bestPlantationStorage.Prestige;
                            }
                        }
                        else
                        {
                            if (districtDon == null)
                            {
                                minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                                minorTest.Description = "Dzielnica nie posiada don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Prestiż najlepszej plantacji: " + bestPlantationStorage.Prestige;
                            }
                            else
                            {
                                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                                minorTest.Description = "Dzielnica posiada don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Prestiż najlepszej plantacji: " + bestPlantationStorage.Prestige;                        
                            }
                        }
                    }
                }

                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość dzielnic: " + Districts.Count;
        model.Add(structureTest);
    }

    private void AnyDropsAndRequirementsAreAssigned(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy nagrody i wymagania posiadają przypisania";

        foreach (var req in Requirements)
        {
            try
            {
                var minorTest = new MinorTest();

                var assigns = QuestsRequirementsProgress.Where(item =>
                    JsonConvert.DeserializeObject<Dictionary<int, decimal>>(item.RequirementsProgress)
                        .Any(currItem => currItem.Key == req.Id)).ToList();
                
                if (assigns.Count > 0)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest.Description = "Wymaganie posiada przypisania. Wymaganie - " + req.Name +
                                            ". Id wymagania: " + req.Id +
                                            ". Ilość przypisanych zadań: " +
                                            assigns.Count + ". Id przypisanych zadań: " +
                                            string.Join(", ", assigns.Select(item => item.Id));
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                    minorTest.Description = "Wymaganie nie posiada przypisania. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id;
                }

                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        foreach (var drop in Drops)
        {
            try
            {
                var minorTest = new MinorTest();

                var assigns = DropsQuests.Where(item => item.DropId == drop.Id).ToList();
                if (assigns.Count > 0)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    minorTest.Description = "Nagroda posiada przypisania. Nagroda - " + drop.Name + 
                                            ". Id nagrody: " + drop.Id +
                                            ". Ilość przypisanych zadań: " +
                                            assigns.Count + ". Id przypisanych zadań: " +
                                            string.Join(", ", assigns.Select(item => item.Id));
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                    minorTest.Description = "Nagroda nie posiada przypisania. Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
                }

                structureTest.MinorTests.Add(minorTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość wymagań: " + Requirements.Count + ". Ilość nagród: " + Drops.Count;
        model.Add(structureTest);
    }
    
    
    #region ZALEŻNOŚCI PÓL

    private void RequirementFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy wymagania mają właściwie ustawione zależności pól";

        foreach (var req in Requirements)
        {
            try
            {
                switch (req.Comparer)
                {
                    case DbComparers.OwnedAmount:
                        FieldDependenciesTestsHelper.ProcessRequirementOwnedAmountComparer(req, structureTest, GeneratedTypes);
                        break;
                    case DbComparers.Level:
                        FieldDependenciesTestsHelper.ProcessRequirementLevelComparer(req, structureTest);
                        break;
                    case DbComparers.GainedExperience:
                        FieldDependenciesTestsHelper.ProcessRequirementGainedExperienceComparer(req, structureTest);
                        break;
                    case DbComparers.Gold:
                        FieldDependenciesTestsHelper.ProcessRequirementGoldComparer(req, structureTest);
                        break;
                    case DbComparers.Prestige:
                        FieldDependenciesTestsHelper.ProcessRequirementPrestigeComparer(req, structureTest);
                        break;
                    case DbComparers.CompletedAmount:
                        FieldDependenciesTestsHelper.ProcessRequirementCompletedAmountComparer(req, structureTest, GeneratedTypes);
                        break;
                    case DbComparers.Usages:
                        FieldDependenciesTestsHelper.ProcessRequirementUsagesComparer(req, structureTest, GeneratedTypes);
                        break;
                    case DbComparers.CollectPlant:
                        FieldDependenciesTestsHelper.ProcessRequirementCollectPlantComparer(req, structureTest, GeneratedTypes);
                        break;
                    case DbComparers.RemovePlant:
                        FieldDependenciesTestsHelper.ProcessRequirementRemovePlantComparer(req, structureTest, GeneratedTypes);
                        break;
                    case DbComparers.CreatePlant:
                        FieldDependenciesTestsHelper.ProcessRequirementCreatePlantComparer(req, structureTest, GeneratedTypes);
                        break;
                    case DbComparers.SellOnBlackMarket:
                        FieldDependenciesTestsHelper.ProcessRequirementSellOnBlackMarketComparer(req, structureTest, GeneratedTypes);
                        break;
                    case DbComparers.BuyOnBlackMarket:
                        FieldDependenciesTestsHelper.ProcessRequirementBuyOnBlackMarketComparer(req, structureTest, GeneratedTypes);
                        break;
                    case DbComparers.QuestToken:
                        FieldDependenciesTestsHelper.ProcessRequirementQuestTokenComparer(req, structureTest);
                        break;
                    case DbComparers.DealerToken:
                        FieldDependenciesTestsHelper.ProcessRequirementDealerTokenComparer(req, structureTest);
                        break;
                    case DbComparers.BlackMarketToken:
                        FieldDependenciesTestsHelper.ProcessRequirementBlackMarketTokenComparer(req, structureTest);
                        break;
                    case DbComparers.DonToken:
                        FieldDependenciesTestsHelper.ProcessRequirementDonTokenComparer(req, structureTest);
                        break;
                    case DbComparers.UnlockToken:
                        FieldDependenciesTestsHelper.ProcessRequirementUnlockTokenComparer(req, structureTest);
                        break;
                    case DbComparers.Honor:
                        FieldDependenciesTestsHelper.ProcessRequirementHonorComparer(req, structureTest);
                        break;
                }
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość wymagań: " + Requirements.Count;
        model.Add(structureTest);
    }

    private void DropFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy nagrody mają właściwie ustawione zależności pól";

        foreach (var drop in Drops)
        {
            try
            {
                var fieldsHeaveValues = new Dictionary<string, bool>
                {
                    [EntitiesDbNames.GeneratedType] = false,
                    [PlantationStorageObservedFields.Gold] = false,
                    [PlantationStorageObservedFields.Prestige] = false,
                    [PlantationStorageObservedFields.QuestToken] = false,
                    [PlantationStorageObservedFields.DealerToken] = false,
                    [PlantationStorageObservedFields.BlackMarketToken] = false,
                    [PlantationStorageObservedFields.DonToken] = false,
                    [PlantationStorageObservedFields.UnlockToken] = false,
                    [PlayerStorageObservedFields.Honor] = false,
                    [PlantationStorageObservedFields.GainedExperience] = false,
                };
                
                if (drop.GeneratedTypeId != null)
                    fieldsHeaveValues[EntitiesDbNames.GeneratedType] = true;

                if (drop.Gold != null)
                    fieldsHeaveValues[PlantationStorageObservedFields.Gold] = true;
                
                if (drop.Prestige != null)
                    fieldsHeaveValues[PlantationStorageObservedFields.Prestige] = true;
                
                if (drop.QuestToken != null)
                    fieldsHeaveValues[PlantationStorageObservedFields.QuestToken] = true;
                
                if (drop.DealerToken != null)
                    fieldsHeaveValues[PlantationStorageObservedFields.DealerToken] = true;
                
                if (drop.BlackMarketToken != null)
                    fieldsHeaveValues[PlantationStorageObservedFields.BlackMarketToken] = true;
                
                if (drop.DonToken != null)
                    fieldsHeaveValues[PlantationStorageObservedFields.DonToken] = true;
                
                if (drop.UnlockToken != null)
                    fieldsHeaveValues[PlantationStorageObservedFields.UnlockToken] = true;
                
                if (drop.Honor != null)
                    fieldsHeaveValues[PlayerStorageObservedFields.Honor] = true;
                
                if (drop.Experience != null)
                    fieldsHeaveValues[PlantationStorageObservedFields.GainedExperience] = true;

                FieldDependenciesTestsHelper.ProcessDropFieldDependencies(fieldsHeaveValues, structureTest, drop, GeneratedTypes);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość nagród: " + Drops.Count;
        model.Add(structureTest);
    }

    private void QuestFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy zadania mają właściwie ustawione zależności pól";

        foreach (var quest in Quests)
        {
            try
            {
                FieldDependenciesTestsHelper.ProcessQuestFieldDependencies(quest, structureTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość zadań: " + Quests.Count;
        model.Add(structureTest);
    }

    private void DriedFruitFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy susz ma właściwie ustawione zależności pól";

        foreach (var driedFruit in DriedFruits)
        {
            try
            {
                FieldDependenciesTestsHelper.ProcessProductPricesForShopItemFieldDependencies(driedFruit, structureTest);
                FieldDependenciesTestsHelper.ProcessProductPricesForBlackMarketFieldDependencies(driedFruit, structureTest);
                FieldDependenciesTestsHelper.ProcessDriedFruitFieldDependencies(driedFruit, structureTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }
            
        structureTest.AdditionalInfos = "Ilość suszu: " + DriedFruits.Count;
        model.Add(structureTest);
    }
    
    private void BonusFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy bonus ma właściwie ustawione zależności pól";

        foreach (var bonus in Bonuses)
        {
            try
            {
                FieldDependenciesTestsHelper.ProcessProductPricesForShopItemFieldDependencies(bonus, structureTest);
                FieldDependenciesTestsHelper.ProcessProductPricesForBlackMarketFieldDependencies(bonus, structureTest);
                FieldDependenciesTestsHelper.ProcessProductOwnedAmountFieldDependencies(bonus, structureTest, GeneratedTypes);
                FieldDependenciesTestsHelper.ProcessBonusFieldDependencies(bonus, structureTest);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość bonusów: " + Bonuses.Count;
        model.Add(structureTest);
    }
    
    private void ProductFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy produkt ma właściwie ustawione zależności pól";

        var products = new List<Product>();
        products.AddRange(Lamps);
        products.AddRange(Manures);
        products.AddRange(Pots);
        products.AddRange(Seeds);
        products.AddRange(Soils);
        products.AddRange(Waters);

        foreach (var product in products)
        {
            try
            {
                FieldDependenciesTestsHelper.ProcessProductPricesForShopItemFieldDependencies(product, structureTest);
                FieldDependenciesTestsHelper.ProcessProductPricesForBlackMarketFieldDependencies(product, structureTest);
                FieldDependenciesTestsHelper.ProcessProductOwnedAmountFieldDependencies(product, structureTest, GeneratedTypes);
            }
            catch (Exception ex)
            {
                StructureTestsHelper.SetErrorInTest(structureTest, ex);
                Logger.Error("Blad====!!!!====", ex);
            }
        }

        structureTest.AdditionalInfos = "Ilość produktów (lampa, nawóz, doniczka, nasiono, gleba, woda): " + products.Count;
        model.Add(structureTest);
    }

    #endregion
}
