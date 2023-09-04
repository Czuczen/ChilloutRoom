using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;

/// <summary>
/// Interfejs dla ładowania opcji pól typu SelectList.
/// </summary>
public interface ISelectListLoader : ITransientDependency
{
    /// <summary>
    /// Pobiera opcje dla pól typu SelectList.
    /// </summary>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości obiektu.</param>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="isEdit">Czy edycja.</param>
    /// <param name="isAdmin">Czy administrator.</param>
    /// <returns>Słownik opcji pól typu SelectList.</returns>
    Task<Dictionary<string, object>> GetSelectFieldsOptions(List<PropertyInfo> properties, List<object> values,
        string entity, bool isEdit, bool isAdmin);
}