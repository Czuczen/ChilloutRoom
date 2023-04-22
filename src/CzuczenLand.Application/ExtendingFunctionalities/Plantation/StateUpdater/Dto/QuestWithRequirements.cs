using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

public class QuestWithRequirements
{
    public Quest Quest { get; set; }

    public QuestRequirementsProgress QuestRequirementsProgress { get; set; }

    public Dictionary<int, decimal> RequirementsProgress =>
        JsonConvert.DeserializeObject<Dictionary<int, decimal>>(QuestRequirementsProgress.RequirementsProgress);
        
        
    public List<Requirement> Requirements { get; } = new();
}