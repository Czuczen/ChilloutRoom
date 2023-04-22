using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.General.Quest))]
public class QuestDto : PartMessagesDto, IPlantationStorageResource, IGeneratedEntity
{
    [DisplayName(QuestFieldsHrNames.Duration)]
    public decimal? Duration { get; set; }
        
    [DisplayName(QuestFieldsHrNames.CurrentDuration)]
    public decimal CurrentDuration { get; set; }
        
    [DisplayName(QuestFieldsHrNames.IsAvailableInitially)]
    public bool IsAvailableInitially { get; set; }
        
    [DisplayName(QuestFieldsHrNames.IsRepetitive)]
    public bool IsRepetitive { get; set; }
        
    [DisplayName(QuestFieldsHrNames.IsComplete)]
    public bool IsComplete { get; set; }
        
    [DisplayName(QuestFieldsHrNames.WorkerSent)]
    public bool WorkerSent { get; set; }
        
    [DisplayName(QuestFieldsHrNames.CyclicTime)]
    public int? CyclicTime { get; set; }
        
    [DisplayName(QuestFieldsHrNames.CompletedAmount)]
    public int CompletedAmount { get; set; }
        
    [DisplayName(QuestFieldsHrNames.PlantationLevelRequirement)]
    public int? PlantationLevelRequirement { get; set; }

    [DisplayName(QuestFieldsHrNames.QuestType)]
    public string QuestType { get; set; }
        
    [DisplayName(QuestFieldsHrNames.StartTime)]
    public DateTime? StartTime { get; set; }
        
    [DisplayName(QuestFieldsHrNames.EndTime)]
    public DateTime? EndTime { get; set; }

    [DisplayName(QuestFieldsHrNames.InProgress)]
    public bool InProgress { get; set; }
        
    [DisplayName(EntitiesHrNames.GeneratedType), Display(GroupName = EntitiesDbNames.GeneratedType)]
    public int GeneratedTypeId { get; set; }
        
    [DisplayName(EntitiesHrNames.PlantationStorage), Display(GroupName = EntitiesDbNames.PlantationStorage)]
    public int? PlantationStorageId { get; set; }
        
    [DisplayName(EntitiesHrNames.PlayerStorage), Display(GroupName = EntitiesDbNames.PlayerStorage)]
    public int? PlayerStorageId { get; set; }
}