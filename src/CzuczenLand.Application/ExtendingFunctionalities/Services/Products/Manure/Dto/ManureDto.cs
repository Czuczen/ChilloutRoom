using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;

/// <summary>
/// Reprezentuje DTO dla produktu typu "Nawóz".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Manure))]
public class ManureDto : ProductEnhancementsDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [DisplayName(ManureFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}