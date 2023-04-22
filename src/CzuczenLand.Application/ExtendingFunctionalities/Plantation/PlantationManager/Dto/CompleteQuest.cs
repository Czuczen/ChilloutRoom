using System.Collections.Generic;
using Abp.ObjectMapping;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Db;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

public class CompleteQuest
{
    public decimal GainedExp = 0;
    public decimal GainedGold = 0;
    public int GainedPrestige = 0;
    public int GainedQuestTokens  = 0;
    public int GainedDealerTokens  = 0;
    public int GainedBlackMarketTokens  = 0;
    public int GainedHonor  = 0;
        
        
    public Quest Quest { get; set; }
        
    public PlayerStorage PlayerStorage { get; set; }
        
    public PlantationStorage PlantationStorage { get; set; }
        
    public Quest QuestDefinition { get; set; }

    public List<DropQuest> Connections { get; set; }
        
    public IObjectMapper ObjectMapper { get; set; }
        
    public DbStrategy ParserDbStrategy { get; set; }
        
    public IParser DbParser { get; set; }
        
    public List<GeneratedType> GeneratedTypes { get; set; }
        
    public long UserId { get; set; }
        
    public District District { get; set; }
        
    public int GainedDonTokens { get; set; }
        
    public int GainedUnlockTokens { get; set; }
        
    public bool InitStatus { get; set; }
        
    public QuestInfoCreation QuestInfoCreation { get; set; }
        
    public QuestRequirementsProgress QuestRequirementsProgress { get; set; }
        
    public List<string> DropsNotification { get; } = new();
        
    public List<Drop> ConnectedDrops { get; } = new();
}