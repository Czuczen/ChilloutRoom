using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.EntityFramework.Uow;
using CzuczenLand.EntityFramework;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;

public class CustomRepository<TEntity> : ICustomRepository
    where TEntity : class, IEntity<int>
{
    private string _forEntity;
    private readonly CustomRepositoryBase<TEntity, int> _repository;
    
    public string ForEntity => _forEntity ??= typeof(TEntity).Name;
    
    
    /// <summary>
    /// Klasa ta wymaga jednostki pracy (UnitOfWork) od klasy nadrzędnej 
    /// </summary>
    public CustomRepository()
    {
        _repository = new CustomRepositoryBase<TEntity, int>(
            new UnitOfWorkDbContextProvider<CzuczenLandDbContext>(
                new AsyncLocalCurrentUnitOfWorkProvider()));
    }

    public List<IEntity<int>> GetWhere(string key, int? value)
    {
        List<IEntity<int>> ret;
        var propToCompare = typeof(TEntity).GetProperty(key);

        if (propToCompare != null)
            ret = _repository.GetAllList().Where(item => (int?) propToCompare.GetValue(item) == value)
                .Cast<IEntity<int>>().ToList();
        else
            throw new AbpException("Pole " + key + " nie istnieje w encji " + ForEntity);

        return ret;
    }
        
    public List<IEntity<int>> GetWhere(string key, long? value)
    {
        List<IEntity<int>> ret;
        var propToCompare = typeof(TEntity).GetProperty(key);

        if (propToCompare != null)
            ret = _repository.GetAllList().Where(item => (long?) propToCompare.GetValue(item) == value)
                .Cast<IEntity<int>>().ToList();
        else
            throw new AbpException("Pole " + key + " nie istnieje w encji " + ForEntity);

        return ret;
    }
    
    public async Task<List<IEntity<int>>> GetWhereAsync(string key, int? value)
    {
        List<IEntity<int>> ret;
        var propToCompare = typeof(TEntity).GetProperty(key);

        if (propToCompare != null)
            ret = (await _repository.GetAllListAsync()).Where(item => (int?) propToCompare.GetValue(item) == value)
                .Cast<IEntity<int>>().ToList();
        else
            throw new AbpException("Pole " + key + " nie istnieje w encji " + ForEntity);

        return ret;
    }

    public async Task<List<IEntity<int>>> GetWhereAsync(string key, long? value)
    {
        List<IEntity<int>> ret;
        var propToCompare = typeof(TEntity).GetProperty(key);
        
        if (propToCompare != null)
            ret = (await _repository.GetAllListAsync()).Where(item => (long?) propToCompare.GetValue(item) == value)
                .Cast<IEntity<int>>().ToList();
        else
            throw new AbpException("Pole " + key + " nie istnieje w encji " + ForEntity);

        return ret;
    }
    
    public IEntity<int> Get(object id) => _repository.Get((int) id);

    public async Task<IEntity<int>> GetAsync(object id) => await _repository.GetAsync((int) id);
    
    public List<IEntity<int>> GetAllList() => _repository.GetAllList().Cast<IEntity<int>>().ToList();
    
    public async Task<List<IEntity<int>>> GetAllListAsync() => (await _repository.GetAllListAsync()).Cast<IEntity<int>>().ToList();
    
    public void Delete(object id) => _repository.Delete((int) id);

    public async Task DeleteAsync(object id) => await _repository.DeleteAsync((int) id);
    
    public IEntity<int> MapAndInsert(string serializedObj)
    {
        var mappedObject =  JsonConvert.DeserializeObject<TEntity>(serializedObj);
        var id = _repository.InsertAndGetId(mappedObject);
        var createdObject = _repository.Get(id);
    
        return createdObject;
    }
    
    public async Task<IEntity<int>> MapAndInsertAsync(string serializedObj)
    {
        var mappedObject =  JsonConvert.DeserializeObject<TEntity>(serializedObj);
        var id = await _repository.InsertAndGetIdAsync(mappedObject);
        var createdObject = await _repository.GetAsync(id);
    
        return createdObject;
    }
        
    public IEntity<int> MapAndUpdate(string serializedObj)
    {
        var mappedObject =  JsonConvert.DeserializeObject<TEntity>(serializedObj);
        var updatedObject = _repository.Update(mappedObject);
            
        return updatedObject;
    }
    
    public async Task<IEntity<int>> MapAndUpdateAsync(string serializedObj)
    {
        var mappedObject =  JsonConvert.DeserializeObject<TEntity>(serializedObj);
        var id = await _repository.InsertOrUpdateAndGetIdAsync(mappedObject);
        var updatedObject = await _repository.GetAsync(id);
    
        return updatedObject;
    }
}