using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia produktu typu "Lampa".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.Lamp))]
public class LampCreateDto : ProductEnhancementsCreateDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(LampFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}