using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord.SupportedTypes;

/// <summary>
/// Klasa do tworzenia obiektów typu Lamp.
/// </summary>
public class CreatePlayerLamp : CreatePlayerRecord<Lamp, LampCreateDto>, ICreatePlayerRecord<LampCreateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium lamp.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    public CreatePlayerLamp(
        IRepository<Lamp> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    )
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}