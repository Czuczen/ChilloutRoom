using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.Products;

[Audited]
public class Water : ProductEnhancements
{
    public decimal CapacityInPotRequirement { get; set; }
}