using System;
using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Edit;

/// <summary>
/// Klasa odpowiedzialna za przetwarzanie wartości edytowanych.
/// </summary>
public class EditStrategy : IParserStrategy
{
    /// <summary>
    /// Przetwarza wartość właściwości na podstawie jej typu.
    /// </summary>
    /// <param name="prop">Właściwość do przetworzenia.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość edytowana.</returns>
    public object ParseValue(PropertyInfo prop, object value)
    {
        if (prop != null && value == null && prop.PropertyType != typeof(string) && Nullable.GetUnderlyingType(prop.PropertyType) == null)
            throw new ArgumentNullException(nameof(value), "Argument nie może być wartością null.");
        
        return ParseValue(ParserHelper.GetPropType(prop), value);
    }

    /// <summary>
    /// Przetwarza wartość na podstawie określonego typu właściwości.
    /// </summary>
    /// <param name="propType">Typ właściwości.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość edytowana.</returns>
    public object ParseValue(EnumUtils.PropTypes? propType, object value)
    {
        if (propType != null && (value == null || value.ToString() == "")) return "";
        
        switch (propType)
        {
            case EnumUtils.PropTypes.Int:
                return int.Parse(value.ToString()).ToString();
            case EnumUtils.PropTypes.Long:
                return long.Parse(value.ToString()).ToString();
            case EnumUtils.PropTypes.String:
                return value.ToString();
            case EnumUtils.PropTypes.Double:
                return double.Parse(value.ToString().Replace(".", ",")).ToString("0.###############").Replace(",", ".");
            case EnumUtils.PropTypes.Decimal:
                return decimal.Parse(value.ToString().Replace(".", ",")).ToString("F2").Replace(",", ".");
            case EnumUtils.PropTypes.Boolean:
                return bool.Parse(value.ToString()).ToString().ToLower();
            case EnumUtils.PropTypes.DateTime:
                return DateTime.Parse(value.ToString()).ToString("s");
            case EnumUtils.PropTypes.Guid:
                return Guid.Parse(value.ToString()).ToString();
            default:
                throw new ArgumentOutOfRangeException(propType.ToString(), value, "Ten typ nie jest obsługiwany.");
        }
    }
}
