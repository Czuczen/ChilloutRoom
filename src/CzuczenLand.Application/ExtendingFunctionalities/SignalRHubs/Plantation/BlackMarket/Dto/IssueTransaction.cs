namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.BlackMarket.Dto;

public class IssueTransaction
{
    public int ItemId { get; set; }
        
    public string ItemName { get; set; }
        
    public string ItemEntityName { get; set; }
        
    public decimal Quantity { get; set; }
        
    public decimal Price { get; set; }
}