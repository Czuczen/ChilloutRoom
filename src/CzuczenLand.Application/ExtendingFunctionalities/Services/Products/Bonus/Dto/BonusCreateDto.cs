using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.Bonus))]
public class BonusCreateDto : ProductEnhancementsCreateDto
{
    [FieldIsRequired]
    [DisplayName(BonusFieldsHrNames.IsArtifact)]
    public bool IsArtifact { get; set; }
        
    [DisplayName(BonusFieldsHrNames.ArtifactPutCost)]
    public decimal? ArtifactPutCost { get; set; }
        
    [DisplayName(BonusFieldsHrNames.ArtifactPullCost)]
    public decimal? ArtifactPullCost { get; set; }

    [DisplayName(BonusFieldsHrNames.ActiveTimePerUse)]
    public int? ActiveTimePerUse { get; set; }
        
    [DisplayName(BonusFieldsHrNames.Color)]
    public string Color { get; set; }
        
    [DisplayName(BonusFieldsHrNames.IsStackable)]
    public bool? IsStackable { get; set; }
        
    [DisplayName(BonusFieldsHrNames.IncreaseDropChanceFromQuests)]
    public decimal? IncreaseDropChanceFromQuests { get; set; }
}