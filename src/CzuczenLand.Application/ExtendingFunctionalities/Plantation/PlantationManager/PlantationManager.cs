using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.Timing;
using Castle.Core.Logging;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Db;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Comparers;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Conditions;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.CustomEntity;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Quest;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.ExtendingModels.Models.Shared;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager;

/// <summary>
/// Klasa zarządzająca plantacjami.
/// </summary>
public class PlantationManager : IPlantationManager
{
    /// <summary>
    /// Repozytorium użytkowników.
    /// </summary>
    private readonly IRepository<User, long> _userRepository;
    
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
    /// Repozytorium wody.
    /// </summary>
    private readonly IRepository<Water> _waterRepository;
    
    /// <summary>
    /// Repozytorium roślin.
    /// </summary>
    private readonly IRepository<Plant> _plantRepository;
    
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium zadań.
    /// </summary>
    private readonly IRepository<Quest> _questRepository;
    
    /// <summary>
    /// Repozytorium bonusów.
    /// </summary>
    private readonly IRepository<Bonus> _bonusRepository;
    
    /// <summary>
    /// Repozytorium magazynów gracza.
    /// </summary>
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Repozytorium typów generowanych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium relacji zadań do nagród.
    /// </summary>
    private readonly IRepository<DropQuest> _dropQuestRepository;
    
    /// <summary>
    /// Repozytorium wymagań.
    /// </summary>
    private readonly IRepository<Requirement> _requirementRepository;
    
    /// <summary>
    /// Repozytorium magazynu plantacji.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Repozytorium nagród.
    /// </summary>
    private readonly IRepository<Drop> _dropRepository;
    
    /// <summary>
    /// Repozytorium donów dzielnic.
    /// </summary>
    private readonly IRepository<DistrictDon> _districtDonRepository;
    
    /// <summary>
    /// Serwis podstawowy obsługujący logikę biznesową związaną z rośliną.
    /// </summary>
    private readonly IPlantService _plantService;
    
    /// <summary>
    /// Serwis podstawowy obsługujący logikę biznesową związaną z magazynem gracza.
    /// </summary>
    private readonly IPlayerStorageService _playerStorageService;
    
    /// <summary>
    /// Serwis podstawowy obsługujący logikę biznesową związaną z magazynem plantacji.
    /// </summary>
    private readonly IPlantationStorageService _plantationStorageService;
    
    /// <summary>
    /// Generator nowych graczy.
    /// </summary>
    private readonly INewPlayerGenerator _newPlayerGenerator;
    
    /// <summary>
    /// Serwis podstawowy obsługujący logikę biznesową związaną z zadaniami.
    /// </summary>
    private readonly IQuestService _questService;
    
    /// <summary>
    /// Serwis podstawowy obsługujący logikę ignorowania zmian dla encji.
    /// </summary>
    private readonly IIgnoreChangeService _ignoreChangeService;
    
    /// <summary>
    /// Repozytorium transakcji czarnego rynku.
    /// </summary>
    private readonly IRepository<BlackMarketTransaction> _blackMarketTransactionRepository;
    
    /// <summary>
    /// Repozytorium postępu wymagań zadania.
    /// </summary>
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    
    /// <summary>
    /// Kontekst huba dla zadań.
    /// </summary>
    private readonly IHubContext _questHub;
    
    /// <summary>
    /// Generator liczb losowych.
    /// </summary>
    private readonly Random _random = new();
        
    
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
    /// <param name="userRepository">Repozytorium użytkowników.</param>
    /// <param name="driedFruitRepository">Repozytorium suszu.</param>
    /// <param name="lampRepository">Repozytorium lamp.</param>
    /// <param name="manureRepository">Repozytorium nawozów.</param>
    /// <param name="potRepository">Repozytorium donic.</param>
    /// <param name="seedRepository">Repozytorium nasion.</param>
    /// <param name="soilRepository">Repozytorium gleb.</param>
    /// <param name="waterRepository">Repozytorium wody.</param>
    /// <param name="plantRepository">Repozytorium roślin.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="questRepository">Repozytorium zadań.</param>
    /// <param name="bonusRepository">Repozytorium bonusów.</param>
    /// <param name="playerStorageRepository">Repozytorium magazynów gracza.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="dropQuestRepository">Repozytorium relacji zadań do nagród.</param>
    /// <param name="requirementRepository">Repozytorium wymagań.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynu plantacji.</param>
    /// <param name="dropRepository">Repozytorium nagród.</param>
    /// <param name="districtDonRepository">Repozytorium donów dzielnic.</param>
    /// <param name="plantService">Serwis podstawowy obsługujący logikę biznesową związaną z rośliną.</param>
    /// <param name="playerStorageService">Serwis podstawowy obsługujący logikę biznesową związaną z magazynem gracza.</param>
    /// <param name="plantationStorageService">Serwis podstawowy obsługujący logikę biznesową związaną z magazynem plantacji.</param>
    /// <param name="newPlayerGenerator">Generator nowych graczy.</param>
    /// <param name="questService">Serwis podstawowy obsługujący logikę biznesową związaną z zadaniami.</param>
    /// <param name="ignoreChangeService">Serwis podstawowy obsługujący logikę ignorowania zmian dla encji.</param>
    /// <param name="blackMarketTransactionRepository">Repozytorium transakcji czarnego rynku.</param>
    /// <param name="questRequirementsProgressRepository">Repozytorium postępu wymagań zadania.</param>
    public PlantationManager(
        IRepository<User, long> userRepository,
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<Lamp> lampRepository,
        IRepository<Manure> manureRepository,
        IRepository<Pot> potRepository,
        IRepository<Seed> seedRepository,
        IRepository<Soil> soilRepository,
        IRepository<Water> waterRepository,
        IRepository<Plant> plantRepository,
        IRepository<District> districtRepository,
        IRepository<Quest> questRepository,
        IRepository<Bonus> bonusRepository,
        IRepository<PlayerStorage> playerStorageRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<DropQuest> dropQuestRepository,
        IRepository<Requirement> requirementRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<Drop> dropRepository,
        IRepository<DistrictDon> districtDonRepository,
        IPlantService plantService,
        IPlayerStorageService playerStorageService,
        IPlantationStorageService plantationStorageService,
        INewPlayerGenerator newPlayerGenerator, 
        IQuestService questService,
        IIgnoreChangeService ignoreChangeService,
        IRepository<BlackMarketTransaction> blackMarketTransactionRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository
    )
    {
        Logger = NullLogger.Instance;
        AbpSession = NullAbpSession.Instance;
        _userRepository = userRepository;
        _driedFruitRepository = driedFruitRepository;
        _lampRepository = lampRepository;
        _manureRepository = manureRepository;
        _potRepository = potRepository;
        _seedRepository = seedRepository;
        _soilRepository = soilRepository;
        _waterRepository = waterRepository;
        _plantRepository = plantRepository;
        _districtRepository = districtRepository;
        _questRepository = questRepository;
        _bonusRepository = bonusRepository;
        _playerStorageRepository = playerStorageRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _dropQuestRepository = dropQuestRepository;
        _requirementRepository = requirementRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _dropRepository = dropRepository;
        _districtDonRepository = districtDonRepository;
        _plantService = plantService;
        _playerStorageService = playerStorageService;
        _plantationStorageService = plantationStorageService;
        _newPlayerGenerator = newPlayerGenerator;
        _questService = questService;
        _ignoreChangeService = ignoreChangeService;
        _blackMarketTransactionRepository = blackMarketTransactionRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _questHub = GlobalHost.ConnectionManager.GetHubContext<QuestHub>();
    }
        
