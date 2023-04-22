using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies;

public interface IParserStrategy
{
    object ParseValue(PropertyInfo prop, object value);
        
    object ParseValue(EnumUtils.PropTypes? propType, object value);
}