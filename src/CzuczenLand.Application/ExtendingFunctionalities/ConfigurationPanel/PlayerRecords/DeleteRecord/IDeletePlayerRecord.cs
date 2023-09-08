using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;

/// <summary>
/// Interfejs dla usuwania rekordów graczy generowanych na podstawie definicji.
/// Musi mieć TEntity bo inaczej nie wie dla jakiego typu
/// </summary>
/// <typeparam name="TEntity">Typ encji, który ma być usuwany.</typeparam>
public interface IDeletePlayerRecord<TEntity>
    where TEntity : IEntity<int>
{
    /// <summary>
    /// Usuwa rekordy graczy na podstawie usuniętej definicji.
    /// </summary>
    /// <param name="objectId">Identyfikator usuwanej definicji.</param>
    Task Delete(int objectId);

    /// <summary>
    /// Usuwa rekordy graczy na podstawie usuwanych definicji.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów usuwanych definicji.</param>
    Task Delete(List<int> ids);
}