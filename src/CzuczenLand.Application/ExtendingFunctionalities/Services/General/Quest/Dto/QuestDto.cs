using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

/// <summary>
/// Reprezentuje DTO dla zadania.
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.General.Quest))]
public class QuestDto : PartMessagesDto, IPlantationStorageResource, IGeneratedEntity
{
    /// <summary>
    /// Czas trwania zadania.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.Duration)]
    public decimal? Duration { get; set; }
        
    /// <summary>
    /// Aktualny czas trwania zadania.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.CurrentDuration)]
    public decimal CurrentDuration { get; set; }
        
    /// <summary>
    /// Czy jest dostępne od początku.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.IsAvailableInitially)]
    public bool IsAvailableInitially { get; set; }
        
    /// <summary>
    /// Czy zadanie jest powtarzalne.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.IsRepetitive)]
    public bool IsRepetitive { get; set; }
        
    /// <summary>
    /// Czy zadanie jest ukończone.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.IsComplete)]
    public bool IsComplete { get; set; }

    /// <summary>
    /// Czy pracownik zadań ograniczonych czasowo wysłał je użytkownikowi.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.WorkerSent)]
    public bool WorkerSent { get; set; }
        
    /// <summary>
    /// Czas cyklu (dla zadań cyklicznych).
    /// </summary>
    [DisplayName(QuestFieldsHrNames.CyclicTime)]
    public int? CyclicTime { get; set; }
        
    /// <summary>
    /// Ilość ukończonych zadań.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.CompletedAmount)]
    public int CompletedAmount { get; set; }
        
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

    /// <summary>
    /// Czy zadanie jest w trakcie.
    /// </summary>
    [DisplayName(QuestFieldsHrNames.InProgress)]
    public bool InProgress { get; set; }

    /// <summary>
    /// Identyfikator typu generowanego.
    /// </summary>
    [DisplayName(EntitiesHrNames.GeneratedType), Display(GroupName = EntitiesDbNames.GeneratedType)]
    public int GeneratedTypeId { get; set; }

    /// <summary>
    /// Identyfikator magazynu plantacji.
    /// </summary>
    [DisplayName(EntitiesHrNames.PlantationStorage), Display(GroupName = EntitiesDbNames.PlantationStorage)]
    public int? PlantationStorageId { get; set; }
        
    /// <summary>
    /// Identyfikator magazynu gracza.
    /// </summary>
    [DisplayName(EntitiesHrNames.PlayerStorage), Display(GroupName = EntitiesDbNames.PlayerStorage)]
    public int? PlayerStorageId { get; set; }
}