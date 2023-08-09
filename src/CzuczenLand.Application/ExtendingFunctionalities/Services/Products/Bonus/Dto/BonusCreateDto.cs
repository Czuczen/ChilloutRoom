﻿using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia produktu typu "Bonus".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.Bonus))]
public class BonusCreateDto : ProductEnhancementsCreateDto
{
    /// <summary>
    /// Określa czy bonus jest artefaktem.
    /// </summary>
    [FieldIsRequired]
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