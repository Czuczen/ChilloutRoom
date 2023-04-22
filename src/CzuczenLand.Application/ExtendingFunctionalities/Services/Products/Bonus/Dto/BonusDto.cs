using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.Products.Bonus))]
public class BonusDto : ProductEnhancementsDto
{
    [DisplayName(BonusFieldsHrNames.IsArtifact)]
    public bool IsArtifact { get; set; }

    [DisplayName(BonusFieldsHrNames.ArtifactPutCost)]
    public decimal? ArtifactPutCost { get; set; }
        
    [DisplayName(BonusFieldsHrNames.ArtifactPullCost)]
    public decimal? ArtifactPullCost { get; set; }
        
    [DisplayName(BonusFieldsHrNames.RemainingActiveTime)]
    public int? RemainingActiveTime { get; set; }
        
    [DisplayName(BonusFieldsHrNames.ActiveTimePerUse)]
    public int? ActiveTimePerUse { get; set; }
        
    [DisplayName(BonusFieldsHrNames.Color)]
    public string Color { get; set; }
        
    [DisplayName(BonusFieldsHrNames.Usages)]
    public int Usages { get; set; }
        
    [DisplayName(BonusFieldsHrNames.IsActive)]
    public bool IsActive { get; set; }
        
    [DisplayName(BonusFieldsHrNames.IsStackable)]
    public bool? IsStackable { get; set; }
        
    [DisplayName(BonusFieldsHrNames.IncreaseDropChanceFromQuests)]
    public decimal? IncreaseDropChanceFromQuests { get; set; }
}