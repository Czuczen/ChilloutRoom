using Abp.Domain.Entities;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingModels.Models.General;

public class GeneratedType : Entity<int>, IDistrictEntity
{
    public string Name { get; set; }
        
    public string EntityName { get; set; }
        
    public int DistrictId { get; set; }
}