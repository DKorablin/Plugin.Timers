using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Plugin.Timers.Settings
{
	public class WorkHoursJsonConverter : JsonConverter<WorkHours>
	{
		public override WorkHours ReadJson(JsonReader reader, Type objectType, WorkHours existingValue, Boolean hasExistingValue, JsonSerializer serializer)
		{
			if(reader.TokenType == JsonToken.Null)
				return new WorkHours(WorkHours.MaxWorkHours);

			if(reader.TokenType == JsonToken.StartObject)
			{
				JObject obj = JObject.Load(reader);
				if(obj.TryGetValue("Code", out JToken value))
				{
					Int64 code;
					if(value.Type == JTokenType.Integer)
						code = value.Value<Int64>();
					else if(value.Type == JTokenType.String)
						code = Int64.TryParse(value.Value<String>(), out Int64 result) ? result : WorkHours.MaxWorkHours;
					else
						code = WorkHours.MaxWorkHours;
					
					return new WorkHours(code);
				}
			}

			return new WorkHours(WorkHours.MaxWorkHours);
		}

		public override void WriteJson(JsonWriter writer, WorkHours value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("Code");
			writer.WriteValue(value.Code);
			writer.WriteEndObject();
		}
	}
}