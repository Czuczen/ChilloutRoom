using System.ComponentModel;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;

/// <summary>
/// Klasa reprezentująca przedmiot na czarnym rynku.
/// </summary>
[AutoMapFrom(typeof(BlackMarketTransaction))]
public class BlackMarketItem : Entity<int>
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
    /// Nazwa sprzedawcy.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.SellerName)]
    public string SellerName { get; set; }

    /// <summary>
    /// Cena przedmiotu.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.Price)]
    public decimal Price { get; set; }
    
    /// <summary>
    /// Ilość przedmiotu.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.Quantity)]
    public decimal Quantity { get; set; }
    
    /// <summary>
    /// Identyfikator dzielnicy.
    /// </summary>
    [DisplayName(BlackMarketTransactionFieldsHrNames.DistrictId)]
    public int DistrictId { get; set; }
}