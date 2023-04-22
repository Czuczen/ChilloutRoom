namespace CzuczenLand.ExtendingModels.Interfaces;

public interface IStorageProduct : INamedEntity, IOwnedAmount
{
    decimal? SellPrice { get; set; }
        
    decimal? BuyPrice { get; set; }
}