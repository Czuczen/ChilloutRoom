using System.ComponentModel;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.General.GeneratedType))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.GeneratedType))]
public class GeneratedTypeUpdateDto : EntityDto<int>
{
    [FieldIsRequired]
    [DisplayName(GeneratedTypeFieldsHrNames.Name)]
    public string Name { get; set; }
}