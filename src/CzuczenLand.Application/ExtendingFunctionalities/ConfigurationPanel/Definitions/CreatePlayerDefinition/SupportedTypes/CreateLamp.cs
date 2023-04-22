using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition.SupportedTypes;

public class CreateLamp : CreateDefinition<Lamp, LampCreateDto>, ICreateDefinition<LampCreateDto>
{
    public CreateLamp(
        IRepository<Lamp> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    )
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}