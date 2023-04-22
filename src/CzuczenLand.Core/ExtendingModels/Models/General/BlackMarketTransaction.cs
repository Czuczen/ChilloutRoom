using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingModels.Models.General;

public class BlackMarketTransaction : Entity<int>
{
    public int ItemId { get; set; }
        
    public string ItemName { get; set; }
        
    public string ItemEntityName { get; set; }
        
    public long? SellerId { get; set; }
        
    public string SellerName { get; set; }

    public decimal Price { get; set; }
        
    public decimal Quantity { get; set; }

    public bool IsCanceled { get; set; }
        
    public long? BuyerId { get; set; }
        
    public int GeneratedTypeId { get; set; }
        
    public int DistrictId { get; set; }
}