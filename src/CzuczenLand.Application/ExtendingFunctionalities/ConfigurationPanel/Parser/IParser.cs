using System;
using System.Collections.Generic;
using System.Reflection;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;

/// <summary>
/// Interfejs definiujący operacje przetwarzania danych za pomocą zastosowanej strategii.
/// </summary>
public interface IParser
{
    /// <summary>
    /// Przetwarza wartość określonej właściwości za pomocą zastosowanej strategii.
    /// </summary>
    /// <param name="prop">Właściwość, którą należy przetworzyć.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość.</returns>
    object Parse(PropertyInfo prop, object value);
    
    /// <summary>
    /// Przetwarza wartość określonej właściwości poprzez klucz za pomocą zastosowanej strategii dla podanego typu.
    /// </summary>
    /// <typeparam name="T">Typ, dla którego ma być przetworzona wartość.</typeparam>
    /// <param name="key">Klucz właściwości do przetworzenia.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość.</returns>
    object Parse<T>(string key, object value) where T : new();

    /// <summary>
    /// Przetwarza wartość określonej właściwości poprzez klucz za pomocą zastosowanej strategii dla podanego typu.
    /// </summary>
    /// <param name="type">Typ, dla którego ma być przetworzona wartość.</param>
    /// <param name="key">Klucz właściwości do przetworzenia.</param>
    /// <param name="value">Wartość do przetworzenia.</param>
    /// <returns>Przetworzona wartość.</returns>
    object Parse(Type type, string key, object value);

    /// <summary>
    /// Przetwarza słownik zawierający wartości za pomocą zastosowanej strategii na podstawie listy właściwości.
    /// </summary>
    /// <param name="dictionary">Słownik zawierający wartości do przetworzenia.</param>
    /// <param name="propertyInfos">Lista informacji o właściwościach.</param>
    /// <returns>Przetworzony słownik zawierający przetworzone wartości.</returns>
    Dictionary<string, object> Parse(Dictionary<string, object> dictionary, List<PropertyInfo> propertyInfos);

    /// <summary>
    /// Przetwarza listę obiektów za pomocą zastosowanej strategii.
    /// </summary>
    /// <param name="listObjects">Lista obiektów do przetworzenia.</param>
    /// <returns>Lista przetworzonych obiektów.</returns>
    List<object> Parse(List<object> listObjects);

    /// <summary>
    /// Przetwarza listę wartości za pomocą zastosowanej strategii na podstawie listy właściwości.
    /// </summary>
    /// <param name="values">Lista wartości do przetworzenia.</param>
    /// <param name="properties">Lista informacji o właściwościach.</param>
    /// <returns>Lista przetworzonych wartości.</returns>
    List<object> Parse(List<object> values, List<PropertyInfo> properties);
}