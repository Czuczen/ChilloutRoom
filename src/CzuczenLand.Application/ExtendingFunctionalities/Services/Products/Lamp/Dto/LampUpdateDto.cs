using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.Lamp))]
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Lamp))]
public class LampUpdateDto : ProductEnhancementsUpdateDto
{
    [FieldIsRequired]
    [DisplayName(LampFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}