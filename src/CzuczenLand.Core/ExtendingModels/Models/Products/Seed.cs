using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.Products;

[Audited]
public class Seed : ProductEnhancements
{
    public decimal ManureConsumption { get; set; }

    public decimal WaterConsumption { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }
        
    public decimal CapacityInPotRequirement { get; set; }
}