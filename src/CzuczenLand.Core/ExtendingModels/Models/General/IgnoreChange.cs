using System;
using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingModels.Models.General;

public class IgnoreChange : Entity<int>
{
    public int EntityId { get; set; }
        
    public string EntityName { get; set; }
        
    public Guid IgnoreChangeGuid { get; set; }
}