using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Drop.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia nagrody.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.Drop))]
public class DropCreateDto
{
    /// <summary>
    /// Nazwa nagrody.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DropFieldsHrNames.Name)]
    public string Name { get; set; }

    /// <summary>
    /// Identyfikator typu generowanego powiązanego z nagrodą.
    /// </summary>
    [DisplayName(EntitiesHrNames.GeneratedType)]
    public int? GeneratedTypeId { get; set; }

    /// <summary>
    /// Szansa na nagrodę.
    /// </summary>
    [DisplayName(DropFieldsHrNames.ChanceForDrop)]
    public decimal? ChanceForDrop { get; set; }

    /// <summary>
    /// Ilość przedmiotu.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DropFieldsHrNames.ItemAmount)]
    public decimal? ItemAmount { get; set; }
        
    /// <summary>
    /// Ilość złota.
    /// </summary>
    [DisplayName(DropFieldsHrNames.Gold)]
    public decimal? Gold { get; set; }
        
    /// <summary>
    /// Ilość prestiżu.
    /// </summary>
    [DisplayName(DropFieldsHrNames.Prestige)]
    public decimal? Prestige { get; set; }
        
    /// <summary>
    /// Ilość tokenów misji.
    /// </summary>
    [DisplayName(DropFieldsHrNames.QuestToken)]
    public int? QuestToken { get; set; }
        
    /// <summary>
    /// Ilość tokenów dealera.
    /// </summary>
    [DisplayName(DropFieldsHrNames.DealerToken)]
    public int? DealerToken { get; set; }
        
    /// <summary>
    /// Ilość tokenów czarnego rynku.
    /// </summary>
    [DisplayName(DropFieldsHrNames.BlackMarketToken)]
    public int? BlackMarketToken { get; set; }
        
    /// <summary>
    /// Ilość tokenów dona.
    /// </summary>
    [DisplayName(DropFieldsHrNames.DonToken)]
    public int? DonToken { get; set; }
        
    /// <summary>
    /// Ilość tokenów odblokowania.
    /// </summary>
    [DisplayName(DropFieldsHrNames.UnlockToken)]
    public int? UnlockToken { get; set; }
        
    /// <summary>
    /// Ilość honoru.
    /// </summary>
    [DisplayName(DropFieldsHrNames.Honor)]
    public int? Honor { get; set; }
        
    /// <summary>
    /// Ilość doświadczenia.
    /// </summary>
    [DisplayName(DropFieldsHrNames.Experience)]
    public decimal? Experience { get; set; }

    /// <summary>
    /// Identyfikator dzielnicy, do której przypisana jest nagroda.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.District)]
    public int DistrictId { get; set; }
}