    /// <summary>
    /// Pobiera informacje o plantacji dla określonego użytkownika.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="districtId">Identyfikator dzielnicy (opcjonalnie).</param>
    /// <param name="heWantPayForHollow">Czy gracz chce zapłacić za dziuple.</param>
    /// <returns>Informacje o plantacji.</returns>
    public async Task<Plantation> GetPlantation(long userId, int? districtId, bool heWantPayForHollow)
    {
        var user = await _userRepository.GetAsync(userId);
        var playerStorage = await _playerStorageService.GetPlayerStorage(userId);

        if (districtId != null)
            playerStorage.LastSelectedDistrictId = districtId;

        var ret = await _newPlayerGenerator.GetOrInitPlayerResources(heWantPayForHollow, playerStorage, user);
        if (ret.PlantationStorage == null) return ret;

        var filteredQuestsAndQuestCreation = await FilterQuests(ret);
        ret.FilteredQuests = filteredQuestsAndQuestCreation.Quests;
        ret.QuestInfoCreation = filteredQuestsAndQuestCreation.QuestInfoCreation;
        ret.Plants = await _plantRepository.GetAllListAsync(currPlant => currPlant.PlantationStorageId == ret.PlantationStorage.Id);
        ret.DonData = await GetDonData(ret);
        ret.WardensNames = await GetWardensNames(ret);
        
        await SetUserProducts(ret);
        PlantationManagerHelper.SetCurrencyExchanges(ret);

        return ret;
    }

    /// <summary>
    /// Pobiera nazwy opiekunów dzielnic.
    /// </summary>
    /// <param name="plantation">Informacje o plantacji.</param>
    /// <returns>Słownik zawierający identyfikatory dzielnic z nazwami opiekunów.</returns>
    private async Task<Dictionary<int, string>> GetWardensNames(Plantation plantation)
    {
        var ret = new Dictionary<int, string>();

        foreach (var district in plantation.AvailableDistricts)
        {
            ret[district.Id] = "";
            try
            {
                ret[district.Id] = (await _userRepository.GetAsync(district.UserId)).Name;
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity"))
                {
                    Logger.Warn("Opiekun dzielnicy nie istnieje. Dzielnica - " + district.Name, ex);
                    ret[district.Id] = "Nie istnieje";
                }
                else
                    Logger.Error("Blad =======//========", ex);
            }   
        }

        return ret;
    }

    /// <summary>
    /// Pobiera dane dotyczące dona dzielnicy.
    /// </summary>
    /// <param name="plantation">Informacje o plantacji.</param>
    /// <returns>Dane dotyczące dona dzielnicy.</returns>
    private async Task<DonData> GetDonData(Plantation plantation)
    {
        var districtDon = await _districtDonRepository.FirstOrDefaultAsync(item => item.DistrictId == plantation.District.Id);
        if (districtDon == null) 
            return new DonData();
        
        var donPlantationStorage = await _plantationStorageRepository.SingleAsync(item => item.Id == districtDon.PlantationStorageId);
        var district = await _districtRepository.GetAsync(plantation.PlantationStorage.DistrictId);
        var donPlayerStorage = await _playerStorageRepository.SingleAsync(item => item.UserId == donPlantationStorage.UserId);
            
        return new DonData
        {
            WeHaveDon = true,
            DonName = donPlayerStorage.PlayerName,
            DonCharityPercentage = district.DonCharityPercentage, 
            DonId = donPlantationStorage.UserId,
            DistrictId = donPlantationStorage.DistrictId
        };
    }

