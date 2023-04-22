using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.Products;

[Audited]
public class Soil : ProductEnhancements
{
    public decimal CapacityInPotRequirement { get; set; }
        
    public int SoilClass { get; set; }
}