using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

[AutoMapFrom(typeof(Lamp))]
public class LampInfoViewModel : ProductInfoViewModel
{
    private decimal _capacityInPotRequirement;
        
        
    [DisplayName(LampFieldsHrNames.CapacityInPotRequirement)]
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
}