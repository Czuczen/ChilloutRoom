using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.Water))]
public class WaterCreateDto : ProductEnhancementsCreateDto
{
    [FieldIsRequired]
    [DisplayName(WaterFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}