using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;

/// <summary>
/// Reprezentuje DTO dla produktu typu "Bonus".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Bonus))]
public class BonusDto : ProductEnhancementsDto
{
    /// <summary>
    /// Określa czy bonus jest artefaktem.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.IsArtifact)]
    public bool IsArtifact { get; set; }

    /// <summary>
    /// Koszt umieszczenia artefaktu.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.ArtifactPutCost)]
    public decimal? ArtifactPutCost { get; set; }
        
    /// <summary>
    /// Koszt wyciągnięcia artefaktu.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.ArtifactPullCost)]
    public decimal? ArtifactPullCost { get; set; }
    
    /// <summary>
    /// Pozostały aktywny czas bonusu.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.RemainingActiveTime)]
    public int? RemainingActiveTime { get; set; }
       
    /// <summary>
    /// Czas aktywności bonusu na jedno użycie.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.ActiveTimePerUse)]
    public int? ActiveTimePerUse { get; set; }
        
    /// <summary>
    /// Kolor bonusu.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.Color)]
    public string Color { get; set; }
        
    /// <summary>
    /// Ilość użyć bonusu.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.Usages)]
    public int Usages { get; set; }
        
    /// <summary>
    /// Określa czy bonus jest aktywny.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.IsActive)]
    public bool IsActive { get; set; }
        
    /// <summary>
    /// Określa czy bonus jest ustawiany na stos.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.IsStackable)]
    public bool? IsStackable { get; set; }
        
    /// <summary>
    /// Zwiększenie szansy na zdobycie nagrody z zadań.
    /// </summary>
    [DisplayName(BonusFieldsHrNames.IncreaseDropChanceFromQuests)]
    public decimal? IncreaseDropChanceFromQuests { get; set; }
}