using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.News.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Others.News))]
public class NewsCreateDto : PartMessagesCreateDto
{
    [FieldIsRequired]
    [DisplayName(NewsFieldsHrNames.Header)]
    public string Header { get; set; }
        
    [FieldIsRequired]
    [DisplayName(NewsFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    [FieldIsRequired]
    [DisplayName(NewsFieldsHrNames.TextColor)]
    public string TextColor { get; set; }
}