namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;

/// <summary>
/// Klasa reprezentująca informacje o aktualizacji dzielnicy przez opiekuna/administratora.
/// </summary>
public class ChangeInfo
{
    /// <summary>
    /// Wiadomość informacyjna.
    /// </summary>
    public string InfoMessage { get; set; }
        
    /// <summary>
    /// Identyfikator dzielnicy.
    /// </summary>
    public int? DistrictId { get; set; }
}