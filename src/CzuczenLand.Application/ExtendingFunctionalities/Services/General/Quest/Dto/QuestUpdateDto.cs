using System;
using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w definicji zadania.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.Quest))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.Quest))]
public class QuestUpdateDto : PartMessagesUpdateDto
{
    /// <summary>
    /// Czas trwania zadania.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.Duration)]
    public decimal? Duration { get; set; }

    /// <summary>
    /// Czy zadanie jest powtarzalne.
    /// </summary>
    [FieldIsRequired]
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
    [FieldIsRequired]
    [DisplayName(QuestFieldsHrNames.QuestType)]
    public string QuestType { get; set; }
        
    /// <summary>
    /// Data rozpoczęcia zadania.
    /// </summary>
    [FieldIsRequired]   
    [DisplayName(QuestFieldsHrNames.StartTime)]
    public DateTime? StartTime { get; set; }
        
    /// <summary>
    /// Data zakończenia zadania.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(QuestFieldsHrNames.EndTime)]
    public DateTime? EndTime { get; set; }
}