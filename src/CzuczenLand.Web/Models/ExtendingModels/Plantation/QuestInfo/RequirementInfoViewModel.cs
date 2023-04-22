using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.QuestInfo;

[AutoMapFrom(typeof(Requirement))]
public class RequirementInfoViewModel : EntityDto<int>
{
    public string Name { get; set; }
        
    public decimal Amount { get; set; }
}