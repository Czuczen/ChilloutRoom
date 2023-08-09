using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

/// <summary>
/// Reprezentuje DTO dla produktu typu "Lampa".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Lamp))]
public class LampDto : ProductEnhancementsDto
{
    /// <summary>
    /// Zawiera ilość lamp w użyciu.
    /// </summary>
    [DisplayName(LampFieldsHrNames.InUseCount)]
    public int InUseCount { get; set; }

    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [DisplayName(LampFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}