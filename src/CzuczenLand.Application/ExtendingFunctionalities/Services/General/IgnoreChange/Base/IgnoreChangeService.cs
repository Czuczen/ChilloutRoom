using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;

public class IgnoreChangeService : IIgnoreChangeService
{
    private readonly IRepository<ExtendingModels.Models.General.IgnoreChange> _ignoreChangeRepository;

    
    public IgnoreChangeService(IRepository<ExtendingModels.Models.General.IgnoreChange> ignoreChangeRepository)
    {
        _ignoreChangeRepository = ignoreChangeRepository;
    }

    public async Task Add(object obj)
    {
        var entityName = obj.GetType().Name;
        var entity = (Product) obj;
        var guid = Guid.NewGuid();
        
        entity.IgnoreChangeGuid = guid;
            
        await _ignoreChangeRepository.InsertAsync(new ExtendingModels.Models.General.IgnoreChange
            {EntityId = entity.Id, EntityName = entityName, IgnoreChangeGuid = guid});
    }
        
    public void Remove(int ignoreChangeId)
    {
        _ignoreChangeRepository.Delete(ignoreChangeId);
    }
        
    public ExtendingModels.Models.General.IgnoreChange GetIgnoreChangeForUpdatedEntity(Product entity)
    {
        var entityName = entity.GetType().Name;
        return _ignoreChangeRepository.FirstOrDefault(item =>
            item.EntityId == entity.Id && item.EntityName == entityName && item.IgnoreChangeGuid == entity.IgnoreChangeGuid);
    }
}