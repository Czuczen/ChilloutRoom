using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;

/// <summary>
/// Reprezentuje DTO dla produktu typu "Nasiono".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Seed))]
public class SeedDto : ProductEnhancementsDto
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