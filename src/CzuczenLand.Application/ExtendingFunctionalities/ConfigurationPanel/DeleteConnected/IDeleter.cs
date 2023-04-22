using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.DeleteConnected;

public interface IDeleter : ITransientDependency
{
    Task DeleteConnected(District entity);
        
    Task DeleteConnected(Drop entity);
        
    Task DeleteConnected(Requirement entity);
        
    Task DeleteConnected(GeneratedType entity);
        
    Task DeleteConnected(PlantationStorage entity);
        
    Task DeleteConnected(PlayerStorage entity);
        
    Task DeleteConnected(Quest entity);
        
    Task DeleteConnected(User entity);
}