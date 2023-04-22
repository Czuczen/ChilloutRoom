using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

public class UpdateWater : UpdateDefinition<Water, WaterUpdateDto, WaterUpdateDefinitionDto>, IUpdateDefinition<WaterUpdateDto>
{
    public UpdateWater(
        IRepository<Water> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}