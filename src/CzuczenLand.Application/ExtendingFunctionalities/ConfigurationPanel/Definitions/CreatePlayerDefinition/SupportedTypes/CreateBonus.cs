using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition.SupportedTypes;

public class CreateBonus : CreateDefinition<Bonus, BonusCreateDto>, ICreateDefinition<BonusCreateDto>
{
    public CreateBonus(
        IRepository<Bonus> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    ) 
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}