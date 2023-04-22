using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

[AutoMapFrom(typeof(DriedFruit))]
public class DriedFruitInfoViewModel : ProductInfoViewModel
{
    private decimal _customerZonePrice;

        
    [DisplayName(DriedFruitFieldsHrNames.AvailableInCustomerZone)]
    public bool AvailableInCustomerZone { get; set; }
        
    [DisplayName(DriedFruitFieldsHrNames.CustomerZonePrice)]
    public string CustomerZonePrice
    {
        get
        {
            var ret = (AvailableInCustomerZone ? _customerZonePrice : 0) + "$";

            return ret;
        }
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _customerZonePrice = parsedValue;    
            }
        }
    }
}