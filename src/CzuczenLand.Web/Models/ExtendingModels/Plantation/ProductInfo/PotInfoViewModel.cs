using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

[AutoMapFrom(typeof(Pot))]
public class PotInfoViewModel : ProductInfoViewModel
{
    private decimal _capacity;
        
        
    [DisplayName(PotFieldsHrNames.MaxRangeOfSoilClass)]
    public int MaxRangeOfSoilClass { get; set; }
        
    [DisplayName(PotFieldsHrNames.Capacity)]
    public string Capacity
    {
        get => _capacity + PlantationManagerHelper.GetMeasureUnitByType(typeof(Water));
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _capacity = parsedValue;
            }
        }
    }
}