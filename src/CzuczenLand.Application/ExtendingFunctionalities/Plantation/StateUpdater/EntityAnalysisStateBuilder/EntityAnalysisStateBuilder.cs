using System;
using System.Linq;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.RealTime;
using Abp.Timing;
using Castle.Core.Logging;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;
using CzuczenLand.ExtendingFunctionalities.Services.Others.EntityChange.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.BlackMarket;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Bonus;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Info;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Quest;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.ExtendingModels.Models.Shared;
using Microsoft.AspNet.SignalR;
using ILogger = Castle.Core.Logging.ILogger;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.EntityAnalysisStateBuilder;

/// <summary>
/// Klasa budująca stan analizy encji.
/// </summary>
public class EntityAnalysisStateBuilder : IEntityAnalysisStateBuilder
{
    /// <summary>
    /// Repozytorium wymagań.
    /// </summary>
    private readonly IRepository<Requirement> _requirementRepository;
    
    /// <summary>
    /// Repozytorium magazynu plantacji.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Serwis obsługujący zmiany encji.
    /// </summary>
    private readonly IEntityChangeService _entityChangeService;
    
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
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium bonusów.
    /// </summary>
    private readonly IRepository<Bonus> _bonusRepository;
    
    /// <summary>
    /// Repozytorium postępu wymagań zadań.
    /// </summary>
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    
    /// <summary>
    /// Repozytorium użytkowników.
    /// </summary>
    private readonly IRepository<User, long> _userRepository;
    
    /// <summary>
    /// Serwis podstawowy obsługujący logikę biznesową związaną z zadaniami.
    /// </summary>
    private readonly IQuestService _questService;
    
    /// <summary>
    /// Serwis podstawowy obsługujący logikę ignorowania zmian dla encji.
    /// </summary>
    private readonly IIgnoreChangeService _ignoreChangeService;
    
    /// <summary>
    /// Repozytorium magazynu gracza.
    /// </summary>
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Menadżer klientów online hubów signalr.
    /// </summary>
    private readonly IOnlineClientManager _onlineClientManager;
    
    /// <summary>
    /// Mapper obiektów.
    /// </summary>
    private readonly IObjectMapper _objectMapper;
    
    /// <summary>
    /// Interfejs sprawdzający uprawnienia użytkowników.
    /// </summary>
    private readonly IPermissionChecker _permissionChecker;
    
    /// <summary>
    /// Repozytorium zadań.
    /// </summary>
    private readonly IRepository<Quest> _questRepository;
    
    /// <summary>
    /// Kontekst huba dla informacji.
    /// </summary>
    private readonly IHubContext _infoHub;
    
    /// <summary>
    /// Kontekst huba dla bonusów.
    /// </summary>
    private readonly IHubContext _bonusHub;
    
    /// <summary>
    /// Kontekst huba dla czarnego rynku.
    /// </summary>
    private readonly IHubContext _blackMarketHub;
    
