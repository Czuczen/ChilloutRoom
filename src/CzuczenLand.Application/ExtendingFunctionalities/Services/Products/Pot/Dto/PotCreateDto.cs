using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia produktu typu "Doniczka".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.Pot))]
public class PotCreateDto : ProductEnhancementsCreateDto
{
    /// <summary>
    /// Zawiera maksymalny zakres klasy gleby, jaki obsługuje doniczka.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(PotFieldsHrNames.MaxRangeOfSoilClass)]
    public int MaxRangeOfSoilClass { get; set; }
        
    /// <summary>
    /// Zawiera pojemność doniczki.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(PotFieldsHrNames.Capacity)]
    public decimal Capacity { get; set; }
}