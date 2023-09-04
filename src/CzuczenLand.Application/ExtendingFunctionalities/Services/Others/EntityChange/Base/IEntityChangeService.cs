using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.EntityChange.Base;

/// <summary>
/// Interfejs serwisu obsługującego zmiany encji.
/// Rozszerza interfejs ITransientDependency.
/// </summary>
public interface IEntityChangeService : ITransientDependency
{
    /// <summary>
    /// Pobiera ostatnią zmianę dla danej encji.
    /// </summary>
    /// <param name="entity">Obiekt encji.</param>
    /// <returns>Ostatnia zmiana encji.</returns>
    Abp.EntityHistory.EntityChange GetLastEntityChangeForEntity(object entity);
}