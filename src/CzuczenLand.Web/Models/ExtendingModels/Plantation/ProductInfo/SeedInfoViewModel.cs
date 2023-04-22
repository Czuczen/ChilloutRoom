using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

[AutoMapFrom(typeof(Seed))]
public class SeedInfoViewModel : ProductInfoViewModel
{
    private decimal _capacityInPotRequirement;
    private decimal _manureConsumption;
    private decimal _waterConsumption;
        

    [DisplayName(SeedFieldsHrNames.Description)]
    public string Description { get; set; }
        
    [DisplayName(SeedFieldsHrNames.CapacityInPotRequirement)]
    public string CapacityInPotRequirement
    {
        get => _capacityInPotRequirement + PlantationManagerHelper.GetMeasureUnitByType(typeof(Water));
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _capacityInPotRequirement = parsedValue;
            }
        }
    }
        
    [DisplayName(SeedFieldsHrNames.ManureConsumption)]
    public string ManureConsumption
    {
        get => _manureConsumption + PlantationManagerHelper.GetMeasureUnitByType(typeof(Manure));
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _manureConsumption = parsedValue;
            }
        }
    }
        
    [DisplayName(SeedFieldsHrNames.WaterConsumption)]
    public string WaterConsumption
    {
        get => _waterConsumption + PlantationManagerHelper.GetMeasureUnitByType(typeof(Water));
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _waterConsumption = parsedValue;
            }
        }
    }
}