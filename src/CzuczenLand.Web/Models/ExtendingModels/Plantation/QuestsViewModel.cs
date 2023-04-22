using System.Collections.Generic;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.QuestInfo;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation;

public class QuestsViewModel
{
    public List<QuestInfoViewModel> AvailableQuests { get; } = new();
        
    public List<QuestInfoViewModel> InProgressQuests { get; } = new();
        
    public List<QuestInfoViewModel> CompletedQuests { get; } = new();
}