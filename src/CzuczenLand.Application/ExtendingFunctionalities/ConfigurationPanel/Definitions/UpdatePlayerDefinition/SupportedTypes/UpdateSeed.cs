using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

public class UpdateSeed : UpdateDefinition<Seed, SeedUpdateDto, SeedUpdateDefinitionDto>, IUpdateDefinition<SeedUpdateDto>
{
    public UpdateSeed(
        IRepository<Seed> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}