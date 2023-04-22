using System;
using System.Collections.Generic;
using System.Reflection;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;

public interface IParser
{
    object Parse(PropertyInfo prop, object value);
    
    object Parse<T>(string key, object value) where T : new();

    object Parse(Type type, string key, object value);

    Dictionary<string, object> Parse(Dictionary<string, object> dictionary, List<PropertyInfo> propertyInfos);

    List<object> Parse(List<object> listObjects);

    List<object> Parse(List<object> values, List<PropertyInfo> properties);
}