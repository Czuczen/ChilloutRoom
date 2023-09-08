using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord.SupportedTypes;

/// <summary>
/// Klasa do tworzenia obiektów typu Soil.
/// </summary>
public class CreatePlayerSoil : CreatePlayerRecord<Soil, SoilCreateDto>, ICreatePlayerRecord<SoilCreateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium gleb.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    public CreatePlayerSoil(
        IRepository<Soil> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    ) 
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}