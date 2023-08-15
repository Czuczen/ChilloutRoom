using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

/// <summary>
/// Klasa reprezentująca czarny rynek.
/// </summary>
public class BlackMarket
{
    /// <summary>
    /// Lista bazodanowych nazw właściwości dla operacji kupna.
    /// </summary>
    public List<string> BuyDbProperties { get; set; }
        
    /// <summary>
    /// Lista czytelnych nazw właściwości dla operacji kupna.
    /// </summary>
    public List<string> BuyHrProperties { get; set; }
        
    /// <summary>
    /// Lista bazodanowych nazw właściwości dla operacji sprzedaży.
    /// </summary>
    public List<string> SellDbProperties { get; set; }
        
    /// <summary>
    /// Lista czytelnych nazw właściwości dla operacji sprzedaży.
    /// </summary>
    public List<string> SellHrProperties { get; set; }
        
    /// <summary>
    /// Lista bazodanowych nazw właściwości dla operacji wystawienia.
    /// </summary>
    public List<string> IssuedDbProperties { get; set; }
        
    /// <summary>
    /// Lista czytelnych nazw właściwości dla operacji wystawienia.
    /// </summary>
    public List<string> IssuedHrProperties { get; set; }
        
    /// <summary>
    /// Lista rekordów dla operacji kupna.
    /// </summary>
    public List<object> BuyRecords { get; set; }
        
    /// <summary>
    /// Lista rekordów dla operacji sprzedaży.
    /// </summary>
    public List<BlackMarketSellItem> SellRecords { get; set; }
        
    /// <summary>
    /// Lista rekordów dla operacji wystawienia.
    /// </summary>
    public List<object> IssuedRecords { get; set; }
}