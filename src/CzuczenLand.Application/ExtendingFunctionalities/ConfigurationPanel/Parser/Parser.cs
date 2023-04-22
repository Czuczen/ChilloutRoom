using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;

public class Parser : IParser
{
    private readonly IParserStrategy _parserStrategy;
            
    public Parser(IParserStrategy parserStrategy)
    {
        _parserStrategy = parserStrategy;
    }

    public object Parse(PropertyInfo prop, object value)
    {
        return _parserStrategy.ParseValue(prop, value);
    }

    public object Parse<T>(string key, object value) where T : new()
    {
        var prop = typeof(T).GetProperty(key);
        
        return _parserStrategy.ParseValue(prop, value);
    }
        
    public object Parse(Type type, string key, object value)
    {
        var prop = type.GetProperty(key);
        
        return _parserStrategy.ParseValue(prop, value);
    }

    public Dictionary<string, object> Parse(Dictionary<string, object> dictionary, List<PropertyInfo> propertyInfos)
    {
        var ret = new Dictionary<string, object>();
        if (dictionary?.Count > 0 && propertyInfos?.Count > 0)
        {
            foreach (var dictItem in dictionary.ToList())
            {
                var currProp = propertyInfos.Single(propItem => propItem.Name == dictItem.Key);
                ret[dictItem.Key] = Parse(currProp, dictItem.Value);
            }    
        }

        return ret;
    }
        
    public List<object> Parse(List<object> listObjects)
    {
        var ret = new List<Dictionary<string, object>>();
        
        if (listObjects?.Count > 0)
        {
            var properties = listObjects.First().GetType().GetProperties();
            foreach (var obj in listObjects)
            {
                var objDict = new Dictionary<string, object>();
                foreach (var prop in properties)
                    objDict[prop.Name] = Parse(prop, prop.GetValue(obj));
                
                ret.Add(objDict);
            }
        }

        return ret.Cast<object>().ToList();
    }
        
    public List<object> Parse(List<object> values, List<PropertyInfo> properties)
    {
        var ret = new List<object>();
        
        if (properties?.Count > 0 && values?.Count > 0)
            ret.AddRange(properties.Select((prop, i) => Parse(prop, values[i])));

        return ret;
    }
}