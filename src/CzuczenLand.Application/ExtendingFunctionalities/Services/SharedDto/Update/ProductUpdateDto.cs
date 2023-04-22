using System.ComponentModel;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

public class ProductUpdateDto : EntityDto<int>
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
}