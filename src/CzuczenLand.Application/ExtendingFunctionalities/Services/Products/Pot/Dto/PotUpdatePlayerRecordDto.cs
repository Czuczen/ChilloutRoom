using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdatePlayerRecord;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;

/// <summary>
/// Reprezentuje DTO służące do określenia jakie pola mają być aktualizowane w produkcie typu "Doniczka", który jest powiązany z graczem.
/// </summary>
public abstract class PotUpdatePlayerRecordDto : ProductEnhancementsUpdatePlayerRecordDto
{
    /// <summary>
    /// Zawiera maksymalny zakres klasy gleby, jaki obsługuje doniczka.
    /// </summary>
    [DisplayName(PotFieldsHrNames.MaxRangeOfSoilClass)]
    public int MaxRangeOfSoilClass { get; set; }
       
    /// <summary>
    /// Zawiera pojemność doniczki.
    /// </summary>
    [DisplayName(PotFieldsHrNames.Capacity)]
    public decimal Capacity { get; set; }
}