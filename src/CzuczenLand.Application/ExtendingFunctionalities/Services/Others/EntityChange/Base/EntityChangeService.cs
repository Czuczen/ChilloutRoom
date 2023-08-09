using System.Linq;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.EntityChange.Base;

/// <summary>
/// Serwis obsługujący zmiany encji.
/// Implementuje interfejs IEntityChangeService.
/// </summary>
public class EntityChangeService : IEntityChangeService 
{
    /// <summary>
    /// Repozytorium historii zmian encji.
    /// </summary>
    private readonly IRepository<Abp.EntityHistory.EntityChange, long> _entityChange;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="entityChange">Repozytorium historii zmian encji.</param>
    public EntityChangeService(IRepository<Abp.EntityHistory.EntityChange, long> entityChange)
    {
        _entityChange = entityChange;
    }
        
    /// <summary>
    /// Pobiera ostatnią zmianę dla danej encji.
    /// </summary>
    /// <param name="entity">Obiekt encji.</param>
    /// <returns>Ostatnia zmiana encji.</returns>
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