using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Loader;

public class CustomRepositoryLoader : ICustomRepositoryLoader
{
    private readonly List<ICustomRepository> _customRepositories = new();

    private Dictionary<string, List<IEntity<int>>> Objects { get; } = new();

    
    public ICustomRepository GetRepository(string entityName)
    {
        if (_customRepositories.All(item => item.ForEntity != entityName))
            _customRepositories.Add(CustomRepositoryFactory.GetRepository(entityName));
            
        return _customRepositories.Single(item => item.ForEntity == entityName);
    }
        
    public async Task<string> GetObjectName(string entityName, int id)
    {
        var repo = GetRepository(entityName);
        
        if (!Objects.ContainsKey(entityName))
            Objects[entityName] = await repo.GetAllListAsync();

        return ((INamedEntity) Objects[entityName].Single(item => item.Id == id)).Name;
    }
}