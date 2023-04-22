using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.News.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.Others.News))]
public class NewsDto : PartMessagesDto
{
    [DisplayName(NewsFieldsHrNames.Header)]
    public string Header { get; set; }
        
    [DisplayName(NewsFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    [DisplayName(NewsFieldsHrNames.TextColor)]
    public string TextColor { get; set; }
}