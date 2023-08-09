using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

/// <summary>
/// Klasa reprezentująca dane transferowe (DTO) do tworzenia definicji ulepszeń produktu, dziedzicząca po ProductCreateDto.
/// </summary>
public class ProductEnhancementsCreateDto : ProductCreateDto
{
    /// <summary>
    /// Zwiększenie prędkości wzrostu.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGrowingSpeed)]
    public decimal IncreaseGrowingSpeed { get; set; }

    /// <summary>
    /// Zwiększenie czasu niewrażliwości.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseTimeOfInsensitivity)]
    public int IncreaseTimeOfInsensitivity { get; set; }
        
    /// <summary>
    /// Zwiększenie szansy na nasiona.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseChanceForSeed)]
    public int IncreaseChanceForSeed { get; set; }
        
    /// <summary>
    /// Zwiększenie ilości suszu.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseDriedFruitAmount)]
    public decimal IncreaseDriedFruitAmount { get; set; }
       
    /// <summary>
    /// Zwiększenie ilości nasion.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseSeedAmount)]
    public int IncreaseSeedAmount { get; set; }
        
    /// <summary>
    /// Zwiększenie zdobywanego doświadczenia.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGainedExp)]
    public decimal IncreaseGainedExp { get; set; }
        
    /// <summary>
    /// Nazwa zestawu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.SetName)]
    public string SetName { get; set; }
}