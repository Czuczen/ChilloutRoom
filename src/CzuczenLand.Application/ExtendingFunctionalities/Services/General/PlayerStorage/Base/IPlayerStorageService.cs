using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Base;

/// <summary>
/// Interfejs serwisu obsługującego magazyn gracza.
/// </summary>
public interface IPlayerStorageService : ITransientDependency
{
    /// <summary>
    /// Pobiera magazyn gracza na podstawie identyfikatora użytkownika.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <returns>Obiekt reprezentujący magazyn gracza.</returns>
    Task<ExtendingModels.Models.General.PlayerStorage> GetPlayerStorage(long userId);

    /// <summary>
    /// Tworzy magazyn gracza, jeśli nie istnieje.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    Task CreatePlayerStorageIfNotExist(long userId, string userName);
}