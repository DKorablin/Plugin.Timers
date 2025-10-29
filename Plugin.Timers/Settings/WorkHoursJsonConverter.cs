using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Plugin.Timers.Settings
{
	public class WorkHoursJsonConverter : JavaScriptConverter
	{
		public override IEnumerable<Type> SupportedTypes => new[] { typeof(WorkHours), };

		public override Object Deserialize(IDictionary<String, Object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			Int64 unboxed;
			if(!dictionary.TryGetValue("Code", out Object value))
				unboxed = WorkHours.MaxWorkHours;
			else if(value is Int32 i)
				unboxed = i;
			else if(value is Int64 l)
				unboxed= l;
			else
				unboxed = WorkHours.MaxWorkHours;

			return new WorkHours(unboxed);
		}

		public override IDictionary<String, Object> Serialize(Object obj, JavaScriptSerializer serializer)
		{
			WorkHours wh = (WorkHours)obj;
			return new Dictionary<String, Object> { { "Code", wh.Code } };
		}
	}
}