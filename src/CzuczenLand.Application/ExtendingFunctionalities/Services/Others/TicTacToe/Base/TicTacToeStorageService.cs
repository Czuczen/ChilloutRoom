using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingModels.Models.Others;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.TicTacToe.Base;

/// <summary>
/// Serwis do obsługi przechowywania danych gry "Kółko i krzyżyk".
/// Implementuje interfejs ITicTacToeStorageService.
/// </summary>
public class TicTacToeStorageService : ITicTacToeStorageService
{
    /// <summary>
    /// Repozytorium przechowujące dane gry.
    /// </summary>
    private readonly IRepository<TicTacToeStorage> _ticTacToeStorageRepository;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="ticTacToeStorageRepository">Repozytorium przechowywania danych gry.</param>
    public TicTacToeStorageService(IRepository<TicTacToeStorage> ticTacToeStorageRepository)
    {
        _ticTacToeStorageRepository = ticTacToeStorageRepository;
    }

    /// <summary>
    /// Pobiera istniejące dane gry dla użytkownika lub tworzy nowe, jeśli nie istnieją.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <returns>Obiekt przechowujący dane gry "Kółko i krzyżyk".</returns>
    public async Task<TicTacToeStorage> GetOrInitStorage(long userId, string userName)
    {
        var storage = (await _ticTacToeStorageRepository.GetAllListAsync(item => item.UserId == userId)).SingleOrDefault();
        if (storage == null)
        {
            var newStorage = new TicTacToeStorage
            {
                UserId = userId,
                PlayerName = userName,
                GamesPlayed = 0,
                GamesWon = 0,
                GamesLost = 0,
                TiedGames = 0,
            };

            var storageId = await _ticTacToeStorageRepository.InsertAndGetIdAsync(newStorage);
            storage = await _ticTacToeStorageRepository.GetAsync(storageId);
        }

        return storage;
    }
}