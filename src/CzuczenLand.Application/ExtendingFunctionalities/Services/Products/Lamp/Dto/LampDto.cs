using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

[AutoMapFrom(typeof(ExtendingModels.Models.Products.Lamp))]
public class LampDto : ProductEnhancementsDto
{
    [DisplayName(LampFieldsHrNames.InUseCount)]
    public int InUseCount { get; set; }
        
    [DisplayName(LampFieldsHrNames.CapacityInPotRequirement)]
    public decimal CapacityInPotRequirement { get; set; }
}