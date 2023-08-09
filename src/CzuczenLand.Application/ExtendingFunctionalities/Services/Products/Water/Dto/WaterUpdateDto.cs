﻿using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w definicji produktu typu "Woda".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Products.Water))]
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Water))]
public class WaterUpdateDto : ProductEnhancementsUpdateDto
{
    /// <summary>
    /// Wymagana pojemność doniczki.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(WaterFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}