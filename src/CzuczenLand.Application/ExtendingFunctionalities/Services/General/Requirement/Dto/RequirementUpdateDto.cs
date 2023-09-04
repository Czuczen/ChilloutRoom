using System.ComponentModel;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w definicji wymagania.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.Requirement))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.Requirement))]
public class RequirementUpdateDto : EntityDto<int>
{
    /// <summary>
    /// Nazwa wymagania.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Name)]
    public string Name { get; set; }
        
    /// <summary>
    /// Nazwa niestandardowej encji.
    /// </summary>
    [DisplayName(RequirementFieldsHrNames.CustomEntityName)]
    public string CustomEntityName { get; set; }
    
    /// <summary>
    /// Identyfikator typu generowanego.
    /// </summary>
    [DisplayName(EntitiesHrNames.GeneratedType)]
    public int? GeneratedTypeId { get; set; }

    /// <summary>
    /// Warunek wymagania.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Condition)]
    public string Condition { get; set; }
        
    /// <summary>
    /// Nazwa pola encji do porównania.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Comparer)]
    public string Comparer { get; set; }

    /// <summary>
    /// Wymagana ilość.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Amount)]
    public decimal Amount { get; set; }
}