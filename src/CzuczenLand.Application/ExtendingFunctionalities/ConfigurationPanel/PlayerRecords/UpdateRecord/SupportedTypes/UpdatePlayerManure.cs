using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu Manure.
/// </summary>
public class UpdatePlayerManure : UpdatePlayerRecord<Manure, ManureUpdateDto, ManureUpdatePlayerRecordDto>, IUpdatePlayerRecord<ManureUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium nawozów.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdatePlayerManure(
        IRepository<Manure> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}