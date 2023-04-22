using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.Products.Seed))]
[AutoMapFrom(typeof(ExtendingModels.Models.Products.Seed))]
public class SeedUpdateDto : ProductEnhancementsUpdateDto
{
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.ManureConsumption)]
    public decimal ManureConsumption { get; set; }
        
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.WaterConsumption)]
    public decimal WaterConsumption { get; set; }
        
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.Description)]
    public string Description { get; set; }

    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    [FieldIsRequired]
    [DisplayName(SeedFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
        
}