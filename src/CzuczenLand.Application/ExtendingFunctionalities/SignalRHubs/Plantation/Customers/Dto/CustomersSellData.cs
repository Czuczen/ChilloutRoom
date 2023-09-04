namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Customers.Dto;

/// <summary>
/// Klasa reprezentująca dane dotyczące sprzedaży suszu dla klienta.
/// </summary>
public class CustomersSellData
{
    /// <summary>
    /// Wiadomość informująca o sprzedaży suszu.
    /// </summary>
    public string SellMessage { get; set; }
        
    /// <summary>
    /// Ilość pozostałego suszu.
    /// </summary>
    public decimal DriedFruitAmount { get; set; }
        
    /// <summary>
    /// Ilość złota na plantacji.
    /// </summary>
    public decimal PlantationGold { get; set; }
        
    /// <summary>
    /// Id sprzedanego suszu.
    /// </summary>
    public int DriedFruitId { get; set; }
        
    /// <summary>
    /// Status sprzedaży.
    /// </summary>
    public string Status { get; set; }
}