using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu Quest.
/// </summary>
public class UpdatePlayerQuest : UpdatePlayerRecord<Quest, QuestUpdateDto, QuestUpdatePlayerRecordDto>, IUpdatePlayerRecord<QuestUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium zadań.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdatePlayerQuest(
        IRepository<Quest> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}