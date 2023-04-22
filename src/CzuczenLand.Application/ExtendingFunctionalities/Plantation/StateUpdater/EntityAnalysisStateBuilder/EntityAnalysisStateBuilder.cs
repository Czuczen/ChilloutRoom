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

public class EntityAnalysisStateBuilder : IEntityAnalysisStateBuilder
{
    private readonly IRepository<Requirement> _requirementRepository;
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    private readonly IEntityChangeService _entityChangeService;
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    private readonly IRepository<Lamp> _lampRepository;
    private readonly IRepository<Manure> _manureRepository;
    private readonly IRepository<Pot> _potRepository;
    private readonly IRepository<Seed> _seedRepository;
    private readonly IRepository<Soil> _soilRepository;
    private readonly IRepository<Water> _waterRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IRepository<Bonus> _bonusRepository;
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    private readonly IRepository<User, long> _userRepository;
    private readonly IQuestService _questService;
    private readonly IIgnoreChangeService _ignoreChangeService;
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    private readonly IOnlineClientManager _onlineClientManager;
    private readonly IObjectMapper _objectMapper;
    private readonly IPermissionChecker _permissionChecker;
    private readonly IRepository<Quest> _questRepository;
    private readonly IHubContext _infoHub;
    private readonly IHubContext _bonusHub;
    private readonly IHubContext _blackMarketHub;
    private readonly IHubContext _questHub;

    public ILogger Logger { get; set; }
        

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

    private void SetPlantationStorageAnalysisState(AnalysisState state, PlantationStorage plantationStorage)
    {
        state.PlantationStorage = plantationStorage;
        state.PlayerStorage = _playerStorageRepository.Single(item => item.UserId == plantationStorage.UserId);
        
        if (state.ReceivedLevels > 0)
            state.ReceivedLvlItems = GetReceivedLvlItems(state);
        
        state.AvailableQuests = EntityAnalysisStateBuilderHelper.GetAvailableQuests(state);
        state.QuestsWithRequirements = EntityAnalysisStateBuilderHelper.GetQuestsWithRequirements(state);
    }
        
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
