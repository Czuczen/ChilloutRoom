using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

/// <summary>
/// Reprezentuje DTO dla produktu typu "Woda".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Water))]
public class WaterDto : ProductEnhancementsDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [DisplayName(WaterFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}