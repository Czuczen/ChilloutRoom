using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.UpdatePlayerRecord;

/// <summary>
/// Klasa reprezentująca pola aktualizowane w zadaniu powiązanym z graczem.
/// </summary>
public class PartMessagesUpdatePlayerRecordDto
{
    /// <summary>
    /// Nazwa wiadomości.
    /// </summary>
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