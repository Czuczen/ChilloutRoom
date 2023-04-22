using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.Soil))]
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Soil))]
public class SoilUpdateDto : ProductEnhancementsUpdateDto
{
    [FieldIsRequired]
    [DisplayName(SoilFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
        
    [FieldIsRequired]
    [DisplayName(SoilFieldsHrNames.SoilClass)]
    public int SoilClass { get; set; }
}