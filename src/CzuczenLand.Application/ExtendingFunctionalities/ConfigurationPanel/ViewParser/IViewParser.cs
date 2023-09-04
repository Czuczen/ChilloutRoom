using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;

/// <summary>
/// Interfejs definiujący przetwarzanie wartości obiektów, które będą wyświetlone użytkownikowi.
/// </summary>
public interface IViewParser : ITransientDependency
{
    /// <summary>
    /// Przetwarza listę obiektów DTO na listę słowników zawierających przetworzone wartości.
    /// </summary>
    /// <param name="dtoObjList">Lista obiektów DTO do przetworzenia.</param>
    /// <param name="properties">Lista informacji o właściwościach obiektów.</param>
    /// <returns>Lista przetworzonych obiektów jako słowniki.</returns>
    Task<List<object>> ParseObjectsValues(List<object> dtoObjList, List<PropertyInfo> properties);
}