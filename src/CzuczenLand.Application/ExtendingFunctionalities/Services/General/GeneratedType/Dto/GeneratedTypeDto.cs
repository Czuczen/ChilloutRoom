using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.General.GeneratedType))]
public class GeneratedTypeDto : EntityDto<int>
{
    [DisplayName(GeneratedTypeFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [DisplayName(GeneratedTypeFieldsHrNames.EntityName)]
    public string EntityName { get; set; }

    [DisplayName(EntitiesHrNames.District), Display(GroupName = EntitiesDbNames.District)]
    public int DistrictId { get; set; }
}