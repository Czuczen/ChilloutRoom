using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.Products.Seed))]
public class SeedDto : ProductEnhancementsDto
{
    [DisplayName(SeedFieldsHrNames.ManureConsumption)]
    public decimal ManureConsumption { get; set; }
        
    [DisplayName(SeedFieldsHrNames.WaterConsumption)]
    public decimal WaterConsumption { get; set; }
        
    [DisplayName(SeedFieldsHrNames.Description)]
    public string Description { get; set; }

    [DisplayName(SeedFieldsHrNames.ImageUrl)]
    public string ImageUrl { get; set; }
        
    [DisplayName(SeedFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}