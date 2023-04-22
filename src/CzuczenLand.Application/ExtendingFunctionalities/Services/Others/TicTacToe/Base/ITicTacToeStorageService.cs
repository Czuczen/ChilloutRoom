using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingModels.Models.Others;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.TicTacToe.Base;

public interface ITicTacToeStorageService : ITransientDependency
{
    Task<TicTacToeStorage> GetOrInitStorage(long userId, string userName);
}