using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdatePlayerRecord;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

/// <summary>
/// Reprezentuje DTO służące do określenia jakie pola mają być aktualizowane w produkcie typu "Lampa", który jest powiązany z graczem.
/// </summary>
[AutoMapFrom(typeof(LampUpdateDto))]
public abstract class LampUpdatePlayerRecordDto : ProductEnhancementsUpdatePlayerRecordDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [DisplayName(LampFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}