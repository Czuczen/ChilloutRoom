namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca aktualizację informacji magazynu gracza.
/// </summary>
public class PlayerStorageUpdate
{
    /// <summary>
    /// Poziom gracza.
    /// </summary>
    public string Level { get; set; }
    
    /// <summary>
    /// Punkty honoru gracza.
    /// </summary>
    public string Honor { get; set; }

    /// <summary>
    /// Uzyskane doświadczenie gracza.
    /// </summary>
    public string GainedExperience { get; set; }
        
    /// <summary>
    /// Ilość złota gracza.
    /// </summary>
    public string Gold { get; set; }
}