using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.EntityChange.Base;

public interface IEntityChangeService : ITransientDependency
{
    Abp.EntityHistory.EntityChange GetLastEntityChangeForEntity(object entity);
}