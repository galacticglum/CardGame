using System;
using Newtonsoft.Json;
using UnityEngine;

public class ColorJsonConverter : JsonConverter
{
    public override bool CanRead => true;
    public override bool CanWrite => true;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Color color = (Color)value;
        string htmlColour = ColorUtility.ToHtmlStringRGBA(color);
        writer.WriteStartObject();

        writer.WritePropertyName("html_colour");
        writer.WriteValue(htmlColour);

        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        reader.Read();
        string htmlColour = "#" + reader.ReadAsString();

        Color result;
        if (!ColorUtility.TryParseHtmlString(htmlColour, out result))
        {
            result = Color.white;
        }
    
        // Read end object token. Finalize this conversion
        reader.Read();
        return result;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Color);
    }
}

