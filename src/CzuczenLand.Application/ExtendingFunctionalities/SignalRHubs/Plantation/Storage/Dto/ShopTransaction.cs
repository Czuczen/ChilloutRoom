using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Storage.Dto;

public class ShopTransaction
{
    public bool SuccessfulTransaction { get; set; }
    
    public readonly List<string> InfoMessage = new();
}