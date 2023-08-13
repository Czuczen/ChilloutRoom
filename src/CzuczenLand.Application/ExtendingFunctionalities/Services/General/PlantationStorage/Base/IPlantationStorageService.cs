using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;

/// <summary>
/// Interfejs serwisu obsługującego magazyn plantacji.
/// </summary>
public interface IPlantationStorageService : ITransientDependency
{
    /// <summary>
    /// Pobiera magazyn plantacji gracza dla ostatnio wybranej dzielnicy.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <returns>Obiekt reprezentujący magazyn plantacji.</returns>
    Task<ExtendingModels.Models.General.PlantationStorage> GetPlayerPlantationStorageForLastSelectedDistrictAsync(long userId);
}