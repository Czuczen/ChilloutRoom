using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

public class BlackMarket
{
    public List<string> BuyDbProperties { get; set; }
        
    public List<string> BuyHrProperties { get; set; }
        
    public List<string> SellDbProperties { get; set; }
        
    public List<string> SellHrProperties { get; set; }
        
    public List<string> IssuedDbProperties { get; set; }
        
    public List<string> IssuedHrProperties { get; set; }
        
    public List<object> BuyRecords { get; set; }
        
    public List<BlackMarketSellItem> SellRecords { get; set; }
        
    public List<object> IssuedRecords { get; set; }
}