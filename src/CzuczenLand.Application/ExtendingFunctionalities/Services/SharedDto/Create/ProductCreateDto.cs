using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

public class ProductCreateDto : IGeneratedEntity
{
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.PlantationLevelRequirement)]
    public int PlantationLevelRequirement { get; set; }
    
    [DisplayName(ProductFieldsHrNames.SellPrice)]
    public decimal? SellPrice { get; set; }
    
    [DisplayName(ProductFieldsHrNames.BuyPrice)]
    public decimal? BuyPrice { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.IsShopItem)]
    public bool IsShopItem { get; set; }
        
    [DisplayName(ProductFieldsHrNames.BlackMarketMinSellPrice)]
    public decimal? BlackMarketMinSellPrice { get; set; }
        
    [DisplayName(ProductFieldsHrNames.BlackMarketMaxSellPrice)]
    public decimal? BlackMarketMaxSellPrice { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.IsBlackMarketWorkerItem)]
    public bool IsBlackMarketWorkerItem { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.PlayerCanSellInBlackMarket)]
    public bool PlayerCanSellInBlackMarket { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.OwnedAmount)]
    public decimal OwnedAmount { get; set; }

    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.GeneratedType)]
    public int GeneratedTypeId { get; set; }
}