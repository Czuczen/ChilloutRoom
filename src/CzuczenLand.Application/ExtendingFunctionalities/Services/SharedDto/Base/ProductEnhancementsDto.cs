using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

/// <summary>
/// Klasa reprezentująca dane transferowe (DTO) ulepszeń produktu, dziedzicząca po ProductDto.
/// </summary>
public class ProductEnhancementsDto : ProductDto
{
    /// <summary>
    /// Zwiększenie prędkości wzrostu.
    /// </summary>
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGrowingSpeed)]
    public decimal IncreaseGrowingSpeed { get; set; }

    /// <summary>
    /// Zwiększenie czasu niewrażliwości.
    /// </summary>
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseTimeOfInsensitivity)]
    public int IncreaseTimeOfInsensitivity { get; set; }
        
    /// <summary>
    /// Zwiększenie szansy na nasiona.
    /// </summary>
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseChanceForSeed)]
    public int IncreaseChanceForSeed { get; set; }
        
    /// <summary>
    /// Zwiększenie ilości suszu.
    /// </summary>
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseDriedFruitAmount)]
    public decimal IncreaseDriedFruitAmount { get; set; }
        
    /// <summary>
    /// Zwiększenie ilości nasion.
    /// </summary>
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseSeedAmount)]
    public int IncreaseSeedAmount { get; set; }
        
    /// <summary>
    /// Zwiększenie zdobywanego doświadczenia.
    /// </summary>
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGainedExp)]
    public decimal IncreaseGainedExp { get; set; }
        
    /// <summary>
    /// Nazwa zestawu.
    /// </summary>
    [DisplayName(ProductFieldsHrNames.SetName)]
    public string SetName { get; set; }
}