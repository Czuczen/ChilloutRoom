using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

public class ProductEnhancementsCreateDto : ProductCreateDto
{
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGrowingSpeed)]
    public decimal IncreaseGrowingSpeed { get; set; }

    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseTimeOfInsensitivity)]
    public int IncreaseTimeOfInsensitivity { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseChanceForSeed)]
    public int IncreaseChanceForSeed { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseDriedFruitAmount)]
    public decimal IncreaseDriedFruitAmount { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseSeedAmount)]
    public int IncreaseSeedAmount { get; set; }
        
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGainedExp)]
    public decimal IncreaseGainedExp { get; set; }
        
    [DisplayName(ProductFieldsHrNames.SetName)]
    public string SetName { get; set; }
}