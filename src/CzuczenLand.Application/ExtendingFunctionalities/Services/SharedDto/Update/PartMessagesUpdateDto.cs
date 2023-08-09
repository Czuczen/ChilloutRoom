using System.ComponentModel;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

/// <summary>
/// Klasa reprezentująca pola aktualizowane w części wiadomości lub w definicji zadania.
/// </summary>
public class PartMessagesUpdateDto : EntityDto<int>
{
    /// <summary>
    /// Nazwa wiadomości.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(PartMessagesFieldsHrNames.Name)]
    public string Name { get; set; }
        
    /// <summary>
    /// Treść części pierwszej wiadomości.
    /// </summary>
    [DisplayName(PartMessagesFieldsHrNames.MessageContentOne)]
    public string MessageContentOne { get; set; }

    /// <summary>
    /// Treść części drugiej wiadomości.
    /// </summary>
    [DisplayName(PartMessagesFieldsHrNames.MessageContentTwo)]
    public string MessageContentTwo { get; set; }

    /// <summary>
    /// Treść części trzeciej wiadomości.
    /// </summary>
    [DisplayName(PartMessagesFieldsHrNames.MessageContentThree)]
    public string MessageContentThree { get; set; }
}