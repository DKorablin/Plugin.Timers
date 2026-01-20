using System;
using System.ComponentModel;
using System.Diagnostics;
using Plugin.Timers.Settings;

namespace Plugin.Timers
{
	public class PluginSettings
	{
		private static readonly TimerSettingsItem StaticSettings = new TimerSettingsItem()
		{
			TimerName = "Default",
		};

		private readonly Plugin _plugin;
		private TimerSettingsCollection _timerData;
		private TimerSettingsItem _default = PluginSettings.StaticSettings;

		[Browsable(false)]
		public String TimerDataJson { get; set; }//WARN: Do not open it externally, as this will cause a desynchronization between TimerData and TimerDataJson.

		[Browsable(false)]
		public String DefaultJson { get; set; }//WARN: Do not open it externally, as this will cause a desynchronization between Default and DefaultJson.

		[Category("Data")]
		[Description("Default timer settings for new or unknown timers")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TimerSettingsItem Default
		{
			get
			{
				if(this._default == null)
				{
					this._default = String.IsNullOrEmpty(this.DefaultJson)
						? PluginSettings.StaticSettings
						: Serializer.JavaScriptDeserialize<TimerSettingsItem>(this.DefaultJson);

					this._default.PropertyChanged += this.Default_PropertyChanged;
				}
				return this._default;
			}
		}

		internal TimerSettingsCollection TimerData
			=> this._timerData ?? (this._timerData = new TimerSettingsCollection(this.TimerDataJson));

		internal PluginSettings(Plugin plugin)
			=> this._plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));

		internal TimerSettingsItem GetTimerSettings(String timerName)
		{
			if(String.IsNullOrEmpty(timerName))
				throw new ArgumentNullException(nameof(timerName));

			TimerSettingsItem result = this.TimerData[timerName];
			if(result == null)
			{
				result = new TimerSettingsItem(this.Default) { TimerName = timerName, };
				this._plugin.Trace.TraceEvent(TraceEventType.Verbose, 1, $"Timers -> Settings for timer '{timerName}' not found. Using default '{this.Default.TimerName}' timer");

				this.TimerData.Add(result);
				this.SaveSettings();
			}

			if(result.SingleInstance && !InstanceManager.CreateSingleInstance(timerName, null))
			{
				this._plugin.Trace.TraceEvent(TraceEventType.Stop, 1, "Timers -> Another timer with the same name '{0}' already running", timerName);
				return null;
			}
			return result;
		}

		private void Default_PropertyChanged(Object sender, PropertyChangedEventArgs e)
			=> this._plugin.Host.Plugins.Settings(this._plugin).SaveAssemblyParameters();

		internal void SaveSettings()
		{
			this.TimerDataJson = this.TimerData.ToJson();
			this._plugin.Host.Plugins.Settings(this._plugin).SaveAssemblyParameters();
		}
	}
}