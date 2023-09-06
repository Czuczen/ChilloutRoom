using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;

/// <summary>
/// Serwis podstawowy obsługujący logikę ignorowania zmian dla encji.
/// </summary>
public class IgnoreChangeService : IIgnoreChangeService
{
    /// <summary>
    /// Repozytorium ignorowania zmian.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.IgnoreChange> _ignoreChangeRepository;


    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="ignoreChangeRepository">Repozytorium ignorowania zmian.</param>
    public IgnoreChangeService(IRepository<ExtendingModels.Models.General.IgnoreChange> ignoreChangeRepository)
    {
        _ignoreChangeRepository = ignoreChangeRepository;
    }

    /// <summary>
    /// Dodaje encję do ignorowanych zmian.
    /// </summary>
    /// <param name="entity">Encja do dodania.</param>
    public async Task Add(Product entity)
    {
        var entityName = entity.GetType().Name;
        var guid = Guid.NewGuid();
        
        entity.IgnoreChangeGuid = guid;
            
        await _ignoreChangeRepository.InsertAsync(new ExtendingModels.Models.General.IgnoreChange
            {EntityId = entity.Id, EntityName = entityName, IgnoreChangeGuid = guid});
    }

    /// <summary>
    /// Usuwa encję z ignorowanych zmian.
    /// </summary>
    /// <param name="ignoreChangeId">Identyfikator ignorowanej zmiany dla encji.</param>
    public void Remove(int ignoreChangeId)
    {
        _ignoreChangeRepository.Delete(ignoreChangeId);
    }

    /// <summary>
    /// Pobiera ignorowaną zmianę dla encji.
    /// Czyli jeśli rekord ignorowania zostanie znaleziony to bieżąca zmiana na encji nie ma być analizowana przy aktualizacji stanu plantacji.
    /// </summary>
    /// <param name="entity">Zaktualizowana encja.</param>
    /// <returns>Ignorowana zmiana dla encji.</returns>
    public ExtendingModels.Models.General.IgnoreChange GetIgnoreChangeForUpdatedEntity(Product entity)
    {
        var entityName = entity.GetType().Name;
        return _ignoreChangeRepository.FirstOrDefault(item =>
            item.EntityId == entity.Id && item.EntityName == entityName && item.IgnoreChangeGuid == entity.IgnoreChangeGuid);
    }
}