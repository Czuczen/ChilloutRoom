using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

public class QuestInfoCreation
{
    public List<Quest> QuestDefinitions { get; set; }
        
    public List<DropQuest> DropQuestRelations { get; set; }
        
    public List<Drop> Drops { get; set; }
        
    public List<Requirement> Requirements { get; set; }
        
    public List<QuestRequirementsProgress> QuestsRequirementsProgress { get; set; }
}