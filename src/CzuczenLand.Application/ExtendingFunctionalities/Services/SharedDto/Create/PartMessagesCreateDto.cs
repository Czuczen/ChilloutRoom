using System.ComponentModel;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

public class PartMessagesCreateDto
{
    [FieldIsRequired]
    [DisplayName(PartMessagesFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [DisplayName(PartMessagesFieldsHrNames.MessageContentOne)]
    public string MessageContentOne { get; set; }

    [DisplayName(PartMessagesFieldsHrNames.MessageContentTwo)]
    public string MessageContentTwo { get; set; }

    [DisplayName(PartMessagesFieldsHrNames.MessageContentThree)]
    public string MessageContentThree { get; set; }
}