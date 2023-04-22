using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.QuestsStateUpdater.Dto;

public class UpdateQuest
{
    public int QuestId { get; set; }
        
    public bool InProgress { get; set; }
        
    public bool QuestIsComplete { get; set; }

    public List<UpdateRequirement> UpdatingRequirements { get; } = new();
}