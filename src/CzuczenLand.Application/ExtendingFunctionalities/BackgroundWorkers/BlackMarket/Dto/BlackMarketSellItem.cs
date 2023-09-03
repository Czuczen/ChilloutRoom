using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;

/// <summary>
/// Klasa reprezentująca przedmiot do sprzedaży na czarnym rynku.
/// </summary>
public class BlackMarketSellItem
{
    /// <summary>
    /// Identyfikator przedmiotu.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemId)]
    public int ItemId { get; set; }
        
    /// <summary>
    /// Nazwa przedmiotu.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemName)]
    public string ItemName { get; set; }
        
    /// <summary>
    /// Nazwa encji przedmiotu.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemEntityName)]
    public string ItemEntityName { get; set; }
        
    /// <summary>
    /// Posiadana ilość przedmiotu.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.OwnedAmount)]
    public decimal OwnedAmount { get; set; }
        
    /// <summary>
    /// Minimalna cena sprzedaży przedmiotu na czarnym rynku.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.BlackMarketMinSellPrice)]
    public decimal BlackMarketMinSellPrice { get; set; }
        
    /// <summary>
    /// Maksymalna cena sprzedaży przedmiotu na czarnym rynku.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.BlackMarketMaxSellPrice)]
    public decimal BlackMarketMaxSellPrice { get; set; }

    /// <summary>
    /// Ilość miejsc po przecinku dla posiadanej ilości przedmiotu.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.QuantityInputStep)]
    public string QuantityInputStep { get; set; }
}