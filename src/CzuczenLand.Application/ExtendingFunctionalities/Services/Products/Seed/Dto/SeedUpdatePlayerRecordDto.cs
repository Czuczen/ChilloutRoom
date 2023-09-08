using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdatePlayerRecord;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;

/// <summary>
/// Reprezentuje DTO służące do określenia jakie pola mają być aktualizowane w produkcie typu "Nasiono", który jest powiązany z graczem.
/// </summary>
public abstract class SeedUpdatePlayerRecordDto : ProductEnhancementsUpdatePlayerRecordDto
{
    /// <summary>
    /// Zawiera ilość zużywanego nawozu.
    /// </summary>
    [DisplayName(SeedFieldsHrNames.ManureConsumption)]
    public decimal ManureConsumption { get; set; }
        
    /// <summary>
    /// Zawiera ilość zużywanej wody.
    /// </summary>
    [DisplayName(SeedFieldsHrNames.WaterConsumption)]
    public decimal WaterConsumption { get; set; }
        
    /// <summary>
    /// Zawiera opis nasiona.
    /// </summary>
    [DisplayName(SeedFieldsHrNames.Description)]
    public string Description { get; set; }

    /// <summary>
    /// Zawiera adres URL obrazu.
    /// </summary>
    [DisplayName(SeedFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    /// <summary>
    /// Zawiera wymagane zapotrzebowanie na pojemność w doniczce.
    /// </summary>
    [DisplayName(SeedFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}