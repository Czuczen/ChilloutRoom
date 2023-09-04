using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdateDefinition;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;

/// <summary>
/// Reprezentuje DTO służące do określenia jakie pola mają być aktualizowane w produkcie typu "Nawóz", który jest powiązany z graczem.
/// </summary>
public class ManureUpdateDefinitionDto : ProductEnhancementsUpdateDefinitionDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [DisplayName(ManureFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}