using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;

/// <summary>
/// Reprezentuje DTO dla produktu typu "Doniczka".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Pot))]
public class PotDto : ProductEnhancementsDto
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

    /// <summary>
    /// Zawiera ilość doniczek w użyciu.
    /// </summary>
    [DisplayName(PotFieldsHrNames.InUseCount)]
    public int InUseCount { get; set; }
}