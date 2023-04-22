using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.Products;

[Audited]
public class Bonus : ProductEnhancements
{
    public bool IsArtifact { get; set; }
        
    public decimal? ArtifactPutCost { get; set; }
        
    public decimal? ArtifactPullCost { get; set; }
        
    public int? RemainingActiveTime { get; set; }
        
    public int? ActiveTimePerUse { get; set; }

    public string Color { get; set; }
        
    public int Usages { get; set; }
        
    public bool IsActive { get; set; }
        
    public bool? IsStackable { get; set; }

    public decimal? IncreaseDropChanceFromQuests { get; set; }
}