using System;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.General;

public class DecimalFormatConverter : JsonConverter
{
    public override bool CanRead => false;
        
    public override bool CanConvert(Type objectType) => objectType == typeof(decimal);

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
        writer.WriteValue(decimal.Parse(decimal.Parse(value.ToString()).ToString("0.##")));
        
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => 
        throw new NotImplementedException();
}