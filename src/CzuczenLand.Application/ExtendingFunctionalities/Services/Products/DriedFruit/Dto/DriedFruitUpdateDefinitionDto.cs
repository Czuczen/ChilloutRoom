using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;

public class DriedFruitUpdateDefinitionDto : ProductUpdateDefinitionDto
{
    [DisplayName(DriedFruitFieldsHrNames.OfferChance)]
    public int OfferChance { get; set; }

    [DisplayName(DriedFruitFieldsHrNames.AvailableInCustomerZone)]
    public bool AvailableInCustomerZone { get; set; }
        
    [DisplayName(DriedFruitFieldsHrNames.CustomerZonePrice)]
    public decimal CustomerZonePrice { get; set; }
}