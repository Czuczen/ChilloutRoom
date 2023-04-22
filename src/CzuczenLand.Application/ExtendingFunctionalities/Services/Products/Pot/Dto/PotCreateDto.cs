using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Create;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.Pot))]
public class PotCreateDto : ProductEnhancementsCreateDto
{
    [FieldIsRequired]
    [DisplayName(PotFieldsHrNames.MaxRangeOfSoilClass)]
    public int MaxRangeOfSoilClass { get; set; }
        
    [FieldIsRequired]
    [DisplayName(PotFieldsHrNames.Capacity)]
    public decimal Capacity { get; set; }
}