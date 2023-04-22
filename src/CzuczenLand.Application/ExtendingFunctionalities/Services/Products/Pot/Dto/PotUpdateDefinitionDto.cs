using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;

public class PotUpdateDefinitionDto : ProductEnhancementsUpdateDefinitionDto
{
    [DisplayName(PotFieldsHrNames.MaxRangeOfSoilClass)]
    public int MaxRangeOfSoilClass { get; set; }
        
    [DisplayName(PotFieldsHrNames.Capacity)]
    public decimal Capacity { get; set; }
}