using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;

public interface IIgnoreChangeService : ITransientDependency
{
    Task Add(object entity);
        
    void Remove(int ignoreChangeId);
        
    ExtendingModels.Models.General.IgnoreChange GetIgnoreChangeForUpdatedEntity(Product entity);
}