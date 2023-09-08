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
    /// Status sprzedaży.
    /// </summary>
    public string Status { get; set; }
}