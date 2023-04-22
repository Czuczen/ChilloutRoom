using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;

namespace CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Loader;

public interface ICustomRepositoryLoader : ITransientDependency
{
    ICustomRepository GetRepository(string entityName);

    Task<string> GetObjectName(string entityName, int id);
}