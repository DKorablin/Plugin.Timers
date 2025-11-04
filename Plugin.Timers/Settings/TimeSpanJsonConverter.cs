using System;
using Newtonsoft.Json;

namespace Plugin.Timers.Settings
{
	public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
	{
		public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, Boolean hasExistingValue, JsonSerializer serializer)
		{
			if(reader.TokenType == JsonToken.Null)
				return TimeSpan.Zero;

			if(reader.TokenType == JsonToken.StartObject)
			{
				reader.Read(); // Move to property name
				if(reader.TokenType == JsonToken.PropertyName && (String)reader.Value == "ticks")
				{
					reader.Read(); // Move to value
					Int64 ticks = Convert.ToInt64(reader.Value);
					reader.Read(); // Move past EndObject
					return new TimeSpan(ticks);
				}
			}

			return TimeSpan.Zero;
		}

		public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("ticks");
			writer.WriteValue(value.Ticks);
			writer.WriteEndObject();
		}
	}
}