using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.DeleteConnected;

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
    private readonly IDeleter _deleter;

    
    public DeleterActivityOperator(IDeleter deleter)
    {
        _deleter = deleter;
    }

    public async Task HandleEventAsync(EntityDeletedEventData<District> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    public async Task HandleEventAsync(EntityDeletedEventData<Drop> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    public async Task HandleEventAsync(EntityDeletedEventData<Requirement> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    public async Task HandleEventAsync(EntityDeletedEventData<GeneratedType> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    public async Task HandleEventAsync(EntityDeletedEventData<PlantationStorage> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    public async Task HandleEventAsync(EntityDeletedEventData<PlayerStorage> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    public async Task HandleEventAsync(EntityDeletedEventData<Quest> eventData) => await _deleter.DeleteConnected(eventData.Entity);
        
    public async Task HandleEventAsync(EntityDeletedEventData<User> eventData) => await _deleter.DeleteConnected(eventData.Entity);
}