using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.General.Requirement))]
public class RequirementCreateDto
{
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Name)]
    public string Name { get; set; }
    
    // TODO: SPZ - Wrócić do tego jak będzie tworzony system pól zależnych. [FieldDependsOn("CustomEntityName", "HasValue")] pokaż, wymagaj, ukryj, wymagaj wartosci, zamysł na zależności pól // https://stackoverflow.com/questions/3713281/attribute-dependent-on-another-field
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
        
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.District)]
    public int DistrictId { get; set; }
}