using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

public class FilteredQuests
{
    public QuestInfoCreation QuestInfoCreation { get; set; }
    
    public List<Quest> Quests { get; set; }
}