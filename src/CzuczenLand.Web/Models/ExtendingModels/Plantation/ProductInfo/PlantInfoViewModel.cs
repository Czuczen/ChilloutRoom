using System.ComponentModel;
using System.Globalization;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

[AutoMapFrom(typeof(CzuczenLand.ExtendingModels.Models.General.Plant))]
public class PlantInfoViewModel
{
    private int _setsBaf;
    private decimal _growingSpeed;
    private decimal _wiltSpeed;
    private int _chanceForSeed;
    private decimal _driedFruitAmount;
    private int _seedAmount;
    private decimal _gainedExp;
        
        
    public int Id { get; set; }
        
    [DisplayName(PlantFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [DisplayName(PlantFieldsHrNames.GrowingLevel)]
    public decimal GrowingLevel { get; set; }

    [DisplayName(PlantFieldsHrNames.WiltLevel)]
    public decimal WiltLevel { get; set; }
        
    [DisplayName(PlantFieldsHrNames.TimeOfInsensitivity)]
    public int TimeOfInsensitivity { get; set; }
        
    [DisplayName(PlantFieldsHrNames.TimeRemaining)]
    public int TimeRemaining { get; set; }
        
    [DisplayName(PlantFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    [DisplayName(PlantFieldsHrNames.GrowingSpeedDivider)]
    public decimal GrowingSpeedDivider { private get; set; }
        
    [DisplayName(PlantFieldsHrNames.Description)]
    public string Description { get; set; }
        
    [DisplayName(PlantFieldsHrNames.SetsBaf)]
    public string SetsBaf
    {
        get => (_setsBaf - 1) + "x";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _setsBaf = parsedValue;
            }
        }
    }

    [DisplayName(PlantFieldsHrNames.GrowingSpeed)]
    public string GrowingSpeed
    {
        get => (_growingSpeed * _setsBaf / GrowingSpeedDivider).ToString(CultureInfo.CurrentCulture) + "/sec";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _growingSpeed = parsedValue;
            }
        }
    }

    [DisplayName(PlantFieldsHrNames.WiltSpeed)]
    public string WiltSpeed
    {
        get => _wiltSpeed.ToString(CultureInfo.CurrentCulture) + "/sec";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _wiltSpeed = parsedValue;
            }
        }
    }
    
    [DisplayName(PlantFieldsHrNames.ChanceForSeed)]
    public string ChanceForSeed
    {
        get => _chanceForSeed + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _chanceForSeed = parsedValue;
            }
        }
    }

    [DisplayName(PlantFieldsHrNames.DriedFruitAmount)]
    public string DriedFruitAmount
    {
        get => _driedFruitAmount + PlantationManagerHelper.GetMeasureUnitByType(typeof(DriedFruit));
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _driedFruitAmount = parsedValue;
            }
        }
    }

    [DisplayName(PlantFieldsHrNames.SeedAmount)]
    public string SeedAmount
    {
        get => _seedAmount + PlantationManagerHelper.GetMeasureUnitByType(typeof(Seed));
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _seedAmount = parsedValue;
            }
        }
    }

    [DisplayName(PlantFieldsHrNames.GainedExp)]
    public string GainedExp
    {
        get => _gainedExp + "pkt";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _gainedExp = parsedValue;
            }
        }
    }
}