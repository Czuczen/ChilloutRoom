using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;

/// <summary>
/// Interfejs służący do zarządzania ignorowaniem zmian w encjach.
/// </summary>
public interface IIgnoreChangeService : ITransientDependency
{
    /// <summary>
    /// Dodaje encję do ignorowanych zmian.
    /// </summary>
    /// <param name="entity">Encja do dodania.</param>
    Task Add(Product entity);
        
    /// <summary>
    /// Usuwa encję z ignorowanych zmian.
    /// </summary>
    /// <param name="ignoreChangeId">Identyfikator ignorowanej zmiany dla encji.</param>
    void Remove(int ignoreChangeId);
        
    /// <summary>
    /// Pobiera ignorowaną zmianę dla encji.
    /// Czyli jeśli rekord ignorowania zostanie znaleziony to bieżąca zmiana na encji nie ma być analizowana przy aktualizacji stanu plantacji.
    /// </summary>
    /// <param name="entity">Zaktualizowana encja.</param>
    /// <returns>Ignorowana zmiana dla encji.</returns>
    ExtendingModels.Models.General.IgnoreChange GetIgnoreChangeForUpdatedEntity(Product entity);
}