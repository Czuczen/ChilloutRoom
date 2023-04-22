using System;
using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.General.Quest))]
public class QuestCreateDto : PartMessagesCreateDto, IGeneratedEntity
{
    [DisplayName(QuestFieldsHrNames.Duration)]
    public decimal? Duration { get; set; }

    [FieldIsRequired]
    [DisplayName(QuestFieldsHrNames.IsAvailableInitially)]
    public bool IsAvailableInitially { get; set; }
        
    [FieldIsRequired]
    [DisplayName(QuestFieldsHrNames.IsRepetitive)]
    public bool IsRepetitive { get; set; }

    [DisplayName(QuestFieldsHrNames.CyclicTime)]
    public int? CyclicTime { get; set; }
        
    [DisplayName(QuestFieldsHrNames.PlantationLevelRequirement)]
    public int? PlantationLevelRequirement { get; set; }
        
    [FieldIsRequired]
    [DisplayName(QuestFieldsHrNames.QuestType)]
    public string QuestType { get; set; }

    [FieldIsRequired]
    [DisplayName(QuestFieldsHrNames.StartTime)]
    public DateTime? StartTime { get; set; }
        
    [FieldIsRequired]
    [DisplayName(QuestFieldsHrNames.EndTime)]
    public DateTime? EndTime { get; set; }

    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.GeneratedType)]
    public int GeneratedTypeId { get; set; }
}