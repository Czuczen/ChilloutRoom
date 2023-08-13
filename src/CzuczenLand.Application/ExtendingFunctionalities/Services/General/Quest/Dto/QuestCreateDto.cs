using System;
using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia zadania.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.Quest))]
public class QuestCreateDto : PartMessagesCreateDto, IGeneratedEntity
{
    /// <summary>
    /// Czas trwania zadania.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.Duration)]
    public decimal? Duration { get; set; }

    /// <summary>
    /// Czy jest dostępne od początku.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(QuestFieldsHrNames.IsAvailableInitially)]
    public bool IsAvailableInitially { get; set; }
        
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

    /// <summary>
    /// Identyfikator typu generowanego.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.GeneratedType)]
    public int GeneratedTypeId { get; set; }
}