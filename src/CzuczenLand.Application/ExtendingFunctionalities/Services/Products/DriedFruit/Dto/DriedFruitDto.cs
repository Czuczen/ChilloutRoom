using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;

/// <summary>
/// Reprezentuje DTO dla produktu typu "Susz".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Products.DriedFruit))]
public class DriedFruitDto : ProductDto
{
    /// <summary>
    /// Szansa na złożenie oferty w strefie klienta.
    /// </summary>
    [DisplayName(DriedFruitFieldsHrNames.OfferChance)]
    public int? OfferChance { get; set; }

    /// <summary>
    /// Określa czy susz jest dostępny w strefie klienta.
    /// </summary>
    [DisplayName(DriedFruitFieldsHrNames.AvailableInCustomerZone)]
    public bool AvailableInCustomerZone { get; set; }

    /// <summary>
    /// Cena suszu w strefie klienta.
    /// </summary>
    [DisplayName(DriedFruitFieldsHrNames.CustomerZonePrice)]
    public decimal? CustomerZonePrice { get; set; }
}