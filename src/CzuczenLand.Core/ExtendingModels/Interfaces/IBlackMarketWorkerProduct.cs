namespace CzuczenLand.ExtendingModels.Interfaces;

public interface IBlackMarketWorkerProduct : IGeneratedEntity, INamedEntity, IOwnedAmount
{
    decimal? BlackMarketMinSellPrice { get; set; }
        
    decimal? BlackMarketMaxSellPrice { get; set; }

    bool IsBlackMarketWorkerItem { get; set; }
}