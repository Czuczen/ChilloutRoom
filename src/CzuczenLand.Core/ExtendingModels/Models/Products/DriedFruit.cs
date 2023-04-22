using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.Products;

[Audited]
public class DriedFruit : Product
{
    public int? OfferChance { get; set; }

    public bool AvailableInCustomerZone { get; set; }
        
    public decimal? CustomerZonePrice { get; set; }
}