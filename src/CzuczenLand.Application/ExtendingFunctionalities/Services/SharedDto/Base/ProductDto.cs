using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

/// <summary>
/// Klasa reprezentująca dane transferowe (DTO) produktu, dziedzicząca po EntityDto int, IPlantationStorageResource oraz IGeneratedEntity.
/// </summary>
public class ProductDto : EntityDto<int>, IPlantationStorageResource, IGeneratedEntity
{
    /// <summary>
    /// Nazwa produktu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.Name)]
    public string Name { get; set; }

    /// <summary>
    /// Wymagany poziom plantacji dla produktu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.PlantationLevelRequirement)]
    public int PlantationLevelRequirement { get; set; }
        
    /// <summary>
    /// Cena sprzedaży produktu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.SellPrice)]
    public decimal? SellPrice { get; set; }
        
    /// <summary>
    /// Cena zakupu produktu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.BuyPrice)]
    public decimal? BuyPrice { get; set; }
        
    /// <summary>
    /// Czy produkt jest przedmiotem sklepu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.IsShopItem)]
    public bool IsShopItem { get; set; }
        
    /// <summary>
    /// Minimalna cena sprzedaży na czarnym rynku.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.BlackMarketMinSellPrice)]
    public decimal? BlackMarketMinSellPrice { get; set; }
        
    /// <summary>
    /// Maksymalna cena sprzedaży na czarnym rynku.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.BlackMarketMaxSellPrice)]
    public decimal? BlackMarketMaxSellPrice { get; set; }
        
    /// <summary>
    /// Czy produkt jest przedmiotem pracownika czarnego rynku.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.IsBlackMarketWorkerItem)]
    public bool IsBlackMarketWorkerItem { get; set; }
        
    /// <summary>
    /// Czy gracz może sprzedawać produkt na czarnym rynku.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.PlayerCanSellInBlackMarket)]
    public bool PlayerCanSellInBlackMarket { get; set; }
    
    /// <summary>
    /// Identyfikator do ignorowania zmian przy aktualizacji stanu plantacji.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.IgnoreChangeGuid)]
    public Guid IgnoreChangeGuid { get; set; }
        
    /// <summary>
    /// Posiadana ilość produktu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.OwnedAmount)]
    public decimal OwnedAmount { get; set; }

    /// <summary>
    /// Identyfikator powiązanego typu generowanego.
    /// </summary>
    [DisplayName(EntitiesHrNames.GeneratedType), Display(GroupName = EntitiesDbNames.GeneratedType)]
    public int GeneratedTypeId { get; set; }

    /// <summary>
    /// Identyfikator powiązanego magazynu plantacji.
    /// </summary>
    [DisplayName(EntitiesHrNames.PlantationStorage), Display(GroupName = EntitiesDbNames.PlantationStorage)]
    public int? PlantationStorageId { get; set; }
}