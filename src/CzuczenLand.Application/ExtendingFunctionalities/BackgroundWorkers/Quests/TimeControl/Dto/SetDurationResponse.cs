namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Quests.TimeControl.Dto;

/// <summary>
/// Klasa reprezentująca odpowiedź na ustawienie czasu trwania zadania.
/// </summary>
public class SetDurationResponse
{
    /// <summary>
    /// Identyfikator zadania.
    /// </summary>
    public int QuestId { get; set; }
        
    /// <summary>
    /// Pozostały czas trwania zadania w formie tekstowej.
    /// </summary>
    public string RemainingTime { get; set; }

    /// <summary>
    /// Wartość logiczna wskazująca, czy czas zadania minął.
    /// </summary>
    public bool TimeUp { get; set; }
}