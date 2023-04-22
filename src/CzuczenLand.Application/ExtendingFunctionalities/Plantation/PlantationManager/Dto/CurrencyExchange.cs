namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

public class CurrencyExchange
{
    public string Description { get; set; }

    public int ExchangeRate { get; set; }
        
    public int PlantationStorageId { get; set; }
        
    public decimal OwnedAmount { get; set; }
        
    public string CurrencyName { get; set; }
        
    public decimal SellPrice { get; set; }
        
    public decimal BuyPrice { get; set; }
        
    public bool IsOnlyToPlantationGoldTransfer { get; set; }
}