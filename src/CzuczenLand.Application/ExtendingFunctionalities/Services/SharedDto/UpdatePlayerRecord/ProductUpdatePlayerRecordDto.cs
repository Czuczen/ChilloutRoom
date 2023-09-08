using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdatePlayerRecord;

/// <summary>
/// Klasa reprezentująca pola aktualizowane w produkcie powiązanym z graczem.
/// </summary>
public class ProductUpdatePlayerRecordDto
{
    /// <summary>
    /// Nazwa produktu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.Name)]
    public string Name { get; set; }
        
    /// <summary>
    /// Wymagany poziom plantacji do odblokowania produktu.
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
    /// Wskazuje, czy produkt jest dostępny w sklepie.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.IsShopItem)]
    public bool IsShopItem { get; set; }
        
    /// <summary>
    /// Minimalna cena sprzedaży produktu na czarnym rynku.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.BlackMarketMinSellPrice)]
    public decimal? BlackMarketMinSellPrice { get; set; }
        
    /// <summary>
    /// Maksymalna cena sprzedaży produktu na czarnym rynku.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.BlackMarketMaxSellPrice)]
    public decimal? BlackMarketMaxSellPrice { get; set; }

    /// <summary>
    /// Wskazuje, czy produkt jest dostępny dla pracownika czarnego rynku.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.IsBlackMarketWorkerItem)]
    public bool IsBlackMarketWorkerItem { get; set; }
        
    /// <summary>
    /// Wskazuje, czy gracz może sprzedawać produkt na czarnym rynku.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.PlayerCanSellInBlackMarket)]
    public bool PlayerCanSellInBlackMarket { get; set; }
}
