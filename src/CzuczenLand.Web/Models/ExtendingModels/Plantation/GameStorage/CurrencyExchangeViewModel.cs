using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.GameStorage;

[AutoMapFrom(typeof(CurrencyExchange))]
public class CurrencyExchangeViewModel
{
    private decimal _ownedAmount { get; set; }
        
        
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal ExchangeRate { get; set; }
        
    /// <summary>
    /// musi być public dla mapowania
    /// </summary>
    public int PlantationStorageId { get; set; }
        
    public string PlantationStorageIdWithCurrencyName => PlantationStorageId + CurrencyName;
        
    public string CurrencyName { get; set; }
        
    public decimal SellPrice { get; set; }
        
    public decimal BuyPrice { get; set; }
        
    public bool IsOnlyToPlantationGoldTransfer { get; set; }
        
    public string OwnedAmount
    {
        get => _ownedAmount + PlantationManagerHelper.GetMeasureUnitForCurrencyExchange(CurrencyName);
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _ownedAmount = parsedValue;
            }   
        }
    }
}