using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;

/// <summary>
/// Reprezentuje DTO dla produktu typu "Gleba".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Soil))]
public class SoilDto : ProductEnhancementsDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [DisplayName(SoilFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
        
    /// <summary>
    /// Klasa gleby.
    /// </summary>
    [DisplayName(SoilFieldsHrNames.SoilClass)]
    public int SoilClass { get; set; }
}