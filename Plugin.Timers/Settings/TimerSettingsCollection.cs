using System;
using System.Collections.Generic;

namespace Plugin.Timers.Settings
{
	/// <summary>Timer settings storage</summary>
	internal class TimerSettingsCollection : IEnumerable<TimerSettingsItem>
	{
		/// <summary>Dictionary for settings</summary>
		private readonly Dictionary<String, TimerSettingsItem> _timerData = new Dictionary<String, TimerSettingsItem>();

		public TimerSettingsItem this[String timerName]
		{
			get
			{
				TimerSettingsItem result = this._timerData.TryGetValue(TimerSettingsItem.FixName(timerName), out result) ? result : null;
				return result;
			}
		}

		/// <summary>Change timer name to ensure uniqueness</summary>
		/// <param name="item">Timer for name change</param>
		/// <param name="newTimerName">new timer name</param>
		/// <returns>Timer name successfully changed or timer with newTimerName already exists</returns>
		public Boolean ChangeTimerName(TimerSettingsItem item, String newTimerName)
		{
			if(this.ContainsTimer(newTimerName))
				return false;

			this.Remove(item);
			item.TimerName = newTimerName;
			this.Add(item);
			return true;
		}

		/// <summary>Check that timer with same name already exists</summary>
		/// <param name="timerName">Timer name to check</param>
		/// <returns>Timer with same name already exists</returns>
		public Boolean ContainsTimer(String timerName)
			=> this._timerData.ContainsKey(timerName);

		/// <summary>Gets list of all registered timer names</summary>
		/// <returns>List of previously created timer keys</returns>
		public String[] GetTimersName()
			=> new List<String>(this._timerData.Keys).ToArray();

		/// <summary>Create instance of timers settings collection</summary>
		/// <param name="json">JSON with timer settings</param>
		public TimerSettingsCollection(String json)
		{
			if(!String.IsNullOrEmpty(json))
				foreach(TimerSettingsItem item in Serializer.JavaScriptDeserialize<TimerSettingsItem[]>(json))
					this.AddWithCheck(item);
		}

		public void AddWithCheck(TimerSettingsItem item)
		{
			item.TimerName = this.GetUniqueTimerName(item.TimerName);
			this.Add(item);
		}

		private String GetUniqueTimerName(String timerName)
		{
			UInt32 counter = 1;
			String name = timerName;
			while(this.ContainsTimer(name))//Create unique timer key based on timer settings and timer index
				name = $"{timerName} ({(counter++):N0})";

			return name;
		}

		public void Add(TimerSettingsItem item)
			=> this._timerData.Add(item.TimerName, item);

		public Boolean Remove(TimerSettingsItem item)
			=> this.RemoveByKey(item.TimerName);

		private Boolean RemoveByKey(String timerName)
			=> this._timerData.Remove(timerName);

		public String ToJson()
		{
			if(this._timerData != null && this._timerData.Count > 0)
			{
				TimerSettingsItem[] data = new TimerSettingsItem[this._timerData.Count];
				Int32 loop = 0;
				foreach(var item in this.ToArray())
					data[loop++] = item;
				return Serializer.JavaScriptSerialize(data);
			} else return null;
		}

		public IEnumerable<TimerSettingsItem> ToArray()
		{
			foreach(var item in this._timerData)
				yield return item.Value;
		}

		public IEnumerator<TimerSettingsItem> GetEnumerator()
		{
			foreach(var item in this._timerData)
				yield return item.Value;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			=> this.GetEnumerator();
	}
}