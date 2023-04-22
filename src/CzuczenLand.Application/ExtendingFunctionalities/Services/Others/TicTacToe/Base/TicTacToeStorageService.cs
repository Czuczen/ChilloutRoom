using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingModels.Models.Others;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.TicTacToe.Base;

public class TicTacToeStorageService : ITicTacToeStorageService
{
    private readonly IRepository<TicTacToeStorage> _ticTacToeStorageRepository;

    
    public TicTacToeStorageService(IRepository<TicTacToeStorage> ticTacToeStorageRepository)
    {
        _ticTacToeStorageRepository = ticTacToeStorageRepository;
    }

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