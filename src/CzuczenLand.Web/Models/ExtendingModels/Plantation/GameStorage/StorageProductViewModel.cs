using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.GameStorage;

[AutoMapFrom(typeof(DriedFruit),
    typeof(Lamp),
    typeof(Manure),
    typeof(Pot),
    typeof(Seed),
    typeof(Soil),
    typeof(Water),
    typeof(Bonus)
)]
public class StorageProductViewModel
{
    /// <summary>
    /// musi być public bo inaczej refleksja nie ustawi wartości
    /// </summary>
    public int Id { get; set; }
        
    public string Name { get; set; }
        
    public bool IsShopItem { get; set; }
        
    public bool IsArtifact { get; set; }
        
    public bool IsActive { get; set; }
        
    public decimal BuyPrice { get; set; }
        
    public decimal SellPrice { get; set; }

    public decimal OwnedAmount { get; set; }
        
    public string OwnedAmountWithMeasureUnit => OwnedAmount + PlantationManagerHelper.GetMeasureUnitByEntityName(EntityName);

    public string EntityName { get; set; }
            
    public object ProductInfo { get; set; }

    public string StorageProductId => Id + EntityName;
}