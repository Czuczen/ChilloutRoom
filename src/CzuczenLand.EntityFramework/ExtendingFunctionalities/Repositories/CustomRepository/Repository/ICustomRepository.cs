using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;

public interface ICustomRepository
{
    string ForEntity { get; }
    
    List<IEntity<int>> GetWhere(string key, int? value);
        
    List<IEntity<int>> GetWhere(string key, long? value);
        
    Task<List<IEntity<int>>> GetWhereAsync(string key, int? value);
        
    Task<List<IEntity<int>>> GetWhereAsync(string key, long? value);

    IEntity<int> Get(object id);

    Task<IEntity<int>> GetAsync(object id);
    
    List<IEntity<int>> GetAllList();
    
    Task<List<IEntity<int>>> GetAllListAsync();

    void Delete(object id);
        
    Task DeleteAsync(object id);

    IEntity<int> MapAndInsert(string serializedObj);
    
    Task<IEntity<int>> MapAndInsertAsync(string serializedObj);

    IEntity<int> MapAndUpdate(string serializedObj);
    
    Task<IEntity<int>> MapAndUpdateAsync(string serializedObj);
}
