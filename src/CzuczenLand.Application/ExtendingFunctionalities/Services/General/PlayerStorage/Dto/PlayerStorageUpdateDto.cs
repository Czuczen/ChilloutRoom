using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w magazynie gracza.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.PlayerStorage))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.PlayerStorage))]
public class PlayerStorageUpdateDto : PartStorageUpdateDto
{
    /// <summary>
    /// Ilość honoru gracza.
    /// </summary>
    [DisplayName(PlayerStorageFieldsHrNames.Honor)]
    public int Honor { get; set; }
}