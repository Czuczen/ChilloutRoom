using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Drop.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.General.Drop))]
public class DropDto : EntityDto<int>
{
    [DisplayName(DropFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [DisplayName(EntitiesHrNames.GeneratedType), Display(GroupName = EntitiesDbNames.GeneratedType)]
    public int? GeneratedTypeId { get; set; }
        
    [DisplayName(DropFieldsHrNames.ChanceForDrop)]
    public decimal? ChanceForDrop { get; set; }

    [DisplayName(DropFieldsHrNames.ItemAmount)]
    public decimal? ItemAmount { get; set; }

    [DisplayName(DropFieldsHrNames.Gold)]
    public decimal? Gold { get; set; }
        
    [DisplayName(DropFieldsHrNames.Prestige)]
    public decimal? Prestige { get; set; }

    [DisplayName(DropFieldsHrNames.QuestToken)]
    public int? QuestToken { get; set; }
        
    [DisplayName(DropFieldsHrNames.DealerToken)]
    public int? DealerToken { get; set; }
        
    [DisplayName(DropFieldsHrNames.BlackMarketToken)]
    public int? BlackMarketToken { get; set; }
        
    [DisplayName(DropFieldsHrNames.DonToken)]
    public int? DonToken { get; set; }
        
    [DisplayName(DropFieldsHrNames.UnlockToken)]
    public int? UnlockToken { get; set; }
        
    [DisplayName(DropFieldsHrNames.Honor)]
    public int? Honor { get; set; }
        
    [DisplayName(DropFieldsHrNames.Experience)]
    public decimal? Experience { get; set; }

    [DisplayName(EntitiesHrNames.District), Display(GroupName = EntitiesDbNames.District)]
    public int DistrictId { get; set; }
}