using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies;

/// <summary>
/// Interfejs definiujący operacje przetwarzania wartości właściwości zgodnie z określoną strategią.
/// </summary>
public interface IParserStrategy
{
    /// <summary>
    /// Przetwarza wartość właściwości na podstawie jej typu.
    /// </summary>
    /// <param name="prop">Właściwość do przetworzenia.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość edytowana.</returns>
    object ParseValue(PropertyInfo prop, object value);
        
    /// <summary>
    /// Przetwarza wartość na podstawie określonego typu właściwości.
    /// </summary>
    /// <param name="propType">Typ właściwości.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość edytowana.</returns>
    object ParseValue(EnumUtils.PropTypes? propType, object value);
}