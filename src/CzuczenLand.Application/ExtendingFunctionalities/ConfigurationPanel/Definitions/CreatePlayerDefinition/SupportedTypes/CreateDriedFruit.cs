using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition.SupportedTypes;

public class CreateDriedFruit : CreateDefinition<DriedFruit, DriedFruitCreateDto>, ICreateDefinition<DriedFruitCreateDto>
{
    public CreateDriedFruit(
        IRepository<DriedFruit> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    ) 
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}