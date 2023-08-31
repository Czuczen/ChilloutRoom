using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;

/// <summary>
/// Klasa odpowiedzialna za przetwarzanie danych za pomocą określonej strategii.
/// </summary>
public class Parser : IParser
{
    /// <summary>
    /// Strategia przetwarzania, którą ma wykorzystać parser.
    /// </summary>
    private readonly IParserStrategy _parserStrategy;
            
    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="parserStrategy">Strategia przetwarzania, którą ma wykorzystać parser.</param>
    public Parser(IParserStrategy parserStrategy)
    {
        _parserStrategy = parserStrategy;
    }

    /// <summary>
    /// Przetwarza wartość określonej właściwości za pomocą zastosowanej strategii.
    /// </summary>
    /// <param name="prop">Właściwość, którą należy przetworzyć.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość.</returns>
    public object Parse(PropertyInfo prop, object value)
    {
        return _parserStrategy.ParseValue(prop, value);
    }

    /// <summary>
    /// Przetwarza wartość określonej właściwości poprzez klucz za pomocą zastosowanej strategii dla podanego typu.
    /// </summary>
    /// <typeparam name="T">Typ, dla którego ma być przetworzona wartość.</typeparam>
    /// <param name="key">Klucz właściwości do przetworzenia.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość.</returns>
    public object Parse<T>(string key, object value) where T : new()
    {
        var prop = typeof(T).GetProperty(key);
        
        return _parserStrategy.ParseValue(prop, value);
    }
        
    /// <summary>
    /// Przetwarza wartość określonej właściwości poprzez klucz za pomocą zastosowanej strategii dla podanego typu.
    /// </summary>
    /// <param name="type">Typ, dla którego ma być przetworzona wartość.</param>
    /// <param name="key">Klucz właściwości do przetworzenia.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość.</returns>
    public object Parse(Type type, string key, object value)
    {
        var prop = type.GetProperty(key);
        
        return _parserStrategy.ParseValue(prop, value);
    }

    /// <summary>
    /// Przetwarza słownik zawierający wartości za pomocą zastosowanej strategii na podstawie listy właściwości.
    /// </summary>
    /// <param name="dictionary">Słownik zawierający wartości do przetworzenia.</param>
    /// <param name="propertyInfos">Lista informacji o właściwościach.</param>
    /// <returns>Przetworzony słownik zawierający przetworzone wartości.</returns>
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
        
    /// <summary>
    /// Przetwarza listę obiektów za pomocą zastosowanej strategii.
    /// </summary>
    /// <param name="listObjects">Lista obiektów do przetworzenia.</param>
    /// <returns>Lista przetworzonych obiektów.</returns>
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
        
    /// <summary>
    /// Przetwarza listę wartości za pomocą zastosowanej strategii na podstawie listy właściwości.
    /// </summary>
    /// <param name="values">Lista wartości do przetworzenia.</param>
    /// <param name="properties">Lista informacji o właściwościach.</param>
    /// <returns>Lista przetworzonych wartości.</returns>
    public List<object> Parse(List<object> values, List<PropertyInfo> properties)
    {
        var ret = new List<object>();
        
        if (properties?.Count > 0 && values?.Count > 0)
            ret.AddRange(properties.Select((prop, i) => Parse(prop, values[i])));

        return ret;
    }
}