    /// <summary>
    /// Kontekst huba dla zadań.
    /// </summary>
    private readonly IHubContext _questHub;

    
    /// <summary>
    /// Interfejs ILogger służy do rejestrowania komunikatów z aplikacji.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public ILogger Logger { get; set; }
        

    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="questRepository">Repozytorium zadań.</param>
    /// <param name="requirementRepository">Repozytorium wymagań.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynu plantacji.</param>
    /// <param name="entityChangeService">Serwis obsługujący zmiany encji..</param>
    /// <param name="driedFruitRepository">Repozytorium suszu.</param>
    /// <param name="lampRepository">Repozytorium lamp.</param>
    /// <param name="manureRepository">Repozytorium nawozów.</param>
    /// <param name="potRepository">Repozytorium donic.</param>
    /// <param name="seedRepository">Repozytorium nasion.</param>
    /// <param name="soilRepository">Repozytorium gleb.</param>
    /// <param name="waterRepository">Repozytorium wód.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="bonusRepository">Repozytorium bonusów.</param>
    /// <param name="questRequirementsProgressRepository">Repozytorium postępu wymagań zadań.</param>
    /// <param name="userRepository">Repozytorium użytkowników.</param>
    /// <param name="questService">Serwis podstawowy obsługujący logikę biznesową związaną z zadaniami.</param>
    /// <param name="ignoreChangeService">Serwis podstawowy obsługujący logikę ignorowania zmian dla encji.</param>
    /// <param name="playerStorageRepository">Repozytorium magazynu gracza.</param>
    /// <param name="onlineClientManager">Menadżer klientów online hubów signalr.</param>
    /// <param name="objectMapper">Mapper obiektów.</param>
    /// <param name="permissionChecker">Interfejs sprawdzający uprawnienia użytkowników.</param>
    public EntityAnalysisStateBuilder(
        IRepository<Quest> questRepository,
        IRepository<Requirement> requirementRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IEntityChangeService entityChangeService,
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<Lamp> lampRepository,
        IRepository<Manure> manureRepository,
        IRepository<Pot> potRepository,
        IRepository<Seed> seedRepository,
        IRepository<Soil> soilRepository,
        IRepository<Water> waterRepository,
        IRepository<District> districtRepository,
        IRepository<Bonus> bonusRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository,
        IRepository<User, long> userRepository,
        IQuestService questService,
        IIgnoreChangeService ignoreChangeService,
        IRepository<PlayerStorage> playerStorageRepository,
        IOnlineClientManager onlineClientManager,
        IObjectMapper objectMapper,
        IPermissionChecker permissionChecker
    )
    {
        Logger = NullLogger.Instance;
        _requirementRepository = requirementRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _entityChangeService = entityChangeService;
        _driedFruitRepository = driedFruitRepository;
        _lampRepository = lampRepository;
        _manureRepository = manureRepository;
        _potRepository = potRepository;
        _seedRepository = seedRepository;
        _soilRepository = soilRepository;
        _waterRepository = waterRepository;
        _districtRepository = districtRepository;
        _bonusRepository = bonusRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _userRepository = userRepository;
        _questService = questService;
        _ignoreChangeService = ignoreChangeService;
        _playerStorageRepository = playerStorageRepository;
        _onlineClientManager = onlineClientManager;
        _objectMapper = objectMapper;
        _permissionChecker = permissionChecker;
        _questRepository = questRepository;
            
        _blackMarketHub = GlobalHost.ConnectionManager.GetHubContext<BlackMarketHub>();
        _infoHub = GlobalHost.ConnectionManager.GetHubContext<InfoHub>();
        _questHub = GlobalHost.ConnectionManager.GetHubContext<QuestHub>();
        _bonusHub = GlobalHost.ConnectionManager.GetHubContext<BonusHub>();
    }
    
