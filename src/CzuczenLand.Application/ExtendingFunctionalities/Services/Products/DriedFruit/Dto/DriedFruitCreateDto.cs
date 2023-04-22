using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.DriedFruit))]
public class DriedFruitCreateDto : ProductCreateDto
{
    [DisplayName(DriedFruitFieldsHrNames.OfferChance)]
    public int? OfferChance { get; set; }

    [FieldIsRequired]
    [DisplayName(DriedFruitFieldsHrNames.AvailableInCustomerZone)]
    public bool AvailableInCustomerZone { get; set; }

    [DisplayName(DriedFruitFieldsHrNames.CustomerZonePrice)]
    public decimal? CustomerZonePrice { get; set; }
}