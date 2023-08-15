namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca aktualizację bonusu.
/// </summary>
public class BonusUpdate
{
    /// <summary>
    /// Określa, czy bonus jest artefaktem.
    /// </summary>
    public bool IsArtifact { get; set; }
        
    /// <summary>
    /// Identyfikator bonusu.
    /// </summary>
    public int BonusId { get; set; }

    /// <summary>
    /// Pozostały aktywny czas bonusu.
    /// </summary>
    public int RemainingActiveTime { get; set; }
        
    /// <summary>
    /// Określa, czy bonus jest aktywny.
    /// </summary>
    public bool IsActive { get; set; }
}