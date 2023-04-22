using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.Water))]
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Water))]
public class WaterUpdateDto : ProductEnhancementsUpdateDto
{
    [FieldIsRequired]
    [DisplayName(WaterFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}