using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.News.Dto;

/// <summary>
/// Reprezentuje DTO dla encji "News".
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.Others.News))]
public class NewsDto : PartMessagesDto
{
    /// <summary>
    /// Nagłówek wiadomości.
    /// </summary>
    [DisplayName(NewsFieldsHrNames.Header)]
    public string Header { get; set; }
        
    /// <summary>
    /// Adres URL obrazu dla wiadomości.
    /// </summary>
    [DisplayName(NewsFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    /// <summary>
    /// Kolor tekstu wiadomości.
    /// </summary>
    [DisplayName(NewsFieldsHrNames.TextColor)]
    public string TextColor { get; set; }
}