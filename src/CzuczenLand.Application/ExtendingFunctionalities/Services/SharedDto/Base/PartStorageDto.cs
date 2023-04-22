using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

public class PartStorageDto : EntityDto<int>
{
    [DisplayName(PartStorageFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [DisplayName(PartStorageFieldsHrNames.Level)]
    public int Level { get; set; }
        
    [DisplayName(PartStorageFieldsHrNames.GainedExperience)]
    public decimal GainedExperience { get; set; }
        
    [DisplayName(PartStorageFieldsHrNames.Gold)]
    public decimal Gold { get; set; }
        
    [DisplayName(EntitiesHrNames.User), Display(GroupName = EntitiesDbNames.User)]
    public long UserId { get; set; }
}