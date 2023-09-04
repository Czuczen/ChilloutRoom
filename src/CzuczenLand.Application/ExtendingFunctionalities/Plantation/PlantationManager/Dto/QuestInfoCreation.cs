using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

/// <summary>
/// Reprezentuje tworzenie informacji o zadaniach.
/// </summary>
public class QuestInfoCreation
{
    /// <summary>
    /// Lista definicji zadań.
    /// </summary>
    public List<Quest> QuestDefinitions { get; set; }
        
    /// <summary>
    /// Lista relacji między zadaniami a nagrodami.
    /// </summary>
    public List<DropQuest> DropQuestRelations { get; set; }
        
    /// <summary>
    /// Lista nagród.
    /// </summary>
    public List<Drop> Drops { get; set; }
        
    /// <summary>
    /// Lista wymagań.
    /// </summary>
    public List<Requirement> Requirements { get; set; }
        
    /// <summary>
    /// Lista postępów wymagań w zadaniach.
    /// </summary>
    public List<QuestRequirementsProgress> QuestsRequirementsProgress { get; set; }
}