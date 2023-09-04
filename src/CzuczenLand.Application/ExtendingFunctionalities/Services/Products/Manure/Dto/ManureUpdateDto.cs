using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w definicji produktu typu "Nawóz".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.Manure))]
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Manure))]
public class ManureUpdateDto : ProductEnhancementsUpdateDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ManureFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}