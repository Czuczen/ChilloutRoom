using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdatePlayerRecord;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

/// <summary>
/// Reprezentuje DTO służące do określenia jakie pola mają być aktualizowane w produkcie typu "Woda", który jest powiązany z graczem.
/// </summary>
public abstract class WaterUpdatePlayerRecordDto : ProductEnhancementsUpdatePlayerRecordDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [DisplayName(WaterFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}