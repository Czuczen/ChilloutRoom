using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

public class ProductUpdateDefinitionDto
{
    [DisplayName(ProductFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [DisplayName(ProductFieldsHrNames.PlantationLevelRequirement)]
    public int PlantationLevelRequirement { get; set; }
        
    [DisplayName(ProductFieldsHrNames.SellPrice)]
    public decimal? SellPrice { get; set; }
        
    [DisplayName(ProductFieldsHrNames.BuyPrice)]
    public decimal? BuyPrice { get; set; }
        
    [DisplayName(ProductFieldsHrNames.IsShopItem)]
    public bool IsShopItem { get; set; }
        
    [DisplayName(ProductFieldsHrNames.BlackMarketMinSellPrice)]
    public decimal? BlackMarketMinSellPrice { get; set; }
        
    [DisplayName(ProductFieldsHrNames.BlackMarketMaxSellPrice)]
    public decimal? BlackMarketMaxSellPrice { get; set; }

    [DisplayName(ProductFieldsHrNames.IsBlackMarketWorkerItem)]
    public bool IsBlackMarketWorkerItem { get; set; }
        
    [DisplayName(ProductFieldsHrNames.PlayerCanSellInBlackMarket)]
    public bool PlayerCanSellInBlackMarket { get; set; }
}
