using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu Soil.
/// </summary>
public class UpdatePlayerSoil : UpdatePlayerRecord<Soil, SoilUpdateDto, SoilUpdatePlayerRecordDto>, IUpdatePlayerRecord<SoilUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium gleb.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdatePlayerSoil(
        IRepository<Soil> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}