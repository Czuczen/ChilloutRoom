using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.General.Requirement))]
public class RequirementDto : EntityDto<int>
{
    [DisplayName(RequirementFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [DisplayName(RequirementFieldsHrNames.CustomEntityName)]
    public string CustomEntityName { get; set; }
        
    [DisplayName(EntitiesHrNames.GeneratedType), Display(GroupName = EntitiesDbNames.GeneratedType)]
    public int? GeneratedTypeId { get; set; }
        
    [DisplayName(RequirementFieldsHrNames.Condition)]
    public string Condition { get; set; }    
        
    [DisplayName(RequirementFieldsHrNames.Comparer)]
    public string Comparer { get; set; }
        
    [DisplayName(RequirementFieldsHrNames.Amount)]
    public decimal Amount { get; set; }
        
    [DisplayName(EntitiesHrNames.District), Display(GroupName = EntitiesDbNames.District)]
    public int DistrictId { get; set; }
}