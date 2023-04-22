using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.General.PlayerStorage))]
public class PlayerStorageDto : PartStorageDto
{
    [DisplayName(PlayerStorageFieldsHrNames.PlayerName)]
    public string PlayerName { get; set; }
        
    [DisplayName(PlayerStorageFieldsHrNames.Honor)]
    public int Honor { get; set; }
        
    [DisplayName(PlayerStorageFieldsHrNames.LastSelectedDistrictId), Display(GroupName = EntitiesDbNames.District)]
    public int? LastSelectedDistrictId { get; set; }
}
