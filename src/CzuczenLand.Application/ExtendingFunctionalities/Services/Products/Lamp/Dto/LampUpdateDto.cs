using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w definicji produktu typu "Lampa".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.Lamp))]
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Lamp))]
public class LampUpdateDto : ProductEnhancementsUpdateDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(LampFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}