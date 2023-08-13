using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;

/// <summary>
/// Serwis podstawowy obsługujący logikę biznesową związaną z magazynem plantacji.
/// </summary>
public class PlantationStorageService : IPlantationStorageService
{
    /// <summary>
    /// Repozytorium magazynu gracza.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Repozytorium magazynu plantacji.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.PlantationStorage> _plantationStorageRepository;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="plantationStorageRepository">Repozytorium magazynu plantacji.</param>
    /// <param name="playerStorageRepository">Repozytorium magazynu gracza.</param>
    public PlantationStorageService(
        IRepository<ExtendingModels.Models.General.PlantationStorage> plantationStorageRepository,
        IRepository<ExtendingModels.Models.General.PlayerStorage> playerStorageRepository
    )
    {
        _plantationStorageRepository = plantationStorageRepository;
        _playerStorageRepository = playerStorageRepository;
    }

    /// <summary>
    /// Pobiera magazyn plantacji gracza dla ostatnio wybranej dzielnicy.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <returns>Obiekt reprezentujący magazyn plantacji.</returns>
    public async Task<ExtendingModels.Models.General.PlantationStorage> GetPlayerPlantationStorageForLastSelectedDistrictAsync(long userId)
    {
        var ret = (await _plantationStorageRepository.GetAll().Where(item => item.UserId == userId).Join(
            _playerStorageRepository.GetAll().Where(item => item.UserId == userId),
            plantationStorage => plantationStorage.DistrictId, 
            playerStorage => playerStorage.LastSelectedDistrictId,
            (plantationStorage, playerStorage) => plantationStorage).ToListAsync()).Single();

        return ret;
    }
}