using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia magazynu gracza.
/// System panelu konfiguracyjnego i tak nie pozwoli tworzyć rekordów magazynu gracza.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.PlayerStorage))]
public class PlayerStorageCreateDto : PartStorageCreateDto
{
    /// <summary>
    /// Ilość honoru gracza.
    /// </summary>
    [DisplayName(PlayerStorageFieldsHrNames.Honor)]
    public int Honor { get; set; }
}
