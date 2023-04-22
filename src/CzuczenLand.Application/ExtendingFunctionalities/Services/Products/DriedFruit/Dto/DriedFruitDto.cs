using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.Products.DriedFruit))]
public class DriedFruitDto : ProductDto
{
    [DisplayName(DriedFruitFieldsHrNames.OfferChance)]
    public int? OfferChance { get; set; }

    [DisplayName(DriedFruitFieldsHrNames.AvailableInCustomerZone)]
    public bool AvailableInCustomerZone { get; set; }

    [DisplayName(DriedFruitFieldsHrNames.CustomerZonePrice)]
    public decimal? CustomerZonePrice { get; set; }
}