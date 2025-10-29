using System;
using System.Diagnostics;
using Plugin.Timers.Settings;

namespace Plugin.Timers
{
	/// <summary>Base class with general logic for all timers</summary>
	internal class TimerItemBase<T> where T : class, IDisposable
	{
		private readonly TraceSource _trace;
		private readonly TimerSettingsItem _settings;

		public String TimerName => this._settings.TimerName;

		/// <summary>Abstract timer class</summary>
		protected T Timer { get; set; }

		/// <summary>Work timer trigger interval</summary>
		protected Double WorkInterval => this._settings.WorkInterval;

		/// <summary>Start timer trigger interval on first launch</summary>
		protected Double StartInterval => this._settings.StartInterval.TotalMilliseconds;

		/// <summary>Pointer to plugin logic which will be passed on timer invocation</summary>
		protected Object State { get; set; }

		/// <summary>Pointer to callback method</summary>
		private EventHandler<EventArgs> Callback { get; set; }

		/// <summary>Create instance of base timer logic</summary>
		/// <param name="trace">Trace logic to store plugin exceptions</param>
		/// <param name="settings">Timer settings</param>
		/// <param name="state">Pointer to object that will be transferred to plugin on timer invocation</param>
		/// <param name="callback">Plugin method callback</param>
		protected TimerItemBase(TraceSource trace, TimerSettingsItem settings, Object state, EventHandler<EventArgs> callback)
		{
			this._trace = trace;
			this._settings = settings;

			this.State = state;
			this.Callback = callback;
		}

		/// <summary>Invoke callback method if it can be invoked in that day and in that hours</summary>
		public void InvokeCallback()
		{
			if(this._settings.IsWorkDay && this._settings.IsWorkTime)
				try
				{
					this.Callback.Invoke(this.State, EventArgs.Empty);
				} catch(Exception exc)
				{
					if(!Utils.IsFatal(exc))//Threading timers will omit exception and it will totally ignored (We need to test it with all timers maybe only specific timer omit exceptions logging)
						_trace.TraceData(TraceEventType.Error, 10, exc);

					throw;
				}
		}

		/// <summary>Stop timer and remove callback method reference</summary>
		public void Stop()
		{
			if(this.Timer != null)
			{
				this.Timer.Dispose();
				this.Timer = null;
			}
			this.Callback = null;
		}
	}
}