using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

/// <summary>
/// Reprezentuje DTO dla wymagania.
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.General.Requirement))]
public class RequirementDto : EntityDto<int>
{
    /// <summary>
    /// Nazwa wymagania.
    /// </summary>
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
    [DisplayName(EntitiesHrNames.GeneratedType), Display(GroupName = EntitiesDbNames.GeneratedType)]
    public int? GeneratedTypeId { get; set; }
        
    /// <summary>
    /// Warunek wymagania.
    /// </summary>
    [DisplayName(RequirementFieldsHrNames.Condition)]
    public string Condition { get; set; }    
        
    /// <summary>
    /// Nazwa pola encji do porównania.
    /// </summary>
    [DisplayName(RequirementFieldsHrNames.Comparer)]
    public string Comparer { get; set; }
        
    /// <summary>
    /// Wymagana ilość.
    /// </summary>
    [DisplayName(RequirementFieldsHrNames.Amount)]
    public decimal Amount { get; set; }

    /// <summary>
    /// Identyfikator dzielnicy.
    /// </summary>
    [DisplayName(EntitiesHrNames.District), Display(GroupName = EntitiesDbNames.District)]
    public int DistrictId { get; set; }
}