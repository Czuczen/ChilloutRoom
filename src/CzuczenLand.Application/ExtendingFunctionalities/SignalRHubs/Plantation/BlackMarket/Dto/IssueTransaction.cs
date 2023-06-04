namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.BlackMarket.Dto;

/// <summary>
/// Klasa reprezentująca informacje od użytkownika o tworzonej transakcji dla produktu.
/// </summary>
public class IssueTransaction
{
    /// <summary>
    /// ID przedmiotu.
    /// </summary>
    public int ItemId { get; set; }
        
    /// <summary>
    /// Nazwa przedmiotu.
    /// </summary>
    public string ItemName { get; set; }
        
    /// <summary>
    /// Nazwa encji przedmiotu.
    /// </summary>
    public string ItemEntityName { get; set; }
        
    /// <summary>
    /// Ilość przedmiotów.
    /// </summary>
    public decimal Quantity { get; set; }
        
    /// <summary>
    /// Cena przedmiotu.
    /// </summary>
    public decimal Price { get; set; }
}