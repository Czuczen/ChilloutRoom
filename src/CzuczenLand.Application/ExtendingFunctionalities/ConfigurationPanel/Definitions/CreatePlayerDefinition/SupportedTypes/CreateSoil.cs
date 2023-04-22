using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition.SupportedTypes;

public class CreateSoil : CreateDefinition<Soil, SoilCreateDto>, ICreateDefinition<SoilCreateDto>
{
    public CreateSoil(
        IRepository<Soil> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    ) 
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}