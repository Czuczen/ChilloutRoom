namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.QuestsStateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca wymaganie zadania do aktualizacji.
/// </summary>
public class UpdateRequirement
{
    /// <summary>
    /// Identyfikator wymagania.
    /// </summary>
    public int RequirementId { get; set; }

    /// <summary>
    /// Postęp wymagania jako tekst.
    /// </summary>
    public string RequirementProgressText { get; set; }

    /// <summary>
    /// Procentowy postęp wymagania.
    /// </summary>
    public string RequirementProgressPercentage { get; set; }
}