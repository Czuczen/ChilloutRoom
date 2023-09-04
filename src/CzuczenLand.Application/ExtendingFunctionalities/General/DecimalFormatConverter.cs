using System;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.General;

/// <summary>
/// Konwerter JSON dla formatowania liczb dziesiętnych.
/// </summary>
public class DecimalFormatConverter : JsonConverter
{
    /// <summary>
    /// Wskazuje, czy możliwe jest odczytanie wartości.
    /// </summary>
    public override bool CanRead => false;
        
    /// <summary>
    /// Wskazuje, czy możliwe jest przekształcenie typu.
    /// </summary>
    /// <param name="objectType">Typ obiektu do przekształcenia.</param>
    /// <returns>True, jeśli możliwe jest przekształcenie, w przeciwnym razie false.</returns>
    public override bool CanConvert(Type objectType) => objectType == typeof(decimal);

    /// <summary>
    /// Zapisuje wartość dziesiętną do formatu JSON.
    /// </summary>
    /// <param name="writer">Obiekt JsonWriter do zapisu.</param>
    /// <param name="value">Wartość do zapisania.</param>
    /// <param name="serializer">Serializer Json do użycia.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
        writer.WriteValue(decimal.Parse(decimal.Parse(value.ToString()).ToString("0.##")));
        
    /// <summary>
    /// Odczytuje wartość dziesiętną z formatu JSON. (Nie zaimplementowane)
    /// </summary>
    /// <param name="reader">Obiekt JsonReader do odczytu.</param>
    /// <param name="objectType">Oczekiwany typ obiektu.</param>
    /// <param name="existingValue">Istniejąca wartość obiektu.</param>
    /// <param name="serializer">Serializer Json do użycia.</param>
    /// <returns>Rzuca wyjątek NotImplementedException, ponieważ metoda nie jest zaimplementowana.</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => 
        throw new NotImplementedException();
}