using Abp.AutoMapper;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.QuestInfo;

[AutoMapFrom(typeof(Drop))]
public class DropInfoViewModel
{
    public string Name { get; set; }
}