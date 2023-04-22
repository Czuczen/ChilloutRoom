using Abp.Application.Services.Dto;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.QuestInfo;

public class PartMessagesInfoViewModel : EntityDto<int>
{
    public string Name { get; set; }
        
    public string MessageContentOne { get; set; }
        
    public string MessageContentTwo { get; set; }
        
    public string MessageContentThree { get; set; }
}