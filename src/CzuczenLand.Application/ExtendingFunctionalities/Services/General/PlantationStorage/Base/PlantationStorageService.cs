using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;

public class PlantationStorageService : IPlantationStorageService
{
    private readonly IRepository<ExtendingModels.Models.General.PlayerStorage> _playerStorageRepository;
    private readonly IRepository<ExtendingModels.Models.General.PlantationStorage> _plantationStorageRepository;

    
    public PlantationStorageService(
        IRepository<ExtendingModels.Models.General.PlantationStorage> plantationStorageRepository,
        IRepository<ExtendingModels.Models.General.PlayerStorage> playerStorageRepository
    )
    {
        _plantationStorageRepository = plantationStorageRepository;
        _playerStorageRepository = playerStorageRepository;
    }

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