namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca dane zadań dla nowo zdobytych poziomów.
/// </summary>
public class LvlQuestsData
{
    /// <summary>
    /// Identyfikator zadania.
    /// </summary>
    public int QuestId { get; set; }
        
    /// <summary>
    /// Typ zadania.
    /// </summary>
    public string QuestType { get; set; }
}