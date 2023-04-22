using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.General.GeneratedType))]
public class GeneratedTypeCreateDto
{
    [FieldIsRequired]
    [DisplayName(GeneratedTypeFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [FieldIsRequired]
    [DisplayName(GeneratedTypeFieldsHrNames.EntityName)]
    public string EntityName { get; set; }
        
        
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.District)]
    public int DistrictId { get; set; }
}