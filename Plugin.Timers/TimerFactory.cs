using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plugin.Timers.Settings;

namespace Plugin.Timers
{
	/// <summary>Timer factory to store all timers</summary>
	internal class TimerFactory : IDisposable
	{
		private readonly Dictionary<String, ITimerItem> _storage = new Dictionary<String, ITimerItem>();

		/// <summary>Add new timer to storage</summary>
		/// <param name="trace">Trace object instance related to timer</param>
		/// <param name="key">Timer key</param>
		/// <param name="callback">Callback method called when the timer fires</param>
		/// <param name="state">The object reference passed to the callback method</param>
		/// <param name="settingsItem">The timer settings instance</param>
		/// <exception cref="NotImplementedException">Unknown timer type specified</exception>
		public void AddTimer(TraceSource trace, String key, EventHandler<EventArgs> callback, Object state, TimerSettingsItem settingsItem)
		{
			ITimerItem timerState;
			switch(settingsItem.Type)
			{
			case TimerType.ThreadingTimer:
				timerState = new ThreadingTimerItem(trace, settingsItem, state, callback);
				break;
			case TimerType.TimersTimer:
				timerState = new TimersTimerItem(trace, settingsItem, state, callback);
				break;
			case TimerType.WindowsTimer:
				timerState = new WindowsTimerItem(trace, settingsItem, state, callback);
				break;
			default:
				throw new NotImplementedException($"Unknown timer {settingsItem.Type}");
			}

			this._storage.Add(key, timerState);
		}

		/// <summary>Gets a list of all running timers that belongs to timer name</summary>
		/// <param name="timerName">Name of the timer by which to get a list of running timers</param>
		/// <returns>List of running timers</returns>
		public IEnumerable<ITimerItem> GetTimers(String timerName)
		{
			foreach(var item in this._storage)
				if(item.Value.TimerName == timerName)
					yield return item.Value;
		}

		/// <summary>Remove running timer my timer key</summary>
		/// <param name="key">Timer key</param>
		/// <returns>Timer found, removed and stopped</returns>
		public Boolean RemoveTimer(String key)
		{
			if(this._storage.TryGetValue(key, out ITimerItem item))
			{
				this._storage.Remove(key);
				item.Stop();
				return true;
			} else
				return false;
		}

		/// <summary>Invoke timer callback manually</summary>
		/// <param name="key">Timer key</param>
		/// <returns>Timer found and invoked</returns>
		public Boolean InvokeTimer(String key)
		{
			if(this._storage.TryGetValue(key, out ITimerItem item))
			{
				item.InvokeCallback();
				return true;
			} else
				return false;
		}

		/// <summary>Stop all running timers and remove them from storage</summary>
		public void Dispose()
		{
			foreach(KeyValuePair<String, ITimerItem> item in this._storage)
				item.Value.Stop();

			this._storage.Clear();
		}
	}
}