using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Plugin.Timers.Settings
{
	public class TimeSpanJsonConverter : JavaScriptConverter
	{
		public override IEnumerable<Type> SupportedTypes => new[] { typeof(TimeSpan), };

		public override Object Deserialize(IDictionary<String, Object> dictionary, Type type, JavaScriptSerializer serializer)
			=> new TimeSpan(GetValue(dictionary, "ticks"));

		public override IDictionary<String, Object> Serialize(Object obj, JavaScriptSerializer serializer)
		{
			TimeSpan timeSpan = (TimeSpan)obj;

			Dictionary<String, Object> result = new Dictionary<String, Object>{ { "ticks", timeSpan.Ticks }, };

			return result;
		}

		private static Int64 GetValue(IDictionary<String, Object> dictionary, String key)
		{
			const Int64 DefaultValue = 0;

			if(!dictionary.TryGetValue(key, out Object value))
				return DefaultValue;
			else if(value is Int32 i)
				return i;
			else if(value is Int64 l)
				return l;
			else if(value is String s)
				return Int64.TryParse(s, out Int64 returnValue) ? returnValue : DefaultValue;
			else
				return DefaultValue;
		}
	}
}