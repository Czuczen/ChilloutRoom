using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.Products;

[Audited]
public class Pot : ProductEnhancements
{
    public int MaxRangeOfSoilClass { get; set; }
        
    public decimal Capacity { get; set; }
        
    public int InUseCount { get; set; }
}