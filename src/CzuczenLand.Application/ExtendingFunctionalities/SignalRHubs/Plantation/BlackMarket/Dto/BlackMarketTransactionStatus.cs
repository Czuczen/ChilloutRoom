namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.BlackMarket.Dto;

/// <summary>
/// Klasa reprezentująca status transakcji na czarnym rynku.
/// </summary>
public class BlackMarketTransactionStatus
{
    /// <summary>
    /// Status transakcji.
    /// </summary>
    public string Status { get; set; }
        
    /// <summary>
    /// Wiadomość dotycząca transakcji.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Identyfikator dzielnicy.
    /// </summary>
    public int DistrictId { get; set; }
}