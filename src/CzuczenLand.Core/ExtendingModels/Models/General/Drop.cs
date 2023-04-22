using Abp.Domain.Entities;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingModels.Models.General;

public class Drop : Entity<int>, IDistrictEntity
{
    public string Name { get; set; }
        
    public int? GeneratedTypeId { get; set; }
        
    public decimal? ChanceForDrop { get; set; }
        
    public decimal? ItemAmount { get; set; }

    public decimal? Gold { get; set; }
        
    public int? Prestige { get; set; }
        
    public int? QuestToken { get; set; }
        
    public int? DealerToken { get; set; }
        
    public int? BlackMarketToken { get; set; }
        
    public int? DonToken { get; set; }
        
    public int? UnlockToken { get; set; }
        
    public int? Honor { get; set; }
        
    public decimal? Experience { get; set; }

    public int DistrictId { get; set; }
}