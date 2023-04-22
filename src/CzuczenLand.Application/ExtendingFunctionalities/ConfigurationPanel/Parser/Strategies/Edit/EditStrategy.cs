using System;
using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Edit;

public class EditStrategy : IParserStrategy
{
    public object ParseValue(PropertyInfo prop, object value)
    {
        if (prop != null && value == null && prop.PropertyType != typeof(string) && Nullable.GetUnderlyingType(prop.PropertyType) == null)
            throw new ArgumentNullException(nameof(value), "Argument nie może być wartością null.");
        
        return ParseValue(ParserHelper.GetPropType(prop), value);
    }

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
