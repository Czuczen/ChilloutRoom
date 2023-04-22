using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Base;

public interface IPlayerStorageService : ITransientDependency
{
    Task<ExtendingModels.Models.General.PlayerStorage> GetPlayerStorage(long userId);

    Task CreatePlayerStorageIfNotExist(long userId, string userName);
}