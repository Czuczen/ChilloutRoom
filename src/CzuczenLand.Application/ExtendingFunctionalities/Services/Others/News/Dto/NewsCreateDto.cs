using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.News.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia encji "News".
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.Others.News))]
public class NewsCreateDto : PartMessagesCreateDto
{
    /// <summary>
    /// Nagłówek wiadomości.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(NewsFieldsHrNames.Header)]
    public string Header { get; set; }
        
    /// <summary>
    /// Adres URL obrazu dla wiadomości.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(NewsFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    /// <summary>
    /// Kolor tekstu wiadomości.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(NewsFieldsHrNames.TextColor)]
    public string TextColor { get; set; }
}