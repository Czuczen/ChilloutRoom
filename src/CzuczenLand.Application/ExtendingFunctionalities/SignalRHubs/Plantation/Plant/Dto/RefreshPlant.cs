namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Plant.Dto;

/// <summary>
/// Klasa przechowująca informacje o aktualnym stanie odświeżonej rośliny.
/// </summary>
public class RefreshPlant
{
    /// <summary>
    /// Identyfikator rośliny.
    /// </summary>
    public int PlantId { get; set; }
        
    /// <summary>
    /// Pozostały czas do zbiorów.
    /// </summary>
    public int TimeRemaining { get; set; }
        
    /// <summary>
    /// Czas do więdnięcia.
    /// </summary>
    public int TimeOfInsensitivity { get; set; }
        
    /// <summary>
    /// Aktualny poziom wzrostu rośliny.
    /// </summary>
    public decimal GrowingLevel { get; set; }

    /// <summary>
    /// Poziom zwiędnięcia.
    /// </summary>
    public decimal WiltLevel { get; set; }
}