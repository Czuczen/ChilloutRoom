using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition.SupportedTypes;

public class CreatePot : CreateDefinition<Pot, PotCreateDto>, ICreateDefinition<PotCreateDto>
{
    public CreatePot(
        IRepository<Pot> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    ) 
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}