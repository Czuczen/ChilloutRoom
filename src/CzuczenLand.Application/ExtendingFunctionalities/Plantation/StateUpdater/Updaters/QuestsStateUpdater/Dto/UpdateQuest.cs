using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.QuestsStateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca aktualizację zadania u gracza.
/// </summary>
public class UpdateQuest
{
    /// <summary>
    /// Identyfikator zadania.
    /// </summary>
    public int QuestId { get; set; }
        
    /// <summary>
    /// Określa, czy zadanie jest w trakcie wykonywania.
    /// </summary>
    public bool InProgress { get; set; }
        
    /// <summary>
    /// Określa, czy zadanie zostało ukończone.
    /// </summary>
    public bool QuestIsComplete { get; set; }

    /// <summary>
    /// Lista wymagań zadania do aktualizacji.
    /// </summary>
    public List<UpdateRequirement> UpdatingRequirements { get; } = new();
}