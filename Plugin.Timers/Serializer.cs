using System;
using Newtonsoft.Json;
using Plugin.Timers.Settings;

namespace Plugin.Timers
{
	internal static class Serializer
	{
		private static JsonSerializerSettings _settings;

		private static JsonSerializerSettings Settings
		{
			get
			{
				if(_settings == null)
				{
					_settings = new JsonSerializerSettings();
					_settings.Converters.Add(new TimeSpanJsonConverter());
					_settings.Converters.Add(new WorkHoursJsonConverter());
				}
				return _settings;
			}
		}

		/// <summary>Deserialize a string into an object</summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="json">String in JSON format</param>
		/// <returns>Deserialized object</returns>
		internal static T JavaScriptDeserialize<T>(String json)
			=> String.IsNullOrEmpty(json)
				? default
				: JsonConvert.DeserializeObject<T>(json, Serializer.Settings);

		/// <summary>Serialize object</summary>
		/// <param name="item">Object to serialize</param>
		/// <returns>String in JSON format</returns>
		internal static String JavaScriptSerialize(Object item)
			=> item == null
				? null
				: JsonConvert.SerializeObject(item, Serializer.Settings);
	}
}