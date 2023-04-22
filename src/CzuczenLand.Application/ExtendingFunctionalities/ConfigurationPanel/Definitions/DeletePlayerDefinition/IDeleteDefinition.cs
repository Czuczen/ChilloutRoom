using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;

/// <summary>
/// Musi mieć TEntity bo inaczej nie wie dla jakiego typu
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IDeleteDefinition<TEntity>
    where TEntity : IEntity<int>
{
    Task Delete(int objectId);

    Task Delete(List<int> ids);
}