    /// <summary>
    /// Buduje stan analizy dla określonego typu działania na encji.
    /// </summary>
    /// <param name="type">Typ budowania analizy.</param>
    /// <param name="entity">Encja, na której ma zostać wykonana analiza.</param>
    /// <param name="action">Działanie wykonywane na encji.</param>
    /// <param name="page">Strona transakcji czarnego rynku (dotyczy tylko typu BlackMarket).</param>
    /// <returns>Stan analizy dla określonego typu działania na encji.</returns>
    [UnitOfWork]
    public virtual AnalysisState Build(EnumUtils.AnalysisBuildTypes type, object entity, string action, EnumUtils.BlackMarketPages? page)
    {
        var state = new AnalysisState();
        
        state.QuestRequirementsProgressRepository = _questRequirementsProgressRepository;

        switch (type)
        {
            case EnumUtils.AnalysisBuildTypes.Product:
                return GetProductAnalysisState(state, (IPlantationGeneratedEntity) entity, action);
            case EnumUtils.AnalysisBuildTypes.Storage:
                return GetStorageAnalysisState(state, (PartStorage) entity);
            case EnumUtils.AnalysisBuildTypes.BlackMarket:
                return GetBlackMarketAnalysisState(state, (BlackMarketTransaction) entity, page, action);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
        
    /// <summary>
    /// Tworzy stan analizy dla operacji dotyczącej aktualizowanej encji będącej produktem.
    /// </summary>
    /// <typeparam name="TEntity">Typ aktualizowanej encji.</typeparam>
    /// <param name="state">Bieżący stan analizy.</param>
    /// <param name="entity">Aktualizowana encja.</param>
    /// <param name="action">Wykonywane działanie.</param>
    /// <returns>Stan analizy dla aktualizowanej encji.</returns>
    private AnalysisState GetProductAnalysisState<TEntity>(AnalysisState state, TEntity entity, string action) 
        where TEntity : class, IPlantationGeneratedEntity
    {
        if (entity.PlantationStorageId == null) return state;
        
        state.GeneratedEntity = entity;
        state.DbParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Db);
        state.LastEntityPropertyChanges = _entityChangeService.GetLastEntityChangeForEntity(entity)?.PropertyChanges;

        var plantationStorage = _plantationStorageRepository.Get((int) entity.PlantationStorageId);
        var filteredQuestAndQuestProgress = _questRequirementsProgressRepository.GetAll()
            .Join(_questRepository.GetAll()
                    .Where(item => item.PlantationStorageId == plantationStorage.Id && !item.IsComplete),
                progress => progress.QuestId,
                quest => quest.Id,
                (progress, quest) => new {progress, quest}).ToList();
        
        state.PlantationStorage = plantationStorage;
        state.Quests = filteredQuestAndQuestProgress.Select(item => item.quest).ToList();
        state.QuestRequirementsProgress = filteredQuestAndQuestProgress.Select(item => item.progress).ToList();
        state.Requirements = _requirementRepository.GetAllList(item => item.DistrictId == plantationStorage.DistrictId);
        state.AvailableQuests = EntityAnalysisStateBuilderHelper.GetAvailableQuests(state);
        state.IgnoreChange = state.Entity is Product product ? _ignoreChangeService.GetIgnoreChangeForUpdatedEntity(product) : null;
        state.InfoHub = _infoHub;
        state.BonusHub = _bonusHub;
        state.QuestHub = _questHub;
        state.Action = action;
        state.District = _districtRepository.Get(plantationStorage.DistrictId);
        state.PlayerStorage = _playerStorageRepository.Single(item => item.UserId == plantationStorage.UserId);
        state.QuestsWithRequirements = EntityAnalysisStateBuilderHelper.GetQuestsWithRequirements(state);
        
        if (state.IgnoreChange != null)
            _ignoreChangeService.Remove(state.IgnoreChange.Id);
        
        state.InitStatus = true;

        return state;
    }
        
    /// <summary>
    /// Tworzy stan analizy dla operacji dotyczącej aktualizowanej encji będącej magazynem.
    /// </summary>
    /// <typeparam name="TEntity">Typ encji magazynu.</typeparam>
    /// <param name="state">Bieżący stan analizy.</param>
    /// <param name="entity">Encja magazynu.</param>
    /// <returns>Stan analizy dla operacji na magazynie.</returns>
    private AnalysisState GetStorageAnalysisState<TEntity>(AnalysisState state, TEntity entity)
        where TEntity : PartStorage
    {
        state.StorageEntity = entity;
        state.DbParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Db);
        state.LastEntityPropertyChanges = _entityChangeService.GetLastEntityChangeForEntity(entity)?.PropertyChanges;
        state.DisplayParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Display);
        state.DisplayParserStrategy = new DisplayStrategy();
        state.ReceivedLevels = EntityAnalysisStateBuilderHelper.GetReceivedLevels(state);
        state.QuestHub = _questHub;
        state.InfoHub = _infoHub;

