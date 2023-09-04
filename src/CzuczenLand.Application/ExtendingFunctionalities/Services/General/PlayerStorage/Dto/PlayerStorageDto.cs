using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

/// <summary>
/// Reprezentuje DTO dla magazynu gracza.
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.General.PlayerStorage))]
public class PlayerStorageDto : PartStorageDto
{
    /// <summary>
    /// Nazwa gracza.
    /// </summary>
    [DisplayName(PlayerStorageFieldsHrNames.PlayerName)]
    public string PlayerName { get; set; }
        
    /// <summary>
    /// Ilość honoru gracza.
    /// </summary>
    [DisplayName(PlayerStorageFieldsHrNames.Honor)]
    public int Honor { get; set; }

    /// <summary>
    /// Identyfikator ostatnio wybranej dzielnicy.
    /// </summary>
    [DisplayName(PlayerStorageFieldsHrNames.LastSelectedDistrictId), Display(GroupName = EntitiesDbNames.District)]
    public int? LastSelectedDistrictId { get; set; }
}
