using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;

public class BlackMarketSellItem
{
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemId)]
    public int ItemId { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemName)]
    public string ItemName { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemEntityName)]
    public string ItemEntityName { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.OwnedAmount)]
    public decimal OwnedAmount { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.BlackMarketMinSellPrice)]
    public decimal BlackMarketMinSellPrice { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.BlackMarketMaxSellPrice)]
    public decimal BlackMarketMaxSellPrice { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.QuantityInputStep)]
    public string QuantityInputStep { get; set; }
}