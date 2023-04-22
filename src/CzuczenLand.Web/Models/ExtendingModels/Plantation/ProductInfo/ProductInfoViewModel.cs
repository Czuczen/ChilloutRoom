using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Models.Products;
using static CzuczenLand.ExtendingFunctionalities.PlantationManager.PlantationManagerHelper;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

public class ProductInfoViewModel
{
    private decimal _increaseGrowingSpeed;
    private int _increaseTimeOfInsensitivity;
    private int _increaseChanceForSeed;
    private decimal _increaseDriedFruitAmount;
    private int _increaseSeedAmount;
    private decimal _increaseGainedExp;
    private string _setName;
        
        
    [DisplayName(ProductFieldsHrNames.Name)]
    public string Name { get; set; }

    [DisplayName(ProductFieldsHrNames.PlantationLevelRequirement)]
    public int PlantationLevelRequirement { get; set; }
        
    [DisplayName(ProductFieldsHrNames.SetName)]
    public string SetName
    {
        get => string.IsNullOrWhiteSpace(_setName) ? "Brak" : _setName;
        set => _setName = value;
    }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGrowingSpeed)]
    public string IncreaseGrowingSpeed
    {
        get => _increaseGrowingSpeed + "pkt"; 
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _increaseGrowingSpeed = parsedValue;
            }
        }
    }

    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseTimeOfInsensitivity)]
    public string IncreaseTimeOfInsensitivity
    {
        get => _increaseTimeOfInsensitivity + "sec"; 
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _increaseTimeOfInsensitivity = parsedValue;
            }
        }
    }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseChanceForSeed)]
    public string IncreaseChanceForSeed
    {
        get => _increaseChanceForSeed + "%"; 
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _increaseChanceForSeed = parsedValue;
            }
        }
    }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseDriedFruitAmount)]
    public string IncreaseDriedFruitAmount
    {
        get => _increaseDriedFruitAmount + GetMeasureUnitByType(typeof(DriedFruit));
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _increaseDriedFruitAmount = parsedValue;
            }
        }
    }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseSeedAmount)]
    public string IncreaseSeedAmount
    {
        get => _increaseSeedAmount + GetMeasureUnitByType(typeof(Seed));
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _increaseSeedAmount = parsedValue;
            }
        }
    }
        
    [DisplayName(ProductEnhancmentFieldsHrNames.IncreaseGainedExp)]
    public string IncreaseGainedExp
    {
        get => _increaseGainedExp + "pkt"; 
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _increaseGainedExp = parsedValue;
            }
        }
    }

}