﻿using System.ComponentModel;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

/// <summary>
/// Klasa reprezentująca dane transferowe (DTO) części wiadomości, dziedzicząca po EntityDto int.
/// </summary>
public class PartMessagesDto : EntityDto<int>
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