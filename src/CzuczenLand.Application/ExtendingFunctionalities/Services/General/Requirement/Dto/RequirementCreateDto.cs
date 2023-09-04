using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia wymagania.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.Requirement))]
public class RequirementCreateDto
{
    /// <summary>
    /// Nazwa wymagania.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(RequirementFieldsHrNames.Name)]
    public string Name { get; set; }
    
    // TODO: SPZ - Wrócić do tego jak będzie tworzony system pól zależnych. [FieldDependsOn("CustomEntityName", "HasValue")] pokaż, wymagaj, ukryj, wymagaj wartosci, zamysł na zależności pól // https://stackoverflow.com/questions/3713281/attribute-dependent-on-another-field
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
        
    /// <summary>
    /// Identyfikator dzielnicy.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.District)]
    public int DistrictId { get; set; }
}