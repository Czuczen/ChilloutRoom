using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingModels.Models.ManyToManyRelations;

public class DropQuest : Entity<int>
{
    public int DropId { get; set; }
        
    public int QuestId { get; set; }
}