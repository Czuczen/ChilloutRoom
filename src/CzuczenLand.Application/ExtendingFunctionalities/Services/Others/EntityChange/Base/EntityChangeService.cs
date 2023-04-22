using System.Linq;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.EntityChange.Base;

public class EntityChangeService : IEntityChangeService 
{
    private readonly IRepository<Abp.EntityHistory.EntityChange, long> _entityChange;

    
    public EntityChangeService(IRepository<Abp.EntityHistory.EntityChange, long> entityChange)
    {
        _entityChange = entityChange;
    }
        
    public Abp.EntityHistory.EntityChange GetLastEntityChangeForEntity(object entity)
    {
        var entityIdAsString = ((IEntity<int>) entity).Id.ToString();
        var entityTypeFullName = entity.GetType().FullName;
        var lastChange = _entityChange.GetAllList(item =>
                item.EntityId == entityIdAsString && item.EntityTypeFullName == entityTypeFullName)
            ?.OrderByDescending(item => item.ChangeTime).FirstOrDefault();

        return lastChange;
    }
}