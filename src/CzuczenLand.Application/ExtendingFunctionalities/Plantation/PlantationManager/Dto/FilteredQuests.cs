using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

/// <summary>
/// Reprezentuje zestawienie przefiltrowanych zadań.
/// </summary>
public class FilteredQuests
{
    /// <summary>
    /// Zbiór danych do utworzenia informacji o zadaniu.
    /// </summary>
    public QuestInfoCreation QuestInfoCreation { get; set; }
    
    /// <summary>
    /// Lista zadań.
    /// </summary>
    public List<Quest> Quests { get; set; }
}