using System;
using Abp.Auditing;
using Abp.Domain.Entities;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingModels.Models.Shared;

[Audited]
public class Product : Entity<int>, IPlantationGeneratedEntity, IBlackMarketWorkerProduct, IS2Product, IStorageProduct
{
    public string Name { get; set; }

    public int PlantationLevelRequirement { get; set; }
        
    public decimal? SellPrice { get; set; }
        
    public decimal? BuyPrice { get; set; }
        
    public bool IsShopItem { get; set; }
        
    public decimal? BlackMarketMinSellPrice { get; set; }
        
    public decimal? BlackMarketMaxSellPrice { get; set; }
        
    public bool IsBlackMarketWorkerItem { get; set; }
        
    public bool PlayerCanSellInBlackMarket { get; set; }
        
    public Guid IgnoreChangeGuid { get; set; }
        
    public decimal OwnedAmount { get; set; }

    public int GeneratedTypeId { get; set; }

    public int? PlantationStorageId { get; set; }
}