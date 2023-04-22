using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

public class WaterUpdateDefinitionDto : ProductEnhancementsUpdateDefinitionDto
{
    [DisplayName(WaterFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}