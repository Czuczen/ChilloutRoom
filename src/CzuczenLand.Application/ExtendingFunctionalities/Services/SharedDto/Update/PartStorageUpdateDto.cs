using System.ComponentModel;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

/// <summary>
/// Klasa reprezentująca pola aktualizowane w części magazynu.
/// </summary>
public class PartStorageUpdateDto : EntityDto<int>
{
    /// <summary>
    /// Poziom magazynu plantacji/gracza. 
    /// </summary>
    [FieldIsRequired]
    [DisplayName(PartStorageFieldsHrNames.Level)]
    public int Level { get; set; }
        
    /// <summary>
    /// Zdobyte doświadczenie.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(PartStorageFieldsHrNames.GainedExperience)]
    public decimal GainedExperience { get; set; }
    
    /// <summary>
    /// Ilość złota w magazynie plantacji/gracza.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(PartStorageFieldsHrNames.Gold)]
    public decimal Gold { get; set; }
}