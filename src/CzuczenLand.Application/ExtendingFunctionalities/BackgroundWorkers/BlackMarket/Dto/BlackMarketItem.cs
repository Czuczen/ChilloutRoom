using System.ComponentModel;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;

[AutoMapFrom(typeof(BlackMarketTransaction))]
public class BlackMarketItem : Entity<int>
{
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemId)]
    public int ItemId { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemName)]
    public string ItemName { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.ItemEntityName)]
    public string ItemEntityName { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.SellerName)]
    public string SellerName { get; set; }

    [DisplayName(BlackMarketTransactionFieldsHrNames.Price)]
    public decimal Price { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.Quantity)]
    public decimal Quantity { get; set; }
        
    [DisplayName(BlackMarketTransactionFieldsHrNames.DistrictId)]
    public int DistrictId { get; set; }
}