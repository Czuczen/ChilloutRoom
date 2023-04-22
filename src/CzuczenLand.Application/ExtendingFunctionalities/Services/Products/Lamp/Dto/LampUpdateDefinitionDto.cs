using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

[AutoMapFrom(typeof(LampUpdateDto))]
public class LampUpdateDefinitionDto : ProductEnhancementsUpdateDefinitionDto
{
    [DisplayName(LampFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}