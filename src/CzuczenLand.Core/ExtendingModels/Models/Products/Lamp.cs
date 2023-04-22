using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.Products;

[Audited]
public class Lamp : ProductEnhancements
{
    public int InUseCount { get; set; }
        
    public decimal CapacityInPotRequirement { get; set; }
}