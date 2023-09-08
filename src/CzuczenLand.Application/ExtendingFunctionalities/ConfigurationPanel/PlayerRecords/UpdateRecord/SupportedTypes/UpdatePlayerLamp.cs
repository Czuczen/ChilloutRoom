using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu Lamp.
/// </summary>
public class UpdatePlayerLamp : UpdatePlayerRecord<Lamp, LampUpdateDto, LampUpdatePlayerRecordDto>, IUpdatePlayerRecord<LampUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium lamp.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdatePlayerLamp(
        IRepository<Lamp> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}