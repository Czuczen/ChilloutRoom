using System.ComponentModel;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.General.Requirement))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.Requirement))]
public class RequirementUpdateDto : EntityDto<int>
{
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [DisplayName(RequirementFieldsHrNames.CustomEntityName)]
    public string CustomEntityName { get; set; }
    
    [DisplayName(EntitiesHrNames.GeneratedType)]
    public int? GeneratedTypeId { get; set; }

    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Condition)]
    public string Condition { get; set; }
        
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Comparer)]
    public string Comparer { get; set; }
        
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Amount)]
    public decimal Amount { get; set; }
}