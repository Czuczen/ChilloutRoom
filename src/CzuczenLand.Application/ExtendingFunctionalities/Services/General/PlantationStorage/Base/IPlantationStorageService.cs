using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;

public interface IPlantationStorageService : ITransientDependency
{
    Task<ExtendingModels.Models.General.PlantationStorage> GetPlayerPlantationStorageForLastSelectedDistrictAsync(long userId);
}