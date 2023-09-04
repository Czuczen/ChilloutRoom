using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition.SupportedTypes;

/// <summary>
/// Klasa do usuwania obiektów typu Quest.
/// </summary>
public class DeleteQuest : DeleteDefinition<Quest>, IDeleteDefinition<Quest>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium zadań.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantService">Serwis roślin.</param>
    public DeleteQuest(
        IRepository<Quest> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IPlantService plantService
    ) 
        : base(repository, districtRepository, generatedTypeRepository, plantService)
    {
    }
}