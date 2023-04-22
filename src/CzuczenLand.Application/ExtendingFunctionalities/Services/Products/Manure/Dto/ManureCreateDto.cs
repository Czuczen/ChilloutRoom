using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.Manure))]
public class ManureCreateDto : ProductEnhancementsCreateDto
{
    [FieldIsRequired]
    [DisplayName(ManureFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}