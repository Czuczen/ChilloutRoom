using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;

public class SoilUpdateDefinitionDto : ProductEnhancementsUpdateDefinitionDto
{
    [DisplayName(SoilFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
        
    [DisplayName(SoilFieldsHrNames.SoilClass)]
    public int SoilClass { get; set; }
}