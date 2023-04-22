using Abp.Auditing;

namespace CzuczenLand.ExtendingModels.Models.Shared;

[Audited]
public class ProductEnhancements : Product
{
    public decimal IncreaseGrowingSpeed { get; set; }

    public int IncreaseTimeOfInsensitivity { get; set; }
        
    public int IncreaseChanceForSeed { get; set; }
        
    public decimal IncreaseDriedFruitAmount { get; set; }
        
    public int IncreaseSeedAmount { get; set; }
        
    public decimal IncreaseGainedExp { get; set; }
        
    public string SetName { get; set; }
}