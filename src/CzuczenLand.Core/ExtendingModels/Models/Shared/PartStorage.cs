using Abp.Auditing;
using Abp.Domain.Entities;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingModels.Models.Shared;

[Audited]
public class PartStorage : Entity<int>, IUserStorageEntity
{
    public string Name { get; set; }
        
    public int Level { get; set; }
        
    public decimal GainedExperience { get; set; }
        
    public decimal Gold { get; set; }
        
    public long UserId { get; set; }
}