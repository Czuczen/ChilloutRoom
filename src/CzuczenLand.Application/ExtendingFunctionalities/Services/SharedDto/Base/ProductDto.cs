using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

public class ProductDto : EntityDto<int>, IPlantationStorageResource, IGeneratedEntity
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
    
    [DisplayName(ProductFieldsHrNames.IgnoreChangeGuid)]
    public Guid IgnoreChangeGuid { get; set; }
        
    [DisplayName(ProductFieldsHrNames.OwnedAmount)]
    public decimal OwnedAmount { get; set; }

    [DisplayName(EntitiesHrNames.GeneratedType), Display(GroupName = EntitiesDbNames.GeneratedType)]
    public int GeneratedTypeId { get; set; }

    [DisplayName(EntitiesHrNames.PlantationStorage), Display(GroupName = EntitiesDbNames.PlantationStorage)]
    public int? PlantationStorageId { get; set; }
}