    /// <summary>
    /// Ustawia produkty użytkownika dla danej plantacji.
    /// </summary>
    /// <param name="plantation">Informacje o plantacji.</param>
    private async Task SetUserProducts(Plantation plantation)
    {
        var plantationStorageId = plantation.PlantationStorage.Id;
        
        plantation.UserDriedFruits = await _driedFruitRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorageId);
        plantation.UserLamps = await _lampRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorageId);
        plantation.UserManures = await _manureRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorageId);
        plantation.UserSoils = await _soilRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorageId);
        plantation.UserWaters = await _waterRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorageId);
        plantation.UserSeeds = await _seedRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorageId);
        plantation.UserPots = await _potRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorageId);
        plantation.UserBonuses = await _bonusRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorageId);
    }
    
    /// <summary>
    /// Tworzy obiekt czarnego rynku dla danego gracza.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="objectMapper">Mapper obiektów.</param>
    /// <returns>Obiekt reprezentujący czarny rynek.</returns>
    public async Task<BlackMarket> CreatePlayerBlackMarket(long userId, IObjectMapper objectMapper)
    {
        var plantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(userId);
        var districtTransactions = await _blackMarketTransactionRepository.GetAll().Join(
            _generatedTypeRepository.GetAll().Where(item => item.DistrictId == plantationStorage.DistrictId),
            transaction => transaction.GeneratedTypeId,
            generatedType => generatedType.Id,
            (transaction, generatedType) => transaction).ToListAsync();

        var notPlayerTransactions = districtTransactions.Where(item => item.SellerId != plantationStorage.UserId).ToList();
        var issuedItems = districtTransactions.Where(item => item.SellerId == plantationStorage.UserId).ToList();
            
        var playerDriedFruits = await _driedFruitRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id && item.PlayerCanSellInBlackMarket && item.BlackMarketMinSellPrice != null && item.BlackMarketMaxSellPrice != null);
        var playerLamps = await _lampRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id && item.PlayerCanSellInBlackMarket && item.BlackMarketMinSellPrice != null && item.BlackMarketMaxSellPrice != null);
        var playerManures = await _manureRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id && item.PlayerCanSellInBlackMarket && item.BlackMarketMinSellPrice != null && item.BlackMarketMaxSellPrice != null);
        var playerPots = await _potRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id && item.PlayerCanSellInBlackMarket && item.BlackMarketMinSellPrice != null && item.BlackMarketMaxSellPrice != null);
        var playerSeeds = await _seedRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id && item.PlayerCanSellInBlackMarket && item.BlackMarketMinSellPrice != null && item.BlackMarketMaxSellPrice != null);
        var playerSoils = await _soilRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id && item.PlayerCanSellInBlackMarket && item.BlackMarketMinSellPrice != null && item.BlackMarketMaxSellPrice != null);
        var playerWaters = await _waterRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id && item.PlayerCanSellInBlackMarket && item.BlackMarketMinSellPrice != null && item.BlackMarketMaxSellPrice != null);
        var playerBonuses = await _bonusRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id && item.PlayerCanSellInBlackMarket && item.BlackMarketMinSellPrice != null && item.BlackMarketMaxSellPrice != null);

        var forSale = new List<BlackMarketSellItem>();
        forSale.AddRange(PlantationManagerHelper.GenerateBlackMarketSellItems(playerDriedFruits));
        forSale.AddRange(PlantationManagerHelper.GenerateBlackMarketSellItems(playerLamps));
        forSale.AddRange(PlantationManagerHelper.GenerateBlackMarketSellItems(playerManures));
        forSale.AddRange(PlantationManagerHelper.GenerateBlackMarketSellItems(playerPots));
        forSale.AddRange(PlantationManagerHelper.GenerateBlackMarketSellItems(playerSeeds));
        forSale.AddRange(PlantationManagerHelper.GenerateBlackMarketSellItems(playerSoils));
        forSale.AddRange(PlantationManagerHelper.GenerateBlackMarketSellItems(playerWaters));
        forSale.AddRange(PlantationManagerHelper.GenerateBlackMarketSellItems(playerBonuses));

        var displayParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Display);
        var parsedBuyRecords = displayParser.Parse(objectMapper.Map<List<BlackMarketItem>>(notPlayerTransactions).Cast<object>().ToList());
        var parsedIssuedRecords = displayParser.Parse(objectMapper.Map<List<BlackMarketItem>>(issuedItems).Cast<object>().ToList());

        return new BlackMarket
        {
            BuyDbProperties = PlantationManagerHelper.GetPropList(typeof(BlackMarketItem)),
            BuyHrProperties = PlantationManagerHelper.GetHrPropList(typeof(BlackMarketItem)),
            SellDbProperties = PlantationManagerHelper.GetPropList(typeof(BlackMarketSellItem)),
            SellHrProperties = PlantationManagerHelper.GetHrPropList(typeof(BlackMarketSellItem)),
            IssuedDbProperties = PlantationManagerHelper.GetPropList(typeof(BlackMarketItem)),
            IssuedHrProperties = PlantationManagerHelper.GetHrPropList(typeof(BlackMarketItem)),
            BuyRecords = parsedBuyRecords,
            SellRecords = forSale,
            IssuedRecords = parsedIssuedRecords
        };
    }
        
    /// <summary>
    /// Pobiera dostępne produkty gracza dla danej encji.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="valueToSearch">Wartość do wyszukania (opcjonalnie).</param>
    /// <returns>Lista dostępnych produktów gracza dla danej encji.</returns>
    public async Task<List<object>> GetAvailablePlayerProducts(long userId, string entity, string valueToSearch)
    {
        List<object> userProducts;
        var plantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(userId);
        
        switch (entity)
        {
            case EntitiesDbNames.Lamp:
                userProducts = (await _lampRepository.GetAllListAsync(currItem =>
                    currItem.PlantationStorageId == plantationStorage.Id
                    && currItem.OwnedAmount > 0
                    && currItem.PlantationLevelRequirement <= plantationStorage.Level)).Where(
                    currItem =>
                        string.IsNullOrWhiteSpace(valueToSearch) ||
                        currItem.Name.ToLower().Contains(valueToSearch.ToLower())).Cast<object>().ToList();
                break;
            case EntitiesDbNames.Manure:
                userProducts = (await _manureRepository.GetAllListAsync(currItem =>
                    currItem.PlantationStorageId == plantationStorage.Id
                    && currItem.OwnedAmount > 0
                    && currItem.PlantationLevelRequirement <= plantationStorage.Level)).Where(
                    currItem =>
                        string.IsNullOrWhiteSpace(valueToSearch) ||
                        currItem.Name.ToLower().Contains(valueToSearch.ToLower())).Cast<object>().ToList();
                break;
            case EntitiesDbNames.Pot:
                userProducts = (await _potRepository.GetAllListAsync(currItem =>
                    currItem.PlantationStorageId == plantationStorage.Id
                    && currItem.OwnedAmount > 0
                    && currItem.PlantationLevelRequirement <= plantationStorage.Level)).Where(
                    currItem =>
                        string.IsNullOrWhiteSpace(valueToSearch) ||
                        currItem.Name.ToLower().Contains(valueToSearch.ToLower())).Cast<object>().ToList();
                break;
            case EntitiesDbNames.Seed:
                userProducts = (await _seedRepository.GetAllListAsync(currItem =>
                    currItem.PlantationStorageId == plantationStorage.Id
                    && currItem.OwnedAmount > 0
                    && currItem.PlantationLevelRequirement <= plantationStorage.Level)).Where(
                    currItem =>
                        string.IsNullOrWhiteSpace(valueToSearch) ||
                        currItem.Name.ToLower().Contains(valueToSearch.ToLower())).Cast<object>().ToList();
                break;
            case EntitiesDbNames.Soil:
                userProducts = (await _soilRepository.GetAllListAsync(currItem =>
                    currItem.PlantationStorageId == plantationStorage.Id
                    && currItem.OwnedAmount > 0
                    && currItem.PlantationLevelRequirement <= plantationStorage.Level)).Where(
                    currItem =>
                        string.IsNullOrWhiteSpace(valueToSearch) ||
                        currItem.Name.ToLower().Contains(valueToSearch.ToLower())).Cast<object>().ToList();
                break;
            case EntitiesDbNames.Water:
                userProducts = (await _waterRepository.GetAllListAsync(currItem =>
                    currItem.PlantationStorageId == plantationStorage.Id
                    && currItem.OwnedAmount > 0
                    && currItem.PlantationLevelRequirement <= plantationStorage.Level)).Where(
                    currItem =>
                        string.IsNullOrWhiteSpace(valueToSearch) ||
                        currItem.Name.ToLower().Contains(valueToSearch.ToLower())).Cast<object>().ToList();
                break;
            default:
                throw new ArgumentOutOfRangeException(entity);
        }
            
        return userProducts;
    }
        
    /// <summary>
    /// Tworzy roślinę dla danego gracza.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <param name="plantData">Dane dotyczące tworzonej rośliny.</param>
    /// <returns>Obiekt reprezentujący wynik tworzenia rośliny.</returns>
    public async Task<CreatePlant> CreatePlayerPlant(long userId, string userName, PlantData plantData)
    {
        var ret = new CreatePlant();
        
        if (plantData == null) 
            throw new AbpException("Brak danych tworzenia rośliny");

        ret.Lamp = await _lampRepository.GetAsync(plantData.Lamp);
        ret.Manure = await _manureRepository.GetAsync(plantData.Manure);
        ret.Soil = await _soilRepository.GetAsync(plantData.Soil);
        ret.Water = await _waterRepository.GetAsync(plantData.Water);
        ret.Seed = await _seedRepository.GetAsync(plantData.Seed);
        ret.Pot = await _potRepository.GetAsync(plantData.Pot);

        if (!PlantationManagerHelper.CheckRequirements(ret, out var message))
            ret.RequirementsIsNotOkMessage = message;
        else
        {
            ret.RequirementsIsOk = true;
            
            await PlantServiceHelper.SetCreationCosts(ret, _ignoreChangeService);
            var playerPlantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(userId);
            var activeBonuses = await _bonusRepository.GetAllListAsync(item => item.PlantationStorageId == playerPlantationStorage.Id && item.IsActive);
            var district = await _districtRepository.GetAsync(playerPlantationStorage.DistrictId);
            var generatedType = await _generatedTypeRepository.GetAsync(ret.Seed.GeneratedTypeId);
            var plantId = await _plantService.CreatePlayerPlant(ret, district, userName, playerPlantationStorage.Id, activeBonuses, generatedType);
            
            ret.Plant = await _plantRepository.GetAsync(plantId);
        }

        return ret;
    }

    /// <summary>
    /// Usuwa roślinę gracza na podstawie identyfikatora.
    /// </summary>
    /// <param name="id">Identyfikator rośliny do usunięcia.</param>
    /// <returns>Lista powiadomień akcji usuwania rośliny.</returns>
    public async Task<List<string>> RemovePlayerPlant(int id)
    {
        var dropsNotification = new List<string>();
   
        var plant = await _plantRepository.GetAsync(id);
        var pot = await _potRepository.GetAsync(plant.PotId);
        var lamp = await _lampRepository.GetAsync(plant.LampId);
        var soil = await _soilRepository.GetAsync(plant.SoilId);

        pot.InUseCount--;
        pot.OwnedAmount++;
        await _ignoreChangeService.Add(pot);

        dropsNotification.Add("Zwrócono doniczkę: " + pot.Name);

        lamp.OwnedAmount++;
        lamp.InUseCount--;
        await _ignoreChangeService.Add(lamp);
                
        dropsNotification.Add("Zwrócono lampę: " + lamp.Name);
                
        var soilPenaltyCount = PlantServiceHelper.CountSoilPenalty(pot);
        var soilReturnCount = pot.Capacity - soilPenaltyCount;
                
        soil.OwnedAmount += soilReturnCount;
        await _ignoreChangeService.Add(soil);
                
        dropsNotification.Add("Zwrócono: " + soilReturnCount + PlantationManagerHelper.GetMeasureUnitByType(typeof(Soil)) + " gleby");

        await _plantRepository.DeleteAsync(id);

        return dropsNotification;
    }

    /// <summary>
    /// Reprezentuje zebranie rośliny gracza na podstawie identyfikatora.
    /// </summary>
    /// <param name="id">Identyfikator rośliny do zebrania.</param>
    /// <returns>Lista powiadomień akcji zbierania rośliny.</returns>
    public async Task<List<string>> CollectPlayerPlant(int id)
    {
        var userId = AbpSession.GetUserId();

        var plant = await _plantRepository.GetAsync(id);
        var pot = await _potRepository.GetAsync(plant.PotId);
        var soil = await _soilRepository.GetAsync(plant.SoilId);
        var lamp = await _lampRepository.GetAsync(plant.LampId);

        var playerStorage = await _playerStorageService.GetPlayerStorage(userId);
        var playerPlantationStorage = await _plantationStorageRepository.SingleAsync(item =>
            item.DistrictId == playerStorage.LastSelectedDistrictId && item.UserId == playerStorage.UserId);
        var district = await _districtRepository.GetAsync(playerPlantationStorage.DistrictId);
        var dropsNotification = new List<string>();

        PlantServiceHelper.CalculateAndSetWiltPenalty(plant);

        var plantationLevel = playerPlantationStorage.Level;
        PlantationManagerHelper.AddLevels(district, playerPlantationStorage, plant.GainedExp, dropsNotification, playerStorage);
        var receivedLevels = playerPlantationStorage.Level - plantationLevel;
        dropsNotification.Add("Otrzymano " + receivedLevels + " poziomów");

        var playerDriedFruit = await _driedFruitRepository.SingleAsync(currDriedFruit =>
            currDriedFruit.PlantationStorageId == playerPlantationStorage.Id &&
            currDriedFruit.GeneratedTypeId == plant.GeneratedTypeId);

        var playerSeed = await _seedRepository.SingleAsync(currSeed =>
            currSeed.PlantationStorageId == playerPlantationStorage.Id &&
            currSeed.GeneratedTypeId == plant.GeneratedTypeId);

        await PlantationManagerHelper.ProcessCollectPlantDrops(playerDriedFruit, plant, dropsNotification, playerSeed, district, playerPlantationStorage, _random, _ignoreChangeService);

        var lostSoilCount = PlantServiceHelper.CountSoilPenalty(pot);
        var returnedSoil = pot.Capacity - lostSoilCount;

        pot.InUseCount--;
        pot.OwnedAmount++;
        await _ignoreChangeService.Add(pot);
            
        lamp.OwnedAmount++;
        lamp.InUseCount--;
        await _ignoreChangeService.Add(lamp);
            
        soil.OwnedAmount += returnedSoil;
        await _ignoreChangeService.Add(soil);

        dropsNotification.Add("Zwrócono doniczkę: " + pot.Name);
        dropsNotification.Add("Zwrócono lampę: " + lamp.Name);
        dropsNotification.Add("Zwrócono " + returnedSoil + PlantationManagerHelper.GetMeasureUnitByType(typeof(Soil)) + " gleby");

        await _plantRepository.DeleteAsync(id);

        return dropsNotification;
    }

    /// <summary>
    /// Przetwarza ukończone zadanie użytkownika.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="questId">Identyfikator ukończonego zadania.</param>
    /// <param name="objectMapper">Mapper obiektów.</param>
    /// <returns>Model z danymi ukończonego zadania.</returns>
    public async Task<CompleteQuest> ProcessCompletedQuest(long userId, int questId, IObjectMapper objectMapper)
    {
        var completeQuest = new CompleteQuest();
        
        var quest = await _questRepository.GetAsync(questId);
        completeQuest.Quest = quest;
        completeQuest.QuestInfoCreation = await CreateQuestInfoCreationModel(quest);
        
        if (quest.IsComplete) 
            return completeQuest; // Jeśli zadanie jest ukończone bo np. użytkownik miał otwarte dwie zakładki i w pierwszej ukończył zadanie a teraz chce ukończyć w drugiej to jak kliknie po raz drugi ukończ, to zadanie po prostu przeniesie mu się do ukończonych
        
        if (!PlantationManagerHelper.AllRequirementsIsDone(completeQuest))
            return completeQuest; // init status false

        completeQuest.QuestRequirementsProgress = completeQuest.QuestInfoCreation.QuestsRequirementsProgress.First();
        completeQuest.ObjectMapper = objectMapper;
        completeQuest.PlayerStorage = await _playerStorageRepository.SingleAsync(item => item.UserId == userId);
        completeQuest.UserId = completeQuest.PlayerStorage.UserId;
        completeQuest.PlantationStorage = await _plantationStorageRepository.GetAsync((int) quest.PlantationStorageId); // na tym etapie musi mieć PlantationStorageId
        completeQuest.District = await _districtRepository.GetAsync(completeQuest.PlantationStorage.DistrictId);
        completeQuest.QuestDefinition = completeQuest.QuestInfoCreation.QuestDefinitions.First();

        completeQuest.GeneratedTypes = await _generatedTypeRepository.GetAllListAsync(item =>
                item.DistrictId == completeQuest.PlantationStorage.DistrictId);
        completeQuest.Connections = completeQuest.QuestInfoCreation.DropQuestRelations
            .Where(item => item.QuestId == completeQuest.QuestDefinition.Id).ToList();
        completeQuest.DbParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Db);
        completeQuest.ParserDbStrategy = new DbStrategy();
        
        foreach (var connection in completeQuest.Connections)
            completeQuest.ConnectedDrops.Add(completeQuest.QuestInfoCreation.Drops.Single(item => item.Id == connection.DropId));
            
        await ProcessConditionDeliverRequirement(completeQuest);
        await ProcessReward(completeQuest);
        await ProcessQuest(completeQuest);
        completeQuest.InitStatus = true;
            
        _questHub.Clients.User(userId.ToString()).completedQuest(completeQuest.DropsNotification, userId);

        return completeQuest;
    }
        
    /// <summary>
    /// Przetwarza wymagania z warunkiem typu dostarcz dla ukończonego zadania.
    /// </summary>
    /// <param name="completeQuest">Model z danymi ukończonego zadania.</param>
    private async Task ProcessConditionDeliverRequirement(CompleteQuest completeQuest)
    {
        var plantationStorage = completeQuest.PlantationStorage;
        var playerStorage = completeQuest.PlayerStorage;
        var parserDbStrategy = completeQuest.ParserDbStrategy;
        var dbParser = completeQuest.DbParser;
            
        var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(completeQuest.QuestRequirementsProgress.RequirementsProgress);
        foreach (var reqToProgress in requirementsProgress)
        {
            var req = completeQuest.QuestInfoCreation.Requirements.Single(item => item.Id == reqToProgress.Key);
            if (req.Condition != DbRequirementConditionsNames.Deliver) continue;
                
            if (req.GeneratedTypeId == null)
            {
                if (req.CustomEntityName == DbCustomEntityNames.PlantationStorage)
                {
                    var hrReqFieldName = PlantationManagerHelper.GetHrPropName(completeQuest.ObjectMapper
                        .Map<PlantationStorageDto>(plantationStorage).GetType().GetProperties()
                        .Single(item => item.Name == req.Comparer));
                    var reqProp = plantationStorage.GetType().GetProperty(req.Comparer);
                    var fieldValue = reqProp?.GetValue(plantationStorage);
                    var parsedValue = (decimal) parserDbStrategy.ParseValue(EnumUtils.PropTypes.Decimal, fieldValue);
                    var tempNewValue = parsedValue - req.Amount;
                    var reqPropType = ParserHelper.GetPropType(reqProp);
                    var newValue =  reqPropType == EnumUtils.PropTypes.Int ? (int) tempNewValue : (decimal) tempNewValue;
                        
                    reqProp?.SetValue(plantationStorage, dbParser.Parse(reqProp, newValue));
                    completeQuest.DropsNotification.Add("Pobrano " + req.Amount +
                                                        PlantationManagerHelper.GetMeasureUnitForCurrencyExchange(req.Comparer) 
                                                        + " " + hrReqFieldName);
                }
                else if (req.CustomEntityName == DbCustomEntityNames.PlayerStorage)
                {
                    var hrReqFieldName = PlantationManagerHelper.GetHrPropName(completeQuest.ObjectMapper
                        .Map<PlantationStorageDto>(plantationStorage).GetType().GetProperties()
                        .Single(item => item.Name == req.Comparer));
                    var reqProp = playerStorage.GetType().GetProperty(req.Comparer);
                    var fieldValue = reqProp?.GetValue(playerStorage);
                    var parsedValue = (decimal) parserDbStrategy.ParseValue(EnumUtils.PropTypes.Decimal, fieldValue);
                    var tempNewValue = parsedValue - req.Amount;
                    var reqPropType = ParserHelper.GetPropType(reqProp);
                    var newValue =  reqPropType == EnumUtils.PropTypes.Int ? (int) tempNewValue : (decimal) tempNewValue;
                    
                    reqProp?.SetValue(playerStorage, dbParser.Parse(reqProp, newValue));
                    completeQuest.DropsNotification.Add("Pobrano " + req.Amount +
                                                        PlantationManagerHelper.GetMeasureUnitForCurrencyExchange(req.Comparer)
                                                        + " " + hrReqFieldName);
                }    
            }
            else
            {
                var generatedType = completeQuest.GeneratedTypes.Single(item => item.Id == (int) req.GeneratedTypeId);
                var entityName = generatedType.EntityName;
                
                if (req.CustomEntityName == DbCustomEntityNames.DriedFruit || req.CustomEntityName == DbCustomEntityNames.Seed)
                    entityName = req.CustomEntityName;

                var repo = CustomRepositoryFactory.GetRepository(entityName);
                var userItem =
                    (await repo.GetWhereAsync(RelationFieldsNames.PlantationStorageId,
                        plantationStorage.Id)).Cast<Product>()
                    .SingleOrDefault(item => item.GeneratedTypeId == req.GeneratedTypeId); 

                var reqProp = userItem?.GetType().GetProperty(req.Comparer);
                var fieldValue = reqProp?.GetValue(userItem);
                var parsedValue = (decimal) parserDbStrategy.ParseValue(EnumUtils.PropTypes.Decimal, fieldValue);
                var tempNewValue = parsedValue - req.Amount;
                var reqPropType = ParserHelper.GetPropType(reqProp);
                var newValue =  reqPropType == EnumUtils.PropTypes.Int ? (int) tempNewValue : (decimal) tempNewValue;
                reqProp?.SetValue(userItem, dbParser.Parse(reqProp, newValue));

                await _ignoreChangeService.Add(userItem);
                completeQuest.DropsNotification.Add("Pobrano " + req.Amount +
                                                    PlantationManagerHelper.GetMeasureUnitByEntityName(entityName) 
                                                    + " " + userItem?.Name);
            }
        }
    }

    /// <summary>
    /// Przetwarza nagrody dla ukończonego zadania.
    /// </summary>
    /// <param name="completeQuest">Model z danymi ukończonego zadania.</param>
    private async Task ProcessReward(CompleteQuest completeQuest)
    {
        var increaseDropChanceFromQuests = (await _bonusRepository.GetAllListAsync(item => item.IncreaseDropChanceFromQuests > 0 &&
            item.PlantationStorageId == completeQuest.PlantationStorage.Id && item.IsActive)).Sum(item => item.IncreaseDropChanceFromQuests);

        foreach (var drop in completeQuest.ConnectedDrops)
        {
            var chanceForDrop = (decimal) 100;
            if (drop.ChanceForDrop != null && drop.ChanceForDrop != 0 && drop.ChanceForDrop < 100)
                chanceForDrop = (decimal) drop.ChanceForDrop + (increaseDropChanceFromQuests ?? 0);

            var randomInt = _random.Next(1, 100);   
            if (randomInt <= chanceForDrop)
            {
                if (drop.GeneratedTypeId != null)
                {
                    var generatedType = completeQuest.GeneratedTypes.Single(item => item.Id == (int) drop.GeneratedTypeId);
                    if (generatedType.EntityName != EntitiesDbNames.Quest) // drop dla typu suszu, rosliny i nasiona to zawsze typ nasiona. Czyli roślina ani susz nie mogą być dropem
                    {
                        if (drop.ItemAmount == null) continue;

                        var repo = CustomRepositoryFactory.GetRepository(generatedType.EntityName);
                        var connectedRecord = (await repo.GetWhereAsync(RelationFieldsNames.PlantationStorageId,
                                completeQuest.PlantationStorage.Id)).Cast<Product>()
                            .Single(item => item.GeneratedTypeId == generatedType.Id);

                        connectedRecord.OwnedAmount += (decimal) drop.ItemAmount;
                        await _ignoreChangeService.Add(connectedRecord);
                        
                        completeQuest.DropsNotification.Add("Otrzymano " + drop.ItemAmount +
                            PlantationManagerHelper.GetMeasureUnitByEntityName(generatedType.EntityName) + " " + connectedRecord?.Name);
                    }
                    else
                    {
                        var connectedQuest = await _questRepository.SingleAsync(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == completeQuest.PlantationStorage.Id);
                        if (connectedQuest.InProgress)
                        {
                            completeQuest.DropsNotification.Add(
                                "Otrzymane zadanie jest już rozpoczęte. Zadanie - " +
                                SelectListLoaderHelper.QuestTypesNamesDbToHr[
                                    connectedQuest.QuestType] + " - " +
                                connectedQuest.Name);
                        }
                        else
                        {
                            var questProgress = await _questRequirementsProgressRepository.SingleAsync(item => item.QuestId == connectedQuest.Id);
                            WorkersHelper.ResetQuest(connectedQuest, questProgress);
                            await _questService.SetStartValuesAsync(new List<Quest> {connectedQuest});
                            
                            var notAvailable = "";
                            completeQuest.DropsNotification.Add("Otrzymano zadanie - " + SelectListLoaderHelper.QuestTypesNamesDbToHr[connectedQuest.QuestType] + " - " + connectedQuest.Name);
                            
                            if (!PlantationManagerHelper.CheckQuestLimitsAndSetProgressStatus(connectedQuest,
                                    completeQuest.PlantationStorage, _questHub))
                                _questHub.Clients.User(completeQuest.UserId.ToString()).droppedQuest(connectedQuest.Id, connectedQuest.QuestType);
                            else
                            {
                                notAvailable = "Limit wykorzystany. Zadanie przeniesione do dostępnych";
                                _questHub.Clients.User(completeQuest.UserId.ToString()).setRestartedQuest(connectedQuest.Id, connectedQuest.QuestType);
                            }

                            completeQuest.DropsNotification.Add(notAvailable);
                        }
                    }
                }
                else
                    PlantationManagerHelper.AddGainedCurrency(drop, completeQuest);
            }
            else
            {
                completeQuest.DropsNotification.Add(
                    "Niestety nie udało się zdobyć nagrody - " + drop.Name + ". Aktualny procent szansy " + chanceForDrop +
                    " wylosowano " + randomInt);
            }
        }
    }

    /// <summary>
    /// Przetwarza ukończone zadanie.
    /// </summary>
    /// <param name="completeQuest">Model z danymi ukończonego zadania.</param>
    private async Task ProcessQuest(CompleteQuest completeQuest)
    {
        var quest = completeQuest.Quest;
        quest.CompletedAmount += 1;
        
        if (quest.IsRepetitive)
        {
            WorkersHelper.ResetQuest(quest, completeQuest.QuestRequirementsProgress);
            await _questService.SetStartValuesAsync(new List<Quest> {quest});
        }
        else
        {
            quest.IsComplete = true;
            quest.InProgress = false;
        }

        TokensOperator.QuestTokenProfit(completeQuest);

        if (completeQuest.GainedExp > 0)
            PlantationManagerHelper.AddLevels(completeQuest.District, completeQuest.PlantationStorage, completeQuest.GainedExp,
                completeQuest.DropsNotification, completeQuest.PlayerStorage);

        PlantationManagerHelper.SetReceivedItemsMessages(completeQuest);
        
        completeQuest.PlantationStorage.Prestige += completeQuest.GainedPrestige;
        completeQuest.PlantationStorage.Gold += completeQuest.GainedGold;
        completeQuest.PlantationStorage.QuestToken += completeQuest.GainedQuestTokens;
        completeQuest.PlantationStorage.DealerToken += completeQuest.GainedDealerTokens;
        completeQuest.PlantationStorage.BlackMarketToken += completeQuest.GainedBlackMarketTokens;
        completeQuest.PlantationStorage.DonToken += completeQuest.GainedDonTokens;
        completeQuest.PlantationStorage.UnlockToken += completeQuest.GainedUnlockTokens;
        completeQuest.PlayerStorage.Honor += completeQuest.GainedHonor;
    }

    /// <summary>
    /// Tworzy model informacji o zadaniach.
    /// </summary>
    /// <param name="quest">Zadanie dla którego tworzony jest model informacji.</param>
    /// <returns>Model informacji o zadaniach.</returns>
    public async Task<QuestInfoCreation> CreateQuestInfoCreationModel(Quest quest)
    {
        var generatedType = await _generatedTypeRepository.GetAsync(quest.GeneratedTypeId);
        var questDef = await _questRepository.SingleAsync(item =>
            item.GeneratedTypeId == quest.GeneratedTypeId && item.PlantationStorageId == null);

        var filteredDropAndDropQuest = await _dropRepository.GetAll().Join(
            _dropQuestRepository.GetAll().Where(item => item.QuestId == questDef.Id),
            drop => drop.Id,
            dropQuest => dropQuest.DropId,
            (drop, dropQuest) => new {drop, dropQuest}).ToListAsync();
        
        return new QuestInfoCreation
        {
            QuestsRequirementsProgress = new List<QuestRequirementsProgress>{await _questRequirementsProgressRepository.SingleAsync(item => item.QuestId == quest.Id)},
            Requirements = await _requirementRepository.GetAllListAsync(item => item.DistrictId == generatedType.DistrictId),
            QuestDefinitions = new List<Quest>{questDef},
            DropQuestRelations = filteredDropAndDropQuest.Select(item => item.dropQuest).ToList(),
            Drops = filteredDropAndDropQuest.Select(item => item.drop).GroupBy(item => item.Id)
                .Select(item => item.First()).ToList(),
        };
    }

    /// <summary>
    /// Filtruje dostępne zadania dla danej plantacji.
    /// </summary>
    /// <param name="plantation">Plantacja, dla której filtrowane są zadania.</param>
    /// <returns>Obiekt zawierający dostępne zadania i ich dane z informacjami.</returns>
    private async Task<FilteredQuests> FilterQuests(Plantation plantation)
    {
        var ret = new FilteredQuests();
        
        var currDateTime = Clock.Now;
        var plantationStorageId = plantation.PlantationStorage.Id;

        var onePartQuestQuery = _questRepository.GetAll()
            .Where(item =>
                item.PlantationStorageId == plantationStorageId &&
                item.QuestType != DbQuestTypesNames.Achievement &&
                item.PlantationLevelRequirement <= plantation.PlantationStorage.Level &&
                item.IsAvailableInitially);
        
        var twoPartQuestsQuery = onePartQuestQuery.Union(onePartQuestQuery.Where(item =>
                item.QuestType == DbQuestTypesNames.Event &&
                item.EndTime > currDateTime || item.IsComplete));

        var questsQuery = twoPartQuestsQuery.Union(_questRepository.GetAll().Where(item =>
            item.QuestType == DbQuestTypesNames.Achievement &&
            item.PlantationStorageId == plantationStorageId));

        var questAndQuestProgress = await _questRequirementsProgressRepository.GetAll().Join(questsQuery,
            progress => progress.QuestId,
            quest => quest.Id,
            (progress, quest) => new {progress, quest}).ToListAsync();

        var districtQuestDefinitionsQuery = _questRepository.GetAll().Join(
            _generatedTypeRepository.GetAll().Where(item => item.DistrictId == plantation.District.Id),
            quest => quest.GeneratedTypeId,
            generatedType => generatedType.Id,
            (quest, generatedType) => quest).Where(item => item.PlantationStorageId == null);

        var definitionsQuestProgress = await _questRequirementsProgressRepository.GetAll().Join(districtQuestDefinitionsQuery,
            progress => progress.QuestId,
            quest => quest.Id,
            (progress, quest) => progress).ToListAsync();
        
        var dropQuestsQuery = _dropQuestRepository.GetAll().Join(districtQuestDefinitionsQuery,
            dropQuest => dropQuest.QuestId,
            quest => quest.Id,
            (dropQuest, quest) => dropQuest);

        var dropAndDropQuests = await _dropRepository.GetAll().Join(dropQuestsQuery,
            drop => drop.Id,
            dropQuest => dropQuest.DropId,
            (drop, dropQuest) => new {drop, dropQuest}).ToListAsync();

        var questsProgress = new List<QuestRequirementsProgress>();
        questsProgress.AddRange(definitionsQuestProgress);
        questsProgress.AddRange(questAndQuestProgress.Select(item => item.progress));

        var drop = dropAndDropQuests.Select(item => item.drop).First();
        ret.Quests = questAndQuestProgress.Select(item => item.quest).ToList();
        ret.QuestInfoCreation = new QuestInfoCreation
        {
            QuestsRequirementsProgress = questsProgress,
            Drops = dropAndDropQuests.Select(item => item.drop).GroupBy(item => item.Id)
                .Select(item => item.First()).ToList(),
            Requirements = await _requirementRepository.GetAllListAsync(item => item.DistrictId == drop.DistrictId),
            QuestDefinitions = await districtQuestDefinitionsQuery.ToListAsync(),
            DropQuestRelations = dropAndDropQuests.Select(item => item.dropQuest).ToList()
        };

        return ret;
    }
}