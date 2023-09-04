using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

/// <summary>
/// Klasa reprezentująca dane transferowe (DTO) do tworzenia definicji produktu.
/// Implementuje interfejs IGeneratedEntity.
/// </summary>
public class ProductCreateDto : IGeneratedEntity
{
    /// <summary>
    /// Nazwa produktu.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.Name)]
    public string Name { get; set; }
    
    /// <summary>
    /// Wymagany poziom plantacji dla produktu.
    /// </summary>
    [FieldIsRequired]
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
    [FieldIsRequired]
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
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.IsBlackMarketWorkerItem)]
    public bool IsBlackMarketWorkerItem { get; set; }
        
    /// <summary>
    /// Czy gracz może sprzedawać produkt na czarnym rynku.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.PlayerCanSellInBlackMarket)]
    public bool PlayerCanSellInBlackMarket { get; set; }
        
    /// <summary>
    /// Posiadana ilość produktu.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductFieldsHrNames.OwnedAmount)]
    public decimal OwnedAmount { get; set; }

    /// <summary>
    /// Identyfikator powiązanego typu generowanego.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.GeneratedType)]
    public int GeneratedTypeId { get; set; }
}