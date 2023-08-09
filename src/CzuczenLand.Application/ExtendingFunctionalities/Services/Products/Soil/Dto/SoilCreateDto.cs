using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia produktu typu "Gleba".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.Soil))]
public class SoilCreateDto : ProductEnhancementsCreateDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(SoilFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
        
    /// <summary>
    /// Klasa gleby.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(SoilFieldsHrNames.SoilClass)]
    public int SoilClass { get; set; }
}