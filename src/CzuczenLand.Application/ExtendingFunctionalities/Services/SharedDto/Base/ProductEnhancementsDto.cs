using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

public class ProductEnhancementsDto : ProductDto
{
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGrowingSpeed)]
    public decimal IncreaseGrowingSpeed { get; set; }

    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseTimeOfInsensitivity)]
    public int IncreaseTimeOfInsensitivity { get; set; }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseChanceForSeed)]
    public int IncreaseChanceForSeed { get; set; }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseDriedFruitAmount)]
    public decimal IncreaseDriedFruitAmount { get; set; }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseSeedAmount)]
    public int IncreaseSeedAmount { get; set; }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGainedExp)]
    public decimal IncreaseGainedExp { get; set; }
        
    [DisplayName(ProductFieldsHrNames.SetName)]
    public string SetName { get; set; }
}