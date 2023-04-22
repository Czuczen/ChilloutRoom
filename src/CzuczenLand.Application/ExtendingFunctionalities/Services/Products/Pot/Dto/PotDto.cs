using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.Products.Pot))]
public class PotDto : ProductEnhancementsDto
{
    [DisplayName(PotFieldsHrNames.MaxRangeOfSoilClass)]
    public int MaxRangeOfSoilClass { get; set; }
        
    [DisplayName(PotFieldsHrNames.Capacity)]
    public decimal Capacity { get; set; }
        
    [DisplayName(PotFieldsHrNames.InUseCount)]
    public int InUseCount { get; set; }
}