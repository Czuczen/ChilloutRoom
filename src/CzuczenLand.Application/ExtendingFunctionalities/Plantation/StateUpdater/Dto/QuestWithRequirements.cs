using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca zadanie wraz z wymaganiami.
/// </summary>
public class QuestWithRequirements
{
    /// <summary>
    /// Zadanie.
    /// </summary>
    public Quest Quest { get; set; }

    /// <summary>
    /// Postęp wymagań zadania.
    /// </summary>
    public QuestRequirementsProgress QuestRequirementsProgress { get; set; }

    /// <summary>
    /// Słownik postępu wymagań.
    /// </summary>
    public Dictionary<int, decimal> RequirementsProgress =>
        JsonConvert.DeserializeObject<Dictionary<int, decimal>>(QuestRequirementsProgress.RequirementsProgress);
    
    /// <summary>
    /// Lista wymagań zadania.
    /// </summary>
    public List<Requirement> Requirements { get; } = new();
}