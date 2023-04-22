using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.General;

[Audited]
public class PlantationStorage : PartStorage
{
    public decimal CurrExp { get; set; }
    public decimal ExpToNextLevel { get; set; }
  
        
        
    public int QuestToken { get; set; }
    public int DealerToken { get; set; }
    public int BlackMarketToken { get; set; }
    public int DonToken { get; set; }
    public int UnlockToken { get; set; }
    public int Prestige { get; set; }


        
    public int BuffSlotsInUse { get; set; }
    public int ArtifactSlotsInUse { get; set; }
    public int StartedDailyQuestsCount { get; set; }
    public int StartedWeeklyQuestsCount { get; set; }
        
        
        
    public int MaxBuffsSlots { get; set; }
    public int MaxArtifactSlots { get; set; }
    public int MaxDailyQuestsCount { get; set; }
    public int MaxWeeklyQuestsCount { get; set; }
        
        
        
    public int UnlockedBuffsSlots { get; set; }
    public int UnlockedArtifactSlots { get; set; }
    public int UnlockedDailyQuestsCount { get; set; }
    public int UnlockedWeeklyQuestsCount { get; set; }
        
        
        
    public int DistrictId { get; set; }
}