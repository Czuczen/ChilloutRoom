using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingModels.Models.Others;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.TicTacToe.Base;

/// <summary>
/// Interfejs serwisu do obsługi przechowywania danych gry "Kółko i krzyżyk".
/// Rozszerza interfejs ITransientDependency.
/// </summary>
public interface ITicTacToeStorageService : ITransientDependency
{
    /// <summary>
    /// Pobiera istniejące dane gry dla użytkownika lub tworzy nowe, jeśli nie istnieją.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <returns>Obiekt przechowujący dane gry "Kółko i krzyżyk".</returns>
    Task<TicTacToeStorage> GetOrInitStorage(long userId, string userName);
}