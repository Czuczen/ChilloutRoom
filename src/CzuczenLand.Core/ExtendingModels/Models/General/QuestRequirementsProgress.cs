using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingModels.Models.General;

public class QuestRequirementsProgress : Entity<int>
{
    public int QuestId { get; set; }
        
    public string RequirementsProgress { get; set; }
}