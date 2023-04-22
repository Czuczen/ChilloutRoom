using System.Collections.Generic;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.ObjectMapping;
using Abp.RealTime;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Shared;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

public class AnalysisState
{
    private long? _userId;
        

    public List<Quest> AvailableQuests { get; set; }
        
    public List<QuestWithRequirements> QuestsWithRequirements { get; set; }
        
    public PartStorage StorageEntity { get; set; }
        
    public IPlantationGeneratedEntity GeneratedEntity { get; set; }

    public object Entity => (object) GeneratedEntity ?? StorageEntity;

    public PlantationStorage PlantationStorage { get; set; }

    public bool InitStatus { get; set; }
        
    public PlayerStorage PlayerStorage { get; set; }
        
    public IHubContext QuestHub { get; set; }
        
    public IHubContext InfoHub { get; set; }
        
    public IHubContext BlackMarketHub { get; set; }
        
    public IHubContext BonusHub { get; set; }
        
    public ICollection<EntityPropertyChange> LastEntityPropertyChanges { get; set; }
        
    public BlackMarketTransaction BlackMarketEntity { get; set; }
        
    public bool IsDistrictWarden { get; set; }
        
    public string Action { get; set; }
        
    public IOnlineClientManager OnlineClientManager { get; set; }
        
    public IObjectMapper ObjectMapper { get; set; }
        
    public List<Select2Data> S2DistrictsList { get; set; }
        
    public CurrLvlItems ReceivedLvlItems { get; set; }
        
    public District District { get; set; }
        
    public int ReceivedLevels { get; set; }

    public IRepository<QuestRequirementsProgress> QuestRequirementsProgressRepository { get; set; }

    public List<Quest> Quests { get; set; }
        
    public List<QuestRequirementsProgress> QuestRequirementsProgress { get; set; }
        
    public IgnoreChange IgnoreChange { get; set; }
        
    public List<Requirement> Requirements { get; set; }

    public List<District> AllDistricts { get; set; }

    public List<PlantationStorage> PlantationStorages { get; set; }

    public IParser DisplayParser { get; set; }
        
    public IParser DbParser { get; set; }
        
    public DisplayStrategy DisplayParserStrategy { get; set; }
        
    public long UserId
    {
        get => _userId ??= PlantationStorage.UserId;
        set => _userId = value;
    }

    public bool NeedIgnoreChange
    {
        get
        {
            if (Entity is not Product) return false;
            return IgnoreChange != null;
        }
    }
        
    public List<District> NewDistricts { get; } = new();
}