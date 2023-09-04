using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.DeleteConnected;

/// <summary>
/// Operator obsługujący usuwanie powiązanych encji w reakcji na zdarzenia usuwania.
/// </summary>
public class DeleterActivityOperator :
    IAsyncEventHandler<EntityDeletedEventData<District>>,
    IAsyncEventHandler<EntityDeletedEventData<GeneratedType>>,
    IAsyncEventHandler<EntityDeletedEventData<PlantationStorage>>,
    IAsyncEventHandler<EntityDeletedEventData<PlayerStorage>>,
    IAsyncEventHandler<EntityDeletedEventData<Quest>>,
    IAsyncEventHandler<EntityDeletedEventData<Drop>>,
    IAsyncEventHandler<EntityDeletedEventData<Requirement>>,
    IAsyncEventHandler<EntityDeletedEventData<User>>,
    ITransientDependency
{
    /// <summary>
    /// Interfejs wykonujący operacje usuwania powiązanych encji.
    /// </summary>
    private readonly IDeleter _deleter;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="deleter">Interfejs wykonujący operacje usuwania powiązanych encji.</param>
    public DeleterActivityOperator(IDeleter deleter)
    {
        _deleter = deleter;
    }

    /// <summary>
    /// Obsługuje zdarzenie usuwania dzielnicy i wywołuje operację usuwania powiązanych encji.
    /// </summary>
    public async Task HandleEventAsync(EntityDeletedEventData<District> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    /// <summary>
    /// Obsługuje zdarzenie usuwania nagrody i wywołuje operację usuwania powiązanych encji.
    /// </summary>
    public async Task HandleEventAsync(EntityDeletedEventData<Drop> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    /// <summary>
    /// Obsługuje zdarzenie usuwania wymagania i wywołuje operację usuwania powiązanych encji.
    /// </summary>
    public async Task HandleEventAsync(EntityDeletedEventData<Requirement> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    /// <summary>
    /// Obsługuje zdarzenie usuwania typu generowanego i wywołuje operację usuwania powiązanych encji.
    /// </summary>
    public async Task HandleEventAsync(EntityDeletedEventData<GeneratedType> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    /// <summary>
    /// Obsługuje zdarzenie usuwania magazynu plantacji i wywołuje operację usuwania powiązanych encji.
    /// </summary>
    public async Task HandleEventAsync(EntityDeletedEventData<PlantationStorage> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    /// <summary>
    /// Obsługuje zdarzenie usuwania magazynu gracza i wywołuje operację usuwania powiązanych encji.
    /// </summary>
    public async Task HandleEventAsync(EntityDeletedEventData<PlayerStorage> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    /// <summary>
    /// Obsługuje zdarzenie usuwania zadania i wywołuje operację usuwania powiązanych encji.
    /// </summary>
    public async Task HandleEventAsync(EntityDeletedEventData<Quest> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    /// <summary>
    /// Obsługuje zdarzenie usuwania użytkownika i wywołuje operację usuwania powiązanych encji.
    /// </summary>
    public async Task HandleEventAsync(EntityDeletedEventData<User> eventData) => await _deleter.DeleteConnected(eventData.Entity);
}