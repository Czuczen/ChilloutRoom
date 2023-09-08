using System;
using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Utils;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;

/// <summary>
/// Klasa pomocnicza dla asynchronicznych serwisów CRUD.
/// </summary>
public static class AsyncCrudHelper
{
    /// <summary>
    /// Lista generycznych interfejsów.
    /// Potrzebna do rejestracji w iniekcji zależności.
    /// </summary>
    public static readonly List<Type> GenericInterfaces = new()
    {
        typeof(ICreatePlayerRecord<>),
        typeof(IUpdatePlayerRecord<>),
        typeof(IDeletePlayerRecord<>),
        typeof(IResponseBuilder<>),
    };
    
    /// <summary>
    /// Aktualizuje obiekt na podstawie pól do zaktualizowania.
    /// Jednostka pracy zapisze zmiany robione przez currProp?.SetValue
    /// </summary>
    /// <typeparam name="TUpdate">Typ obiektu do aktualizacji.</typeparam>
    /// <param name="fieldsToUpdate">Pola do zaktualizowania.</param>
    /// <param name="destinationObject">Obiekt docelowy do aktualizacji.</param>
    /// <returns>Zaktualizowany obiekt docelowy.</returns>
    public static object UpdateObject<TUpdate>(object fieldsToUpdate, object destinationObject)
    {
        if (fieldsToUpdate == null || destinationObject == null) 
            return destinationObject;
            
        var objectType = destinationObject.GetType();
        var properties = objectType.GetProperties().ToList();
        var propertiesToUpdate = typeof(TUpdate).GetProperties().Select(item => item.Name).ToList(); // TUpdate/TUpdatePlayerRecordDto typy mówiące, które pola można aktualizować
        var fieldsToUpdateAsDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(fieldsToUpdate));
        var withValidValues = ParserFactory.GetParser(EnumUtils.ParseStrategies.Db).Parse(fieldsToUpdateAsDict, properties);
            
        foreach (var field in withValidValues)
        {
            if (propertiesToUpdate.All(item => item != field.Key)) continue;
            
            var currProp = objectType.GetProperty(field.Key);
            currProp?.SetValue(destinationObject, field.Value);
        }

        return destinationObject;
    }
}