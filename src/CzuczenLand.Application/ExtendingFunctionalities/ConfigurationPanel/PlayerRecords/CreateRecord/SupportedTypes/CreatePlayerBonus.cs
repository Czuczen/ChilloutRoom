using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord.SupportedTypes;

/// <summary>
/// Klasa do tworzenia obiektów typu Bonus.
/// </summary>
public class CreatePlayerBonus : CreatePlayerRecord<Bonus, BonusCreateDto>, ICreatePlayerRecord<BonusCreateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium bonusów.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    public CreatePlayerBonus(
        IRepository<Bonus> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    ) 
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}