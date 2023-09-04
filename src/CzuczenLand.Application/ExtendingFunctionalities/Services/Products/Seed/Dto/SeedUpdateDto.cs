using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w definicji produktu typu "Nasiono".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.Seed))]
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Seed))]
public class SeedUpdateDto : ProductEnhancementsUpdateDto
{
    /// <summary>
    /// Zawiera ilość zużywanego nawozu.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.ManureConsumption)]
    public decimal ManureConsumption { get; set; }
        
    /// <summary>
    /// Zawiera ilość zużywanej wody.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.WaterConsumption)]
    public decimal WaterConsumption { get; set; }
        
    /// <summary>
    /// Zawiera opis nasiona.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.Description)]
    public string Description { get; set; }

    /// <summary>
    /// Zawiera adres URL obrazu.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    /// <summary>
    /// Zawiera wymagane zapotrzebowanie na pojemność w doniczce.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}