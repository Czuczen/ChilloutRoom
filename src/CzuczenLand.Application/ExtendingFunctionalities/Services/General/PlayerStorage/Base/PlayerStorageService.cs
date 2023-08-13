using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Base;

/// <summary>
/// Serwis podstawowy obsługujący logikę biznesową związaną z magazynem gracza
/// </summary>
public class PlayerStorageService : IPlayerStorageService
{
    /// <summary>
    /// Repozytorium magazynu gracza.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.PlayerStorage, int> _playerStorageRepository;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="playerStorageRepository">Repozytorium magazynu gracza.</param>
    public PlayerStorageService(IRepository<ExtendingModels.Models.General.PlayerStorage, int> playerStorageRepository)
    {
        _playerStorageRepository = playerStorageRepository;
    }

    /// <summary>
    /// Pobiera magazyn gracza na podstawie identyfikatora użytkownika.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <returns>Obiekt reprezentujący magazyn gracza.</returns>
    public async Task<ExtendingModels.Models.General.PlayerStorage> GetPlayerStorage(long userId) =>
        await _playerStorageRepository.SingleAsync(currPlayerStorage => currPlayerStorage.UserId == userId);
        
    /// <summary>
    /// Tworzy magazyn gracza, jeśli nie istnieje.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
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