        switch (entity)
        {
            case PlantationStorage plantationStorage:
                var filteredQuestAndQuestProgress = _questRequirementsProgressRepository.GetAll()
                    .Join(_questRepository.GetAll().Where(item => item.PlantationStorageId == plantationStorage.Id && !item.IsComplete),
                        progress => progress.QuestId,
                        quest => quest.Id,
                        (progress, quest) => new {progress, quest}).ToList();
                
                state.Quests = filteredQuestAndQuestProgress.Select(item => item.quest).ToList();
                state.QuestRequirementsProgress = filteredQuestAndQuestProgress.Select(item => item.progress).ToList();
                state.Requirements = _requirementRepository.GetAllList(item => item.DistrictId == plantationStorage.DistrictId);
                state.District = _districtRepository.Single(item => item.Id == plantationStorage.DistrictId);
                SetPlantationStorageAnalysisState(state, plantationStorage);
                break;
            case PlayerStorage playerStorage:
                var user = _userRepository.Get(playerStorage.UserId);
                var filteredQuestAndQuestProgressForPlayerStorage = _questRequirementsProgressRepository.GetAll()
                    .Join(_questRepository.GetAll()
                            .Where(item => item.PlayerStorageId == playerStorage.Id && !item.IsComplete),
                        progress => progress.QuestId,
                        quest => quest.Id,
                        (progress, quest) => new {progress, quest}).ToList();

                state.Quests = filteredQuestAndQuestProgressForPlayerStorage.Select(item => item.quest).ToList();
                state.QuestRequirementsProgress = filteredQuestAndQuestProgressForPlayerStorage.Select(item => item.progress).ToList();
                state.Requirements = _requirementRepository.GetAllList();
                state.PlantationStorages = _plantationStorageRepository.GetAllList(item => item.UserId == playerStorage.UserId);
                state.AllDistricts = _districtRepository.GetAllList();
                state.IsDistrictWarden = _permissionChecker.IsGranted(user.ToUserIdentifier(), PermissionNames.Crud_DistrictWarden);
                EntityAnalysisStateBuilderHelper.SetPlayerStorageAnalysisState(state, playerStorage);
                break;
        }

