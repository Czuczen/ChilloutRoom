using Abp.AutoMapper;
using Abp.Domain.Entities;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.General;

[AutoMapFrom(typeof(PartMessages))]
public class Requirement : Entity<int>, IDistrictEntity
{
    public string Name { get; set; }
        
    public string CustomEntityName { get; set; }
        
    public int? GeneratedTypeId { get; set; }

    public string Condition { get; set; }
        
    public string Comparer { get; set; }

    public decimal Amount { get; set; }

    public int DistrictId { get; set; }
}