using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Base;

public class PlayerStorageService : IPlayerStorageService
{
    private readonly IRepository<ExtendingModels.Models.General.PlayerStorage, int> _playerStorageRepository;

    
    public PlayerStorageService(IRepository<ExtendingModels.Models.General.PlayerStorage, int> playerStorageRepository)
    {
        _playerStorageRepository = playerStorageRepository;
    }

    public async Task<ExtendingModels.Models.General.PlayerStorage> GetPlayerStorage(long userId) =>
        await _playerStorageRepository.SingleAsync(currPlayerStorage => currPlayerStorage.UserId == userId);
        
    public async Task CreatePlayerStorageIfNotExist(long userId, string userName)
    {
        var playerStorage = (await _playerStorageRepository.GetAllListAsync(currPlayerStorage => currPlayerStorage.UserId == userId)).SingleOrDefault();
        if (playerStorage == null)
        {
            var playerStorageToCreate = new ExtendingModels.Models.General.PlayerStorage
            {
                Name = "Magazyn użytkownika - " + userName,
                PlayerName = userName,
                UserId = userId,
                Level = 0,
                Gold = 50,
            };

            await _playerStorageRepository.InsertAndGetIdAsync(playerStorageToCreate);
        }
    }
}