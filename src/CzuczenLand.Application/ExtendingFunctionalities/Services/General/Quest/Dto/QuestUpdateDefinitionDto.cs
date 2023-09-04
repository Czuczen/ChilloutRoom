using System;
using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

/// <summary>
/// Reprezentuje DTO służące do określenia jakie pola mają być aktualizowane w zadaniu, które jest powiązane z graczem.
/// </summary>
public class QuestUpdateDefinitionDto : PartMessagesUpdateDefinitionDto
{
    /// <summary>
    /// Czas trwania zadania.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.Duration)]
    public decimal? Duration { get; set; }

    /// <summary>
    /// Czy zadanie jest powtarzalne.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.IsRepetitive)]
    public bool IsRepetitive { get; set; }

    /// <summary>
    /// Czas cyklu (dla zadań cyklicznych).
    /// </summary>
    [DisplayName(QuestFieldsHrNames.CyclicTime)]
    public int? CyclicTime { get; set; }
        
    /// <summary>
    /// Wymagany poziom plantacji.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.PlantationLevelRequirement)]
    public int? PlantationLevelRequirement { get; set; }
     
    /// <summary>
    /// Typ zadania.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.QuestType)]
    public string QuestType { get; set; }
        
    /// <summary>
    /// Data rozpoczęcia zadania.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.StartTime)]
    public DateTime? StartTime { get; set; }
        
    /// <summary>
    /// Data zakończenia zadania.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.EndTime)]
    public DateTime? EndTime { get; set; }
}