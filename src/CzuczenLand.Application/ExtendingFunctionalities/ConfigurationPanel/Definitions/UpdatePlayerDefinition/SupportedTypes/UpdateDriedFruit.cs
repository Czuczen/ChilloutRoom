using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

public class UpdateDriedFruit : UpdateDefinition<DriedFruit, DriedFruitUpdateDto, DriedFruitUpdateDefinitionDto>, IUpdateDefinition<DriedFruitUpdateDto>
{
    public UpdateDriedFruit(
        IRepository<DriedFruit> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}