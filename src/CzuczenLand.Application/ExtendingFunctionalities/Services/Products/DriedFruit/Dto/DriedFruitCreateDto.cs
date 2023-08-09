using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia produktu typu "Susz".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.DriedFruit))]
public class DriedFruitCreateDto : ProductCreateDto
{
    /// <summary>
    /// Szansa na złożenie oferty w strefie klienta.
    /// </summary>
    [DisplayName(DriedFruitFieldsHrNames.OfferChance)]
    public int? OfferChance { get; set; }

    /// <summary>
    /// Określa czy susz jest dostępny w strefie klienta.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DriedFruitFieldsHrNames.AvailableInCustomerZone)]
    public bool AvailableInCustomerZone { get; set; }

    /// <summary>
    /// Cena suszu w strefie klienta.
    /// </summary>
    [DisplayName(DriedFruitFieldsHrNames.CustomerZonePrice)]
    public decimal? CustomerZonePrice { get; set; }
}