        state.InitStatus = true;
        return state;
    }

    /// <summary>
    /// Tworzy stan analizy dla operacji dotyczącej aktualizowanej encji będącej transakcją czarnego rynku.
    /// </summary>
    /// <param name="state">Bieżący stan analizy.</param>
    /// <param name="entity">Encja transakcji na czarnym rynku.</param>
    /// <param name="page">Strona transakcji czarnego rynku (jeśli dotyczy).</param>
    /// <param name="action">Wykonywane działanie.</param>
    /// <returns>Stan analizy dla operacji na transakcji czarnego rynku.</returns>
    private AnalysisState GetBlackMarketAnalysisState(AnalysisState state, BlackMarketTransaction entity, EnumUtils.BlackMarketPages? page, string action)
    {
        if (page == EnumUtils.BlackMarketPages.Buyer && entity.BuyerId != null)
            state.PlantationStorage = _plantationStorageRepository.Single(item => item.UserId == entity.BuyerId && item.DistrictId == entity.DistrictId);
        else if (page == EnumUtils.BlackMarketPages.Seller && entity.SellerId != null)
            state.PlantationStorage = _plantationStorageRepository.Single(item => item.UserId == entity.SellerId && item.DistrictId == entity.DistrictId);

        if (state.PlantationStorage == null && page != null)
        {
            Logger.Error("Problem wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a.");
            return state;
        }
            
        state.District = _districtRepository.Get(entity.DistrictId);
        state.DisplayParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Display);
        state.ObjectMapper = _objectMapper;
        state.OnlineClientManager = _onlineClientManager;
        state.BlackMarketHub = _blackMarketHub;
        state.BlackMarketEntity = entity;
        state.Action = action;

        if (page != null)
        {
            var plantationStorage = state.PlantationStorage;
            var filteredQuestAndQuestProgress = _questRequirementsProgressRepository.GetAll()
                .Join(_questRepository.GetAll().Where(item =>
                        item.PlantationStorageId == plantationStorage.Id && !item.IsComplete),
                    progress => progress.QuestId,
                    quest => quest.Id,
                    (progress, quest) => new {progress, quest}).ToList();
            
            state.QuestHub = _questHub;
            state.PlayerStorage = _playerStorageRepository.Single(item => item.UserId == plantationStorage.UserId);
            state.Quests = filteredQuestAndQuestProgress.Select(item => item.quest).ToList();
            state.QuestRequirementsProgress = filteredQuestAndQuestProgress.Select(item => item.progress).ToList();
            state.Requirements = _requirementRepository.GetAllList(item => item.DistrictId == plantationStorage.DistrictId);
            state.AvailableQuests = EntityAnalysisStateBuilderHelper.GetAvailableQuests(state);
            state.QuestsWithRequirements = EntityAnalysisStateBuilderHelper.GetQuestsWithRequirements(state);    
        }

        state.InitStatus = true;
            
        return state;
    }

    /// <summary>
    /// Ustawia stan analizy dla magazynu plantacji.
    /// </summary>
    /// <param name="state">Bieżący stan analizy.</param>
    /// <param name="plantationStorage">Encja magazynu plantacji.</param>
    private void SetPlantationStorageAnalysisState(AnalysisState state, PlantationStorage plantationStorage)
    {
        state.PlantationStorage = plantationStorage;
        state.PlayerStorage = _playerStorageRepository.Single(item => item.UserId == plantationStorage.UserId);
        
        if (state.ReceivedLevels > 0)
            state.ReceivedLvlItems = GetReceivedLvlItems(state);
        
        state.AvailableQuests = EntityAnalysisStateBuilderHelper.GetAvailableQuests(state);
        state.QuestsWithRequirements = EntityAnalysisStateBuilderHelper.GetQuestsWithRequirements(state);
    }
        
    /// <summary>
    /// Pobiera przedmioty otrzymane na określonym poziomie magazynu plantacji.
    /// </summary>
    /// <param name="state">Bieżący stan analizy.</param>
    /// <returns>Lista przedmiotów otrzymanych na określonym poziomie magazynu plantacji.</returns>
    private CurrLvlItems GetReceivedLvlItems(AnalysisState state)
    {
        var ret = new CurrLvlItems();
        var storage = state.PlantationStorage;
        var oldLvl = state.StorageEntity.Level - state.ReceivedLevels;
        var lvl = oldLvl;
        
        for (var i = 0; i < state.ReceivedLevels; i++)
        {
            lvl += 1;

            ret.DriedFruits.AddRange(_driedFruitRepository.GetAllList(item => item.PlantationLevelRequirement == lvl && item.PlantationStorageId == storage.Id));
            ret.Lamps.AddRange(_lampRepository.GetAllList(item => item.PlantationLevelRequirement == lvl && item.PlantationStorageId == storage.Id));
            ret.Manures.AddRange(_manureRepository.GetAllList(item => item.PlantationLevelRequirement == lvl && item.PlantationStorageId == storage.Id));
            ret.Pots.AddRange(_potRepository.GetAllList(item => item.PlantationLevelRequirement == lvl && item.PlantationStorageId == storage.Id));
            ret.Seeds.AddRange(_seedRepository.GetAllList(item => item.PlantationLevelRequirement == lvl && item.PlantationStorageId == storage.Id));
            ret.Soils.AddRange(_soilRepository.GetAllList(item => item.PlantationLevelRequirement == lvl && item.PlantationStorageId == storage.Id));
            ret.Waters.AddRange(_waterRepository.GetAllList(item => item.PlantationLevelRequirement == lvl && item.PlantationStorageId == storage.Id));
            ret.Bonuses.AddRange(_bonusRepository.GetAllList(item => item.PlantationLevelRequirement == lvl && item.PlantationStorageId == storage.Id));
            ret.Quests.AddRange(state.Quests.Where(item =>
                item.PlantationLevelRequirement == lvl &&
                item.IsAvailableInitially &&
                item.QuestType != DbQuestTypesNames.Achievement).Select(item =>
            {
                if (item.QuestType != DbQuestTypesNames.Event) return item;
                var isCyclic = item.CyclicTime != null && item.CyclicTime > 0;
                return item.EndTime > Clock.Now || isCyclic ? item : null;
            }).Where(item => item != null).ToList());
        }

        _questService.SetStartValues(ret.Quests);

        return ret;
    }
}
