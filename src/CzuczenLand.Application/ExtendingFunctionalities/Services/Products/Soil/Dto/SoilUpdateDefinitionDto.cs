using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;

/// <summary>
/// Reprezentuje DTO służące do określenia jakie pola mają być aktualizowane w produkcie typu "Gleba", który jest powiązany z graczem.
/// </summary>
public class SoilUpdateDefinitionDto : ProductEnhancementsUpdateDefinitionDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [DisplayName(SoilFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
        
    /// <summary>
    /// Klasa gleby.
    /// </summary>
    [DisplayName(SoilFieldsHrNames.SoilClass)]
    public int SoilClass { get; set; }
}