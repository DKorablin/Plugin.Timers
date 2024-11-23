using System;
using System.Diagnostics;
using Plugin.Timers.Settings;
using Plugin.Timers.UI;
using SAL.Flatbed;

namespace Plugin.Timers
{
	public class Plugin : IPlugin, IPluginSettings<PluginSettings>
	{
		private PluginSettings _settings;

		private TraceSource _trace;
		internal TraceSource Trace => this._trace ?? (this._trace = Plugin.CreateTraceSource<Plugin>());

		private TimerFactory _timers;
		internal TimerFactory Timers => this._timers ?? (this._timers = new TimerFactory());

		internal IHost Host { get; }

		/// <summary>Settings to interact with plugin</summary>
		public PluginSettings Settings
		{
			get
			{
				if(this._settings == null)
				{
					this._settings = new PluginSettings(this);
					this.Host.Plugins.Settings(this).LoadAssemblyParameters(this._settings);
				}
				return this._settings;
			}
		}

		Object IPluginSettings.Settings => this.Settings;

		public Plugin(IHost host)
			=> this.Host = host ?? throw new ArgumentNullException(nameof(host));

		/// <summary>Gets extended settings with custom UI that can be added to WinForms application</summary>
		/// <returns></returns>
		public Object GetPluginOptionsControl()
			=> new ConfigCtrl(this);

		/// <summary>Gets list of names of all registered timers</summary>
		/// <returns>List of names of registered timers</returns>
		public String[] GetTimersName()
			=> this.Settings.TimerData.GetTimersName();

		/// <summary>Check for timer existence by name</summary>
		/// <param name="timerName">Name of the timer</param>
		/// <returns>A timer with same name exists</returns>
		public Boolean IsTimerExists(String timerName)
		{
			if(String.IsNullOrEmpty(timerName))
				throw new ArgumentNullException(nameof(timerName), "You must specify timerName (for settings)");

			return this.Settings.TimerData.ContainsTimer(timerName);
		}

		/// <summary>Register timer for invocation</summary>
		/// <param name="key">Timer key that will be used as unique timer identifier</param>
		/// <param name="timerName">Name of the settings for the timer operation (Editable in the plugin settings)</param>
		/// <param name="callback">Callback method that will be invoked when timer fires</param>
		/// <param name="state">Object reference that will be pased to callback method</param>
		public void RegisterTimer(String key, String timerName, EventHandler<EventArgs> callback, Object state)
		{
			if(String.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key), "You must specify unique timer key");
			if(String.IsNullOrEmpty(timerName))
				throw new ArgumentNullException(nameof(timerName), "You must specify timerName (for settings)");

			_ = callback ?? throw new ArgumentNullException(nameof(callback), "Callback method is empty");

			TimerSettingsItem settingsItem = this.Settings.GetTimerSettings(timerName);
			if(settingsItem == null)
				this.Trace.TraceEvent(TraceEventType.Stop, 1, "Timer {0} with settings {1} can't be launched", key, timerName);
			else
			{
				this.Trace.TraceEvent(TraceEventType.Start, 1, "Launching timer {0} with settings {1}. Interval: {2}.", key, timerName, settingsItem.Interval.ToString());
				this.Timers.AddTimer(this._trace ,key, callback, state, settingsItem);
			}
		}

		/// <summary>Unrefister timer by name</summary>
		/// <param name="key">Unique timer identifier to stop and remove from active timers list</param>
		/// <returns>Timer stopped and removed</returns>
		public Boolean UnregisterTimer(String key)
		{
			if(String.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key));

			return this.Timers.RemoveTimer(key);
		}

		/// <summary>Manually call timer callback method that is called by the timer</summary>
		/// <param name="key">The unique key of the timer whose method should be called</param>
		/// <exception cref="ArgumentNullException"><c>key</c> can't be null or empty</exception>
		/// <exception cref="InvalidOperationException">Timer with this key not found</exception>
		public void InvokeTimer(String key)
		{
			if(String.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key));

			if(!this.Timers.InvokeTimer(key))
				throw new InvalidOperationException($"Timer {key} not found");
		}

		Boolean IPlugin.OnConnection(ConnectMode mode)
			=> true;

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			this.Timers.Dispose();
			return true;
		}

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}
	}
}