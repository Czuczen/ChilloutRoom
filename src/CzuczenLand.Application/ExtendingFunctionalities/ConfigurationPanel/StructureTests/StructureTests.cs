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

/// <summary>
/// Klasa zawierająca testy struktury definicji opiekunów dzielnic.
/// </summary>
public class StructureTests : IStructureTests
{
    /// <summary>
    /// Repozytorium suszu.
    /// </summary>
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    
    /// <summary>
    /// Repozytorium lamp.
    /// </summary>
    private readonly IRepository<Lamp> _lampRepository;
    
    /// <summary>
    /// Repozytorium nawozów.
    /// </summary>
    private readonly IRepository<Manure> _manureRepository;
    
    /// <summary>
    /// Repozytorium donic.
    /// </summary>
    private readonly IRepository<Pot> _potRepository;
    
    /// <summary>
    /// Repozytorium nasion.
    /// </summary>
    private readonly IRepository<Seed> _seedRepository;
    
    /// <summary>
    /// Repozytorium gleb.
    /// </summary>
    private readonly IRepository<Soil> _soilRepository;
    
    /// <summary>
    /// Repozytorium wód.
    /// </summary>
    private readonly IRepository<Water> _waterRepository;
    
    /// <summary>
    /// Repozytorium bonusów.
    /// </summary>
    private readonly IRepository<Bonus> _bonusRepository;
    
    /// <summary>
    /// Repozytorium zadań.
    /// </summary>
    private readonly IRepository<Quest> _questRepository;
    
    /// <summary>
    /// Repozytorium magazynu plantacji.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Repozytorium roślin.
    /// </summary>
    private readonly IRepository<Plant> _plantRepository;
    
    /// <summary>
    /// Repozytorium magazynu gracza.
    /// </summary>
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium wymagań.
    /// </summary>
    private readonly IRepository<Requirement> _requirementRepository;
    
    /// <summary>
    /// Repozytorium nagród.
    /// </summary>
    private readonly IRepository<Drop> _dropRepository;
    
    /// <summary>
    /// Repozytorium typów generowanych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium tabeli łączącej zadania z nagrodami.
    /// </summary>
    private readonly IRepository<DropQuest> _dropQuestRepository;
    
    /// <summary>
    /// Repozytorium ignorowanych zmian.
    /// </summary>
    private readonly IRepository<IgnoreChange> _ignoreChangeRepository;
    
    /// <summary>
    /// Repozytorium donów dzielnic.
    /// </summary>
    private readonly IRepository<DistrictDon> _districtDonRepository;
    
    /// <summary>
    /// Repozytorium transakcji na czarnym rynku.
    /// </summary>
    private readonly IRepository<BlackMarketTransaction> _blackMarketTransactionRepository;
    
    /// <summary>
    /// Repozytorium postępów w wymaganiach zadań.
    /// </summary>
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    
    /// <summary>
    /// Obiekt do sprawdzania uprawnień.
    /// </summary>
    private readonly IPermissionChecker _permissionChecker;
    
    /// <summary>
    /// Repozytorium użytkowników.
    /// </summary>
    private readonly IRepository<User, long> _userRepository;
    
    
    /// <summary>
    /// Wszyscy użytkownicy.
    /// </summary>
    private List<User> AllUsers { get; set; }
    
    /// <summary>
    /// Lista wszystkich suszów.
    /// </summary>
    private List<DriedFruit> AllDriedFruits  { get; set; }
    
    /// <summary>
    /// Lista wszystkich lamp.
    /// </summary>
    private List<Lamp> AllLamps { get; set; }
    
    /// <summary>
    /// Lista wszystkich nawozów.
    /// </summary>
    private List<Manure> AllManures { get; set; }
    
    /// <summary>
    /// Lista wszystkich donic.
    /// </summary>
    private List<Pot> AllPots { get; set; }
    
    /// <summary>
    /// Lista wszystkich nasion.
    /// </summary>
    private List<Seed> AllSeeds { get; set; }
    
    /// <summary>
    /// Lista wszystkich gleb.
    /// </summary>
    private List<Soil> AllSoils { get; set; }
    
    /// <summary>
    /// Lista wszystkich wód.
    /// </summary>
    private List<Water> AllWaters { get; set; }
    
    /// <summary>
    /// Lista wszystkich zadań.
    /// </summary>
    private List<Quest> AllQuests { get; set; }
    
    /// <summary>
    /// Lista wszystkich bonusów.
    /// </summary>
    private List<Bonus> AllBonuses { get; set; }
    
    /// <summary>
    /// Lista wszystkich magazynów plantacji.
    /// </summary>
    private List<PlantationStorage> AllPlantationStorages { get; set; }
    
    /// <summary>
    /// Lista wszystkich roślin.
    /// </summary>
    private List<Plant> AllPlants { get; set; }
    
    /// <summary>
    /// Lista wszystkich magazynów graczy.
    /// </summary>
    private List<PlayerStorage> AllPlayerStorages { get; set; }
    
    /// <summary>
    /// Lista wszystkich dzielnic.
    /// </summary>
    private List<District> AllDistricts { get; set; }
    
    /// <summary>
    /// Lista wszystkich wymagań.
    /// </summary>
    private List<Requirement> AllRequirements { get; set; }
    
    /// <summary>
    /// Lista wszystkich nagród.
    /// </summary>
    private List<Drop> AllDrops { get; set; }
    
    /// <summary>
    /// Lista wszystkich typów generowanych.
    /// </summary>
    private List<GeneratedType> AllGeneratedTypes { get; set; }
    
    /// <summary>
    /// Lista wszystkich tabel łączących zadania z nagrodami.
    /// </summary>
    private List<DropQuest> AllDropsQuests { get; set; }
    
    /// <summary>
    /// Lista wszystkich postępów w wymaganiach zadań.
    /// </summary>
    private List<QuestRequirementsProgress> AllQuestsRequirementsProgress { get; set; }
    
    /// <summary>
    /// Lista wszystkich ignorowanych zmian.
    /// </summary>
    private List<IgnoreChange> AllIgnoreChanges { get; set; }
    
    /// <summary>
    /// Lista wszystkich transakcji na czarnym rynku.
    /// </summary>
    private List<BlackMarketTransaction> AllBlackMarketTransactions { get; set; }
    
    /// <summary>
    /// Lista wszystkich donów dzielnic.
    /// </summary>
    private List<DistrictDon> AllDistrictDons { get; set; }
    
    
    /// <summary>
    /// Lista użytkowników w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<User> Users { get; set; }
    
    /// <summary>
    /// Lista suszów w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<DriedFruit> DriedFruits  { get; set; }
    
    /// <summary>
    /// Lista lamp w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Lamp> Lamps { get; set; }
    
    /// <summary>
    /// Lista nawozów w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Manure> Manures { get; set; }
    
    /// <summary>
    /// Lista donic w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Pot> Pots { get; set; }
    
    /// <summary>
    /// Lista nasion w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Seed> Seeds { get; set; }
    
    /// <summary>
    /// Lista gleb w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Soil> Soils { get; set; }
    
    /// <summary>
    /// Lista wód w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Water> Waters { get; set; }
    
    /// <summary>
    /// Lista zadań w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Quest> Quests { get; set; }
    
    /// <summary>
    /// Lista bonusów w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Bonus> Bonuses { get; set; }
    
    /// <summary>
    /// Lista magazynów plantacji w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<PlantationStorage> PlantationStorages { get; set; }
    
    /// <summary>
    /// Lista roślin w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Plant> Plants { get; set; }
    
    /// <summary>
    /// Lista magazynów gracza w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<PlayerStorage> PlayerStorages { get; set; }
    
    /// <summary>
    /// Lista dzielnic opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<District> Districts { get; set; }
    
    /// <summary>
    /// Lista wymagań w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Requirement> Requirements { get; set; }
    
    /// <summary>
    /// Lista nagród w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<Drop> Drops { get; set; }
    
    /// <summary>
    /// Lista typów generowanych w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<GeneratedType> GeneratedTypes { get; set; }
    
    /// <summary>
    /// Lista tabel łączących nagrody z zadaniami w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<DropQuest> DropsQuests { get; set; }
    
    /// <summary>
    /// Lista postępów wymagań zadań w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<QuestRequirementsProgress> QuestsRequirementsProgress { get; set; }
    
    /// <summary>
    /// Lista ignorowanych zmian dla opiekuna dzielnicy nie dostępna, tylko dla administratora.
    /// </summary>
    private List<IgnoreChange> IgnoreChanges { get; set; }
    
    /// <summary>
    /// Lista transakcji czarnego rynku w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<BlackMarketTransaction> BlackMarketTransactions { get; set; }
    
    /// <summary>
    /// Lista donów w dzielnicy opiekuna lub wszystkie jeśli administrator.
    /// </summary>
    private List<DistrictDon> DistrictDons { get; set; }
    
    
    /// <summary>
    /// Interfejs ILogger służy do rejestrowania komunikatów z aplikacji.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public ILogger Logger { get; set; }
        

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="userRepository">Repozytorium użytkowników.</param>
    /// <param name="driedFruitRepository">Repozytorium suszu.</param>
    /// <param name="lampRepository">Repozytorium lamp.</param>
    /// <param name="manureRepository">Repozytorium nawozów.</param>
    /// <param name="potRepository">Repozytorium donic.</param>
    /// <param name="seedRepository">Repozytorium nasion.</param>
    /// <param name="soilRepository">Repozytorium gleb.</param>
    /// <param name="waterRepository">Repozytorium wód.</param>
    /// <param name="bonusRepository">Repozytorium bonusów.</param>
    /// <param name="questRepository">Repozytorium zadań.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynu plantacji.</param>
    /// <param name="plantRepository">Repozytorium roślin.</param>
    /// <param name="playerStorageRepository">Repozytorium magazynu gracza.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="requirementRepository">Repozytorium wymagań.</param>
    /// <param name="dropRepository">Repozytorium nagród.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="dropQuestRepository">Repozytorium tabeli łączącej zadania z nagrodami.</param>
    /// <param name="ignoreChangeRepository">Repozytorium ignorowanych zmian.</param>
    /// <param name="districtDonRepository">Repozytorium donów dzielnic.</param>
    /// <param name="blackMarketTransactionRepository">Repozytorium transakcji na czarnym rynku.</param>
    /// <param name="questRequirementsProgressRepository">Repozytorium postępów w wymaganiach zadań.</param>
    /// <param name="permissionChecker">Obiekt do sprawdzania uprawnień.</param>
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
    
    /// <summary>
    /// Rozpoczyna testy struktury aplikacji i zwraca ich wyniki.
    /// </summary>
    /// <param name="isAdmin">Określa, czy użytkownik jest administratorem.</param>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <returns>Lista wyników testów struktury.</returns>
    public async Task<List<StructureTest>> BeginTests(bool isAdmin, long userId)
    {
        var ret =  new List<StructureTest>();

        await LoadContext(isAdmin, userId);

        if (isAdmin)
        {
            MyAppConstsList(ret);
            IgnoreChangeIsNotToBigAndEntitiesExist(ret);
        }

        UpdatePlayerRecordDtoFieldsList(ret);
        await WardenPermissionForDistrictsExistenceCheck(ret);
        DistrictsWardensExist(ret);
        GeneratedTypeHasConnectedOneProductDefinition(ret);
        SeedAndDriedFruitHeaveSameType(ret);
        GeneratedTypeDistrictExist(ret);
        ProductGeneratedTypeExist(ret);
        AllUsersPlantationStoragesHasProductsRecords(ret);
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

    /// <summary>
    /// Ładuje kontekst, inicjalizując listy obiektów na podstawie uprawnień.
    /// </summary>
    /// <param name="isAdmin">Czy użytkownik jest administratorem.</param>
    /// <param name="userId">Identyfikator użytkownika.</param>
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

    /// <summary>
    /// Wyświetli raport o liście pól aktualizowanych w rekordach użytkowników podczas aktualizacji definicji przez opiekuna dzielnicy.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu aktualizacji pól.</param>
    private void UpdatePlayerRecordDtoFieldsList(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Lista pól aktualizowanych w rekordach użytkowników podczas aktualizacji definicji";
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
                var subTest = new SubTest();
                    
                subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                subTest.Description = type.GetProperties().Aggregate(type.Name + "</br>",
                    (current, prop) => current + "</br>" + prop.Name + " - " + PlantationManagerHelper.GetHrPropName(prop));
                    
                structureTest.SubTests.Add(subTest);
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
        
    /// <summary>
    /// Generuje listę stałych aplikacji wraz z ich wartościami.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu stałych aplikacji.</param>
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
                var subTest = new SubTest();
                    
                subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                subTest.Description = type.GetFields().Aggregate(type.Name + "</br>",
                    (current, filed) => current + "</br>" + filed.Name + ": " + filed.GetValue(null));
                    
                structureTest.SubTests.Add(subTest);
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
        
    /// <summary>
    /// Sprawdza, czy każdy typ generowany ma przypisaną dokładnie jedną definicję produktu.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu powiązania typów generowanych z definicjami produktów.</param>
    private void GeneratedTypeHasConnectedOneProductDefinition(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każdy typ generowany ma zdefiniowany jeden produkt";

        foreach (var generatedType in GeneratedTypes)
        {
            try
            {
                var subTest = new SubTest();
                    
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
                        subTest.Description = "Typ generowany jest przypisany do więcej niż jednej definicji. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Error;

                    }
                    else
                    {
                        subTest.Description = "Typ generowany jest przypisany do jednej definicji. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }    
                }
                else
                {
                    subTest.Description = "Typ generowany nie jest przypisany do żadnej definicji. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                }
                    
                structureTest.SubTests.Add(subTest);
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

    /// <summary>
    /// Sprawdza, czy każda definicja nasiona ma jedną definicję suszu o takim samym typie.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu powiązania definicji nasion z definicjami suszu.</param>
    private void SeedAndDriedFruitHeaveSameType(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każda definicja nasiona ma jedną definicje suszu o takim samym typie";

        foreach (var seed in Seeds)
        {
            try
            {
                var subTest = new SubTest();
                var driedFruitsWithSameGeneratedType = DriedFruits.Where(item => item.GeneratedTypeId == seed.GeneratedTypeId && item.PlantationStorageId == null).ToList();

                if (driedFruitsWithSameGeneratedType.Count > 0)
                {
                    if (driedFruitsWithSameGeneratedType.Count > 1)
                    {
                        subTest.Description = "Nasiono ma więcej niż jeden susz o takim samym typie. " + "Nasiono - " + seed.Name + ". Id: " + seed.Id + ". Ilość suszu o takim samym typie: " + driedFruitsWithSameGeneratedType.Count;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    }
                    else
                    {
                        subTest.Description = "Nasiono ma jeden susz o takim samym typie. " + "Nasiono - " + seed.Name + ". Id: " + seed.Id + ". Ilość suszu o takim samym typie: " + driedFruitsWithSameGeneratedType.Count;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }
                }
                else
                {
                    subTest.Description = "Nasiono nie ma żadnego suszu o takim samym typie. " + "Nasiono - " + seed.Name + ". Id: " + seed.Id + ". Ilość suszu o takim samym typie: " + driedFruitsWithSameGeneratedType.Count;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                    
                structureTest.SubTests.Add(subTest);
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

    /// <summary>
    /// Testuje istnienie uprawnień opiekunów dla dzielnic w systemie.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu istnienia uprawnień opiekunów dla dzielnic.</param>
    private async Task WardenPermissionForDistrictsExistenceCheck(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Test sprawdzający uprawnienia opiekunów dla dzielnic";
        try
        {
            foreach (var user in Users)
            {
                try
                {
                    var subTest = new SubTest();
                    var userName = user.UserName;
                    var userDistricts = Districts.Where(item => item.UserId == user.Id).ToList();
                    var isAdmin = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_Admin);
                    var isDistrictWarden = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_DistrictWarden);
                        
                    if (userDistricts.Count > 0)
                    {
                        if (userDistricts.Count > 1)
                        {
                            var userDistrictsMessage = userDistricts.Aggregate(" Dzielnice:", (current, district) => current + "</br>" + district.Name);
                            subTest.Description = "Użytkownik posiada więcej niż jedną dzielnicę. " + "Użytkownik - " + userName + userDistrictsMessage;
                            subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        }
                        else
                        {
                            if (isDistrictWarden)
                            {
                                subTest.Description = "Użytkownik posiada jedną dzielnicę. " + "Użytkownik - " + userName + ". Dzielnica - " + userDistricts[0].Name;
                                subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                            }
                            else
                            {
                                subTest.Description = "Użytkownik posiada jedną dzielnicę ale nie ma przypisanej roli opiekuna dzielnicy. " + "Użytkownik - " + userName + ". Dzielnica - " + userDistricts[0].Name;
                                subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                            }
                        }
                    }
                    else
                    {
                        if (isAdmin)
                        {
                            subTest.Description = "Użytkownik nie posiada żadnej dzielnicy. " + "Użytkownik - " + userName + ". Jest administratorem";
                            subTest.Status = EnumUtils.StructureTestsStatuses.Ok;    
                        }
                        else if (isDistrictWarden)
                        {
                            subTest.Description = "Użytkownik nie posiada żadnej dzielnicy. " + "Użytkownik - " + userName + ". Jest opiekunem";
                            subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        }
                        else
                        {
                            subTest.Description = "Użytkownik nie posiada żadnej dzielnicy. " + "Użytkownik - " + userName;
                            subTest.Status = EnumUtils.StructureTestsStatuses.Ok;    
                        }
                    }

                    structureTest.SubTests.Add(subTest);
                }
                catch (Exception ex)
                {
                    StructureTestsHelper.SetErrorInTest(structureTest, ex);
                    Logger.Error("Blad====!!!!====", ex);
                }
            }

            var subTest2 = new SubTest();
            var districtsWithoutWarden = Districts.Where(item => item.UserId == 0).ToList();

            if (Districts.Count > 0)
            {
                if (districtsWithoutWarden.Count > 0)
                {
                    var districtsMessage = districtsWithoutWarden.Aggregate("Dzielnice:", (current, district) => current + "</br>" + district.Name);
                    subTest2.Description = "Nie wszystkie dzielnice mają opiekunów. " + districtsMessage;
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Warn;
                }
                else
                {
                    subTest2.Description = "Wszystkie dzielnice mają opiekunów";
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
            }
            else
            {
                subTest2.Description = "Brak dzielnic";
                subTest2.Status = EnumUtils.StructureTestsStatuses.Warn;
            }

            structureTest.SubTests.Add(subTest2);
            structureTest.AdditionalInfos = "Ilość wszystkich dzielnic: " + Districts.Count;
        }
        catch (Exception ex)
        {
            StructureTestsHelper.SetErrorInTest(structureTest, ex);
            Logger.Error("Blad====!!!!====", ex);
        }

        model.Add(structureTest);
    }

    /// <summary>
    /// Sprawdza istnienie dzielnic zdefiniowanych na typach generowanych.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu istnienia dzielnic dla typów generowanych.</param>
    private void GeneratedTypeDistrictExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy dzielnice typów generowanych istnieją";

        foreach (var generatedType in GeneratedTypes)
        {
            try
            {
                var subTest = new SubTest();
                var generatedTypeDistrict = Districts.SingleOrDefault(item => item.Id == generatedType.DistrictId);
                if (generatedTypeDistrict != null)
                {
                    subTest.Description = "Dzielnica istnieje. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id + ". Dzielnica - " + generatedTypeDistrict.Name;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    subTest.Description = "Dzielnica nie istnieje. " + "Typ generowany - " + generatedType.Name + ". Id: " + generatedType.Id;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                }
                
                structureTest.SubTests.Add(subTest);
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

    /// <summary>
    /// Sprawdza istnienie typów generowanych zdefiniowanych na produktach.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu istnienia typów generowanych na produktach.</param>
    private void ProductGeneratedTypeExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy typy generowane zdefiniowane na produktach istnieją";
            
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

    /// <summary>
    /// Sprawdza, czy magazyny plantacji wszystkich użytkowników posiadają rekordy produktów dla typów generowanych dzielnicy.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu.</param>
    private void AllUsersPlantationStoragesHasProductsRecords(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy magazyny plantacji wszystkich użytkowników posiadają rekordy produktów dla typów generowanych dzielnicy";
            
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

    /// <summary>
    /// Sprawdza, czy każdy użytkownik posiada jeden magazyn gracza.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu.</param>
    private void UsersHasOnePlayerStorage(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każdy użytkownik posiada jeden magazyn gracza";
            
        foreach (var user in Users)
        {
            try
            {
                var subTest = new SubTest();
                var userName = user.UserName;
                var playerStorages = PlayerStorages.Where(item => item.UserId == user.Id).ToList();

                if (playerStorages.Count > 0)
                {
                    if (playerStorages.Count > 1)
                    {
                        var storagesMessage = playerStorages.Aggregate(". Magazyny: ", (current, storage) => current + "</br>" + storage.Name);
                        subTest.Description = "Użytkownik ma więcej niż jeden magazyn gracza. " + "Użytkownik - " + userName + storagesMessage;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    }
                    else
                    {
                        subTest.Description = "Użytkownik posiada jeden magazyn gracza. " + "Użytkownik - " + userName;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }
                }
                else
                {
                    subTest.Description = "Użytkownik nie posiada magazynu gracza. " + "Użytkownik - " + userName;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                }

                structureTest.SubTests.Add(subTest);    
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

    /// <summary>
    /// Sprawdza, czy każdy użytkownik nie posiada więcej niż jeden magazyn plantacji dla jednej dzielnicy.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu.</param>
    private async Task UsersNotHasMoreThanOnePlantationStorageForOneDistrict(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każdy użytkownik nie posiada więcej niż jeden magazyn plantacji dla jednej dzielnicy";

        foreach (var user in Users)
        {
            try
            {
                var subTest = new SubTest();
                var userName = user.UserName;
                var userPlantationStorages = PlantationStorages.Where(item => item.UserId == user.Id).ToList();
                var withoutDuplicates = userPlantationStorages.GroupBy(item => item.DistrictId).Select(currGroup => currGroup.First()).ToList();

                if (userPlantationStorages.Count > 0)
                {
                    var storagesMessage = userPlantationStorages.Aggregate(". Magazyny plantacji użytkownika: ", (current, storage) => current + "</br>" + storage.Name);
                    if (userPlantationStorages.Count == withoutDuplicates.Count)
                    {
                        subTest.Description = "Użytkownik nie posiada więcej niż jeden magazyn plantacji dla jednej dzielnicy. " + "Użytkownik - " + userName + storagesMessage;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }
                    else
                    {
                        var redundantStoragesCount = userPlantationStorages.Count - withoutDuplicates.Count;
                        subTest.Description = "Użytkownik posiada więcej niż jeden magazyn plantacji dla jednej dzielnicy. " + "Użytkownik - " + userName + ". Ilość nadmiarowych magazynów plantacji: " + redundantStoragesCount + storagesMessage;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    }
                }
                else
                {
                    var isAdmin = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_Admin);
                    if (isAdmin)
                    {
                        subTest.Description = "Użytkownik nie posiada żadnych magazynów plantacji. " + "Użytkownik - " + userName + ". Jest administratorem";
                        subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    }
                    else
                    {
                        subTest.Description = "Użytkownik nie posiada żadnych magazynów plantacji. " + "Użytkownik - " + userName;
                        subTest.Status = EnumUtils.StructureTestsStatuses.Warn;   
                    }
                }
                    
                structureTest.SubTests.Add(subTest);    
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

    /// <summary>
    /// Sprawdza, czy dzielnice magazynów plantacji istnieją.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu.</param>
    private void PlantationsStoragesDistrictsExists(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy dzielnice magazynów plantacji istnieją";

        foreach (var storage in PlantationStorages)
        {
            try
            {
                var subTest = new SubTest();
                var storageDistrict = Districts.SingleOrDefault(item => item.Id == storage.DistrictId);
                    
                if (storageDistrict != null)
                {
                    subTest.Description = "Dzielnica magazynu plantacji istnieje. " + "Magazyn plantacji - " + storage.Name;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    subTest.Description = "Dzielnica magazynu plantacji nie istnieje. " + "Magazyn plantacji - " + storage.Name;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                }
                    
                structureTest.SubTests.Add(subTest);   
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

    /// <summary>
    /// Sprawdza, czy encje połączone z roślinami istnieją.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu.</param>
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

    /// <summary>
    /// Sprawdza, czy produkty w użyciu są równe ilości roślin.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu.</param>
    private void CheckInUseProductsWithAllPlantsCount(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy produkty w użyciu są równe ilości roślin";
        try
        {
            var subTest1 = new SubTest();
            var subTest2 = new SubTest();
            
            var allUsedPots = Pots.Sum(usedPot => usedPot.InUseCount);
            var allUsedLamps = Lamps.Sum(usedLamp => usedLamp.InUseCount);

            if (allUsedPots == Plants.Count)
            {
                subTest1.Description = "Ilość doniczek w użyciu jest równa ilości roślin. " + " Ilość roślin: " + Plants.Count + ". Ilość doniczek w użyciu: " + allUsedPots;
                subTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
            }
            else
            {
                subTest1.Description = "Ilość doniczek w użyciu nie jest równa ilości roślin. " + " Ilość roślin: " + Plants.Count + ". Ilość doniczek w użyciu: " + allUsedPots;
                subTest1.Status = EnumUtils.StructureTestsStatuses.Error;
            }

            if (allUsedLamps == Plants.Count)
            {
                subTest2.Description = "Ilość lamp w użyciu jest równa ilości roślin. " + " Ilość roślin: " + Plants.Count + ". Ilość lamp w użyciu: " + allUsedLamps;
                subTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
            }
            else
            {
                subTest2.Description = "Ilość lamp w użyciu nie jest równa ilości roślin. " + " Ilość roślin: " + Plants.Count + ". Ilość lamp w użyciu: " + allUsedLamps;
                subTest2.Status = EnumUtils.StructureTestsStatuses.Error;
            }

            structureTest.AdditionalInfos = "";
            structureTest.SubTests.Add(subTest1);
            structureTest.SubTests.Add(subTest2);
        }
        catch (Exception ex)
        {
            StructureTestsHelper.SetErrorInTest(structureTest, ex);
            Logger.Error("Blad====!!!!====", ex);
        }
       
        model.Add(structureTest);
    }
            
    /// <summary>
    /// Sprawdza, czy ilość roślin użytkownika jest równa ilości doniczek i lamp w użyciu.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu.</param>
    private void UserPlantsCountIsSameWithPotAndLampInUseCount(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy rośliny gracza są równe ilości lamp i doniczek w użyciu";
  
        foreach (var user in Users)
        {
            try
            {
                var subTest1 = new SubTest();
                var subTest2 = new SubTest();
                    
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
                    subTest1.Description = "Ilość roślin użytkownika jest równa ilości doniczek w użyciu. " + "Użytkownik - " + user.UserName + 
                                             ". Ilość roślin: " + userPlants.Count + ". Ilość doniczek w użyciu: " + potsInUseCount;
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    subTest1.Description = "Ilość roślin użytkownika nie jest równa ilości doniczek w użyciu. " + "Użytkownik - " + user.UserName + 
                                             ". Ilość roślin: " + userPlants.Count + ". Ilość doniczek w użyciu: " + potsInUseCount;
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                }
                    
                if (userPlants.Count == lampsInUseCount)
                {
                    subTest2.Description = "Ilość roślin użytkownika jest równa ilości lamp w użyciu. " + "Użytkownik - " + user.UserName + 
                                             ". Ilość roślin: " + userPlants.Count + ". Ilość lamp w użyciu: " + lampsInUseCount;
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    subTest2.Description = "Ilość roślin użytkownika nie jest równa ilości lamp w użyciu. " + "Użytkownik - " + user.UserName + 
                                             ". Ilość roślin: " + userPlants.Count + ". Ilość lamp w użyciu: " + lampsInUseCount;
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                }

                structureTest.SubTests.Add(subTest1);
                structureTest.SubTests.Add(subTest2);
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
        
    /// <summary>
    /// Sprawdza, czy wszystkie produkty generowane użytkownikom posiadają magazyn plantacji.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do której będą dodane wyniki testu.</param>
    private void AllUsersGeneratedTypesHasPlantationStorage(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy wszystkie produkty generowane użytkownikom posiadają magazyn plantacji";
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
                var subTest = new SubTest();

                var generatedType = GeneratedTypes.Single(item => item.Id == entity.GeneratedTypeId);
                var district = Districts.Single(item => item.Id == generatedType.DistrictId);
                var plantationStorageExist = PlantationStorages.FirstOrDefault(item => item.Id == entity.PlantationStorageId);
                    
                if (plantationStorageExist != null)
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest.Description = "Encja posiada magazyn plantacji. " + "Encja - " + entity.Name + ". Id: " + entity + ". Magazyn plantacji - " + plantationStorageExist.Name;    
                }
                else
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest.Description = "Encja nie posiada magazynu plantacji. " + "Encja - " + entity.Name + ". Id: " + entity + ". Dzielnica - " + district.Name;
                }
                    
                structureTest.SubTests.Add(subTest);
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
        
    /// <summary>
    /// Sprawdza, czy wymagania przypisane do zadań istnieją.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void QuestsRequirementsExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy wymagania przypisane do zadań istnieją";
            
        foreach (var quest in Quests)
        {
            try
            {
                var questProgress = QuestsRequirementsProgress.Single(item => item.QuestId == quest.Id);
                var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
                foreach (var reqProgress in requirementsProgress)
                {
                    var subTest = new SubTest();
                    var req = Requirements.FirstOrDefault(item => item.Id == reqProgress.Key);
                    if (req == null)
                    {
                        subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        subTest.Description = "Wymaganie nie istnieje. Zadanie - " + quest.Name + ". Id wymagania: " + reqProgress.Key;
                    }
                    else
                    {
                        subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                        subTest.Description = "Wymaganie istnieje. Zadanie - " + quest.Name + ". Wymaganie - " + req.Name + ". Id wymagania: " + reqProgress.Key;
                    }
                    
                    structureTest.SubTests.Add(subTest);
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
        
    /// <summary>
    /// Sprawdza, czy każde zadanie ma przypisany swój postęp wymagań.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void AllQuestsHasRequirementsProgress(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każde zadanie ma swój progres wymagań";
            
        foreach (var quest in Quests)
        {
            try
            {
                var subTest = new SubTest();
                    
                var questProgresses = QuestsRequirementsProgress.Where(item => item.QuestId == quest.Id).ToList();
                if (questProgresses.Count == 0)
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest.Description = "Zadanie nie posiada progresu wymagań. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else if (questProgresses.Count == 1)
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest.Description = "Zadanie posiada jeden progres wymagań. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest.Description = "Zadanie posiada więcej niż jeden progres wymagań. Zadanie - " + quest.Name + ". Id: " + quest.Id + ". Ilość progresu wymagań: " + questProgresses.Count;
                }
                    
                structureTest.SubTests.Add(subTest);
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
        
    /// <summary>
    /// Sprawdza, czy zadania zdefiniowane na progresach wymagań istnieją.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void QuestInQuestRequirementsProgressExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy zadania zdefiniowane na progresach wymagań istnieją";
            
        foreach (var questProgress in QuestsRequirementsProgress)
        {
            try
            {
                var subTest = new SubTest();

                var quest = Quests.FirstOrDefault(item => item.Id == questProgress.QuestId);
                if (quest == null)
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest.Description = "Zadanie nie istnieje. Id zadania: " + questProgress.QuestId + ". Id progresu: " + questProgress.Id;
                }
                else
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest.Description = "Zadanie istnieje. Zadanie - " + quest.Name + ". Id zadania: " + questProgress.QuestId + ". Id progresu: " + questProgress.Id;
                }

                structureTest.SubTests.Add(subTest);
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

    /// <summary>
    /// Sprawdza, czy każde zadanie posiada przynajmniej jedno wymaganie i przynajmniej jedną nagrodę.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void QuestHaveOneRequirementAndDrop(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy każde zadanie posiada chociaż jedno wymaganie i nagrodę";
            
        foreach (var quest in Quests)
        {
            try
            {
                var subTest1 = new SubTest();
                var subTest2 = new SubTest();
                var questProgress = QuestsRequirementsProgress.Single(item => item.QuestId == quest.Id);
                var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
                var questDefinition = Quests.Single(item => item.GeneratedTypeId == quest.GeneratedTypeId && item.PlantationStorageId == null);
                var relationsToDrop = DropsQuests.Where(item => item.QuestId == questDefinition.Id).ToList();
                    
                if (requirementsProgress == null)
                {
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest1.Description = "Zadanie nie posiada ustawionego progresu wymagań. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else if (requirementsProgress.Keys.Count == 0)
                {
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest1.Description = "Zadanie nie posiada żadnego wymagania. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else
                {
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest1.Description = "Zadanie posiada wymagania. Zadanie - " + quest.Name + ". Id: " + quest.Id + ". Ilość wymagań: " + requirementsProgress.Keys.Count;
                }
                    
                    
                if (relationsToDrop.Count == 0)
                {
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest2.Description = "Zadanie nie posiada żadnej nagrody. Zadanie - " + quest.Name + ". Id: " + quest.Id;
                }
                else
                {
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest2.Description = "Zadanie posiada nagrody. Zadanie - " + quest.Name + ". Id: " + quest.Id + ". Ilość nagród: " + relationsToDrop.Count;
                }
                    
                structureTest.SubTests.Add(subTest1);
                structureTest.SubTests.Add(subTest2);
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
        
    /// <summary>
    /// Sprawdza, czy zadania i nagrody w relacji do siebie istnieją.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void DropQuestRelationEntitiesExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy zadania i nagrody w relacji do siebie istnieją";
            
        foreach (var rel in DropsQuests)
        {
            try
            {
                var subTest1 = new SubTest();
                var subTest2 = new SubTest();

                var drop = Drops.FirstOrDefault(item => item.Id == rel.DropId);
                if (drop == null)
                {
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest1.Description = "Nagroda nie istnieje. Id relacji: " + rel.Id + ". Id nagrody: " + rel.DropId;
                }
                else
                {
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest1.Description = "Nagroda istnieje. Nagroda - " + drop.Name + ". Id relacji: " + rel.Id + ". Id nagrody: " + rel.DropId;
                }
                    
                var quest = Quests.FirstOrDefault(item => item.Id == rel.QuestId);
                if (quest == null)
                {
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest2.Description =  "Zadanie nie istnieje. Id relacji: " + rel.Id + ". Id nagrody: " + rel.DropId;
                }
                else
                {
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest2.Description = "Zadanie istnieje. Zadanie - " + quest.Name + ". Id: " + quest.Id + ". Id relacji: " + rel.Id + ". Id nagrody: " + rel.DropId;
                }
                 
                structureTest.SubTests.Add(subTest1);
                structureTest.SubTests.Add(subTest2);
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
        
    /// <summary>
    /// Sprawdza, czy ignorowane zmiany nie są zbyt duże i czy przypisane encje istnieją.
    /// Tylko dla administratora.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void IgnoreChangeIsNotToBigAndEntitiesExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy ignorowane zmiany nie są zbyt duże i czy przypisane encje istnieją";

        var subTest = new SubTest();
        if (IgnoreChanges.Count == 0)
        {
            subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            subTest.Description = "Brak zmian do zignorowania";
            structureTest.SubTests.Add(subTest);
        }
        else if (IgnoreChanges.Count > 5000)
        {
            subTest.Status = EnumUtils.StructureTestsStatuses.Error;
            subTest.Description = "Za dużo zmian do zignorowania. Ilość: " + IgnoreChanges.Count;
            structureTest.SubTests.Add(subTest);
        }
        else
        {
            foreach (var ignoreChange in IgnoreChanges)
            {
                try
                {
                    var subTest1 = new SubTest();

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
                        subTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                        subTest1.Description = "Encja nie istnieje. Id encji: " + ignoreChange.EntityId + ". Nazwa encji - " + ignoreChange.EntityName + ". Id ignorowania: " + ignoreChange.Id;   
                    }
                    else
                    {
                        subTest1.Status = EnumUtils.StructureTestsStatuses.Warn;
                        subTest1.Description = "Encja istnieje. Id encji: " + ignoreChange.EntityId + ". Nazwa encji - " + ignoreChange.EntityName + ". Id ignorowania: " + ignoreChange.Id;
                    }

                    structureTest.SubTests.Add(subTest1);
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
        
    /// <summary>
    /// Sprawdza, czy opiekunowie dzielnic istnieją.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void DistrictsWardensExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy opiekunowie dzielnic istnieją";

        foreach (var district in Districts)
        {
            var subTest = new SubTest();

            try
            {
                var warden = Users.SingleOrDefault(item => item.Id == district.UserId);
                if (warden == null)
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest.Description = "Opiekun dzielnicy nie istnieje. Dzielnica - " + district.Name + ". Id: " + district.Id + ". Id użytkownika: " + district.UserId;
                }
                else
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest.Description = "Opiekun dzielnicy istnieje. Dzielnica - " + district.Name + ". Id: " + district.Id + ". Użytkownik - " + warden.Name + ". Id użytkownika: " + district.UserId;
                }
                
                structureTest.SubTests.Add(subTest);
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

    /// <summary>
    /// Sprawdza, czy encje przypisane do transakcji czarnego rynku istnieją.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void EntitiesConnectedToBlackMarketTransactionsExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy encje przypisane do transakcji czarnego rynku istnieją";

        foreach (var transaction in BlackMarketTransactions)
        {
            try
            {
                var subTest1 = new SubTest();
                var subTest2 = new SubTest();
                var subTest3 = new SubTest();
                var subTest4 = new SubTest();
                var subTest5 = new SubTest();

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
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest1.Description = "Dzielnica transakcji nie istnieje. Id transakcji: " + transaction.Id + ". Id dzielnicy: " + transaction.DistrictId;    
                }
                else
                {
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest1.Description = "Dzielnica transakcji istnieje. Id transakcji: " + transaction.Id + ". Id dzielnicy: " + transaction.DistrictId + ". Dzielnica - " + transactionDistrict.Name;
                }
                    
                if (transactionGeneratedType == null)
                {
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest2.Description = "Typ generowany transakcji nie istnieje. Id transakcji: " + transaction.Id + ". Id typu generowanego: " + transaction.GeneratedTypeId;    
                }
                else
                {
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest2.Description = "Typ generowany transakcji nie istnieje. Id transakcji: " + transaction.Id + ". Id typu generowanego: " + transaction.GeneratedTypeId + ". Typ generowany - " + transactionGeneratedType.Name;
                }
                    
                if (buyer == null)
                {
                    if (transaction.BuyerId == null)
                    {
                        subTest3.Status = EnumUtils.StructureTestsStatuses.Ok;
                        subTest3.Description = "Kupujący transakcję nie ustawiony. Id transakcji: " + transaction.Id;
                    }
                    else
                    {
                        subTest3.Status = EnumUtils.StructureTestsStatuses.Error;
                        subTest3.Description = "Kupujący transakcję ustawiony i nie znaleziony. Id transakcji: " + transaction.Id + ". Id kupującego: " + transaction.BuyerId;
                    }
                }
                else
                {
                    subTest3.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest3.Description = "Kupujący transakcję ustawiony i znaleziony. Id transakcji: " + transaction.Id + ". Id kupującego: " + transaction.BuyerId + ". Kupujący - " + buyer.Name;
                }
                    
                if (seller == null)
                {
                    if (transaction.SellerId == null)
                    {
                        subTest4.Status = EnumUtils.StructureTestsStatuses.Ok;
                        subTest4.Description = "Sprzedający transakcję nie ustawiony. Id transakcji: " + transaction.Id;
                    }
                    else
                    {
                        subTest4.Status = EnumUtils.StructureTestsStatuses.Error;
                        subTest4.Description = "Sprzedający transakcję ustawiony i nie znaleziony. Id transakcji: " + transaction.Id + ". Id sprzedającego: " + transaction.SellerId;  
                    }
                }
                else
                {
                    subTest4.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest4.Description = "Sprzedający transakcję ustawiony i znaleziony. Id transakcji: " + transaction.Id + ". Id sprzedającego: " + transaction.SellerId + ". Sprzedający - " + seller.Name;
                }

                if (obj == null)
                {
                    subTest5.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest5.Description = "Przedmiot transakcji nie istnieje. Id transakcji: " + transaction.Id + ". Id przedmiotu: " + transaction.ItemId + ". Encja przedmiotu - " + transaction.ItemEntityName + ". Nazwa przedmiotu - " + transaction.ItemName;
                }
                else
                {
                    subTest5.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest5.Description = "Przedmiot transakcji istnieje. Id transakcji: " + transaction.Id + ". Id przedmiotu: " + transaction.ItemId + ". Encja przedmiotu - " + transaction.ItemEntityName + ". Nazwa przedmiotu - " + transaction.ItemName;
                }
                    
                structureTest.SubTests.Add(subTest1);
                structureTest.SubTests.Add(subTest2);
                structureTest.SubTests.Add(subTest3);
                structureTest.SubTests.Add(subTest4);
                structureTest.SubTests.Add(subTest5);
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
      
    /// <summary>
    /// Sprawdza, czy don'owie dzielnic istnieją i czy dzielnice don'ów istnieją.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void DonsAndDistrictExist(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy don'owie dzielnic istnieją i czy dzielnice don'ów istnieją";

        foreach (var don in DistrictDons)
        {
            try
            {
                var subTest1 = new SubTest();
                var subTest2 = new SubTest();

                var district = Districts.FirstOrDefault(item => item.Id == don.DistrictId);
                var donStorage = PlantationStorages.FirstOrDefault(item => item.Id == don.PlantationStorageId);

                if (district == null)
                {
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest1.Description = "Dzielnica don'a nie istnieje. Id dzielnicy: " + don.DistrictId + ". Id magazynu plantacji: " + don.PlantationStorageId;
                }
                else
                {
                    subTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest1.Description = "Dzielnica don'a istnieje. Dzielnica - " + district.Name + ". Id dzielnicy: " + don.DistrictId + ". Id magazynu plantacji: " + don.PlantationStorageId;
                }

                if (donStorage == null)
                {
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                    subTest2.Description = "Magazyn plantacji don'a nie istnieje. Id dzielnicy: " + don.DistrictId + ". Id magazynu plantacji: " + don.PlantationStorageId;
                }
                else
                {
                    subTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest2.Description = "Magazyn plantacji don'a istnieje. Magazyn plantacji - " + donStorage.Name + ". Id dzielnicy: " + don.DistrictId + ". Id magazynu plantacji: " + don.PlantationStorageId;
                }
                
                structureTest.SubTests.Add(subTest1);
                structureTest.SubTests.Add(subTest2);
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
        
    /// <summary>
    /// Sprawdza, czy don'owie dzielnic są prawidłowo ustawieni.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void DistrictsDonsCorrectlySet(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy don'owie dzielnic są prawidłowo ustawieni";

        foreach (var district in Districts)
        {
            try
            {
                var subTest = new SubTest();
                    
                var districtPlantationStorages = PlantationStorages.Where(item => item.DistrictId == district.Id).ToList();
                if (!districtPlantationStorages.Any())
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                    subTest.Description = "Brak magazynów plantacji";
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
                        subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        subTest.Description = "Dzielnica posiada więcej niż jednego don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + donsMessage;
                    }
                    else if (districtDon == null && districtDonByDistrict != null)
                    {
                        subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        subTest.Description = "Gracz dzielnicy który ma najwiecej prestiżu nie jest don'em. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id plantacji ustawionej jako najlepsza: " + districtDonByDistrict.PlantationStorageId;
                    }
                    else
                    {
                        if (bestPlantationStorage.Prestige < district.PrestigeToBecomeDon)
                        {
                            if (districtDon == null)
                            {
                                subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                                subTest.Description = "Dzielnica nie posiada don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Prestiż najlepszej plantacji: " + bestPlantationStorage.Prestige;
                            }
                            else
                            {
                                subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                                subTest.Description = "Dzielnica posiada don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Prestiż najlepszej plantacji: " + bestPlantationStorage.Prestige;
                            }
                        }
                        else
                        {
                            if (districtDon == null)
                            {
                                subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                                subTest.Description = "Dzielnica nie posiada don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Prestiż najlepszej plantacji: " + bestPlantationStorage.Prestige;
                            }
                            else
                            {
                                subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                                subTest.Description = "Dzielnica posiada don'a. Dzielnica - " + district.Name + ". Wymagany prestiż do zostania donem: " + district.PrestigeToBecomeDon + ". Id dzielnicy: " + district.Id + ". Najlepsza plantacja - " + bestPlantationStorage.Name + ". Id najlepszej plantacji: " + bestPlantationStorage.Id + ". Prestiż najlepszej plantacji: " + bestPlantationStorage.Prestige;                        
                            }
                        }
                    }
                }

                structureTest.SubTests.Add(subTest);
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

    /// <summary>
    /// Sprawdza, czy nagrody i wymagania posiadają przypisania.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void AnyDropsAndRequirementsAreAssigned(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy nagrody i wymagania posiadają przypisania";

        foreach (var req in Requirements)
        {
            try
            {
                var subTest = new SubTest();

                var assigns = QuestsRequirementsProgress.Where(item =>
                    JsonConvert.DeserializeObject<Dictionary<int, decimal>>(item.RequirementsProgress)
                        .Any(currItem => currItem.Key == req.Id)).ToList();
                
                if (assigns.Count > 0)
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest.Description = "Wymaganie posiada przypisania. Wymaganie - " + req.Name +
                                            ". Id wymagania: " + req.Id +
                                            ". Ilość przypisanych zadań: " +
                                            assigns.Count + ". Id przypisanych zadań: " +
                                            string.Join(", ", assigns.Select(item => item.Id));
                }
                else
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                    subTest.Description = "Wymaganie nie posiada przypisania. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id;
                }

                structureTest.SubTests.Add(subTest);
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
                var subTest = new SubTest();

                var assigns = DropsQuests.Where(item => item.DropId == drop.Id).ToList();
                if (assigns.Count > 0)
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    subTest.Description = "Nagroda posiada przypisania. Nagroda - " + drop.Name + 
                                            ". Id nagrody: " + drop.Id +
                                            ". Ilość przypisanych zadań: " +
                                            assigns.Count + ". Id przypisanych zadań: " +
                                            string.Join(", ", assigns.Select(item => item.Id));
                }
                else
                {
                    subTest.Status = EnumUtils.StructureTestsStatuses.Warn;
                    subTest.Description = "Nagroda nie posiada przypisania. Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
                }

                structureTest.SubTests.Add(subTest);
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

    /// <summary>
    /// Sprawdza, czy wymagania mają właściwie ustawione zależności pól.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
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

    /// <summary>
    /// Sprawdza, czy nagrody mają właściwie ustawione zależności pól.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
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

    /// <summary>
    /// Sprawdza, czy zadania mają właściwie ustawione zależności pól.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
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

    /// <summary>
    /// Sprawdza, czy susze mają właściwie ustawione zależności pól.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void DriedFruitFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy susze mają właściwie ustawione zależności pól";

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
    
    /// <summary>
    /// Sprawdza, czy bonusy mają właściwie ustawione zależności pól.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void BonusFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy bonusy mają właściwie ustawione zależności pól";

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
    
    /// <summary>
    /// Sprawdza, czy produkty mają właściwie ustawione zależności pól.
    /// </summary>
    /// <param name="model">Lista obiektów StructureTest, do których dodawane są wyniki testu.</param>
    private void ProductFieldDependenciesAreFine(List<StructureTest> model)
    {
        var structureTest = new StructureTest();
        structureTest.TestName = "Czy produkty mają właściwie ustawione zależności pól";

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
