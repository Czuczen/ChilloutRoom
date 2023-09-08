using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdatePlayerRecord;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;

/// <summary>
/// Reprezentuje DTO służące do określenia jakie pola mają być aktualizowane w produkcie typu "Susz", który jest powiązany z graczem.
/// </summary>
public abstract class DriedFruitUpdatePlayerRecordDto : ProductUpdatePlayerRecordDto
{
    /// <summary>
    /// Szansa na złożenie oferty w strefie klienta.
    /// </summary>
    [DisplayName(DriedFruitFieldsHrNames.OfferChance)]
    public int OfferChance { get; set; }

    /// <summary>
    /// Określa czy susz jest dostępny w strefie klienta.
    /// </summary>
    [DisplayName(DriedFruitFieldsHrNames.AvailableInCustomerZone)]
    public bool AvailableInCustomerZone { get; set; }
        
    /// <summary>
    /// Cena suszu w strefie klienta.
    /// </summary>
    [DisplayName(DriedFruitFieldsHrNames.CustomerZonePrice)]
    public decimal CustomerZonePrice { get; set; }
}