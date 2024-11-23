using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace Plugin.Timers.Settings
{
	/// <summary>Timer settings</summary>
	public class TimerSettingsItem : INotifyPropertyChanged
	{
		#region Fields
		private String _timerName;
		private String _description;
		private TimeSpan _startInterval = new TimeSpan(0, 0, 1);
		private TimeSpan _interval = new TimeSpan(0, 0, 60);
		private TimerType _timerType = TimerType.TimersTimer;
		private Boolean _singleInstance = false;
		private Int32 _workDays = 127;
		private WorkHours _workHours = new WorkHours();
		private WorkHours _sleepHours = new WorkHours();
		#endregion Fields

		/// <summary>The name of the timer that can be used by a third-party plugin to bind to the timer settings</summary>
		[Category("Details")]
		[DisplayName("Timer Name")]
		[DefaultValue("Default")]
		[Description("The name of the timer that can be used by a third-party plugin to bind to the timer settings")]
		public String TimerName
		{
			get => this._timerName;
			set => this.SetField(ref this._timerName, FixName(value),  nameof(this.TimerName));
		}

		[Category("Details")]
		[DisplayName("Description")]
		[Description("Brief description of the timer")]
		public String Description
		{
			get => this._description;
			set => this.SetField(ref this._description, value, nameof(this.Description));
		}

		/// <summary>Timer type</summary>
		[DisplayName("Timer Type")]
		[Description("Threading.Timer / Timers.Timer / Windows.Forms.Timer")]
		[DefaultValue(TimerType.TimersTimer)]
		public TimerType Type
		{
			get =>  this._timerType;
			set => this.SetField(ref this._timerType, value, nameof(this.Type));
		}

		/// <summary>We launch this timer in a single copy</summary>
		[DisplayName("Single Instance")]
		[Description("The timer can only be started in one instance")]
		[DefaultValue(false)]
		public Boolean SingleInstance
		{
			get => this._singleInstance;
			set => this.SetField(ref this._singleInstance, value, nameof(this.SingleInstance));
		}

		/// <summary>Timer first trigger interval</summary>
		[Category("Interval")]
		[DisplayName("Start Interval (sec)")]
		[Description("Timer first trigger interval")]
		[DefaultValue(1)]
		[Editor(typeof(TimeSpanEditor), typeof(UITypeEditor))]
		//[TypeConverter(typeof(ExpandableObjectConverter))]
		public TimeSpan StartInterval
		{
			get => this._startInterval;
			set
			{
				if(value.Ticks > 0)
					this.SetField(ref this._startInterval, value, nameof(this.StartInterval));
			}
		}

		/// <summary>Timer interval in seconds</summary>
		[Category("Interval")]
		[DisplayName("Interval")]
		[Description("Timer interval in seconds")]
		[Editor(typeof(TimeSpanEditor), typeof(UITypeEditor))]
		//[TypeConverter(typeof(ExpandableObjectConverter))]
		public TimeSpan Interval
		{
			get => this._interval;
			set
			{
				if(value.Ticks > 0)
					this.SetField(ref this._interval, value, nameof(this.Interval));
			}
		}

		[Category("Interval")]
		[DisplayName("Work Days")]
		[Description("Timer triggers only on certain days of the week")]
		[Editor(typeof(ColumnEditor<DayOfWeek>), typeof(UITypeEditor))]
		[DefaultValue(127)]
		public Int32 WorkDays
		{
			get => this._workDays;
			set
			{
				this.SetField(ref this._workDays, value <= 0 || value > 127 ? 127 : value, nameof(this.WorkDays));
			}
		}

		[Category("Interval")]
		[DisplayName("Work Hours")]
		[Description("The timer only works at a certain time")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public WorkHours WorkHours
		{
			get => this._workHours;
			set => this.SetField(ref this._workHours, value ?? new WorkHours(), nameof(this.WorkHours));
		}

		[Category("Interval")]
		[DisplayName("Sleep Hours")]
		[Description("Turning off the timer at a fixed time")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public WorkHours SleepHours
		{
			get => this._sleepHours;
			set => this.SetField(ref this._sleepHours, value ?? new WorkHours(), nameof(this.SleepHours));
		}

		/// <summary>Now is a working day for the timer</summary>
		[Category("Interval")]
		[DefaultValue(true)]
		public Boolean IsWorkDay => Utils.IsBitSet(this.WorkDays, (Int32)DateTime.Now.DayOfWeek);

		/// <summary>Now - working time of the timer, taking into account the silence time</summary>
		[Category("Interval")]
		[DefaultValue(true)]
		public Boolean IsWorkTime => this.WorkHours.IsWorkTime && !this.IsSleepHours;

		/// <summary>Now is the time for silence</summary>
		[Category("Interval")]
		[DefaultValue(false)]
		public Boolean IsSleepHours => this.SleepHours.Code != 0 && this.SleepHours.IsWorkTime;

		/// <summary>Timer interval</summary>
		[Category("Interval")]
		public Double WorkInterval
		{
			get
			{
				if(this.IsWorkTime)
					return this.Interval.TotalMilliseconds;
				else if(this.IsSleepHours)//TODO: Add an algorithm for calculating the next trigger time
					return this.Interval.TotalMilliseconds;
				else
				{
					DateTime now = DateTime.Now;
					DateTime today = now.Date;
					TimeSpan fromMorning = now - today;

					if(fromMorning < this.WorkHours.StartTimeSpan)
						return (today.Add(this.WorkHours.StartTimeSpan) - now).TotalMilliseconds;
					else if(fromMorning >= this.WorkHours.EndTimeSpan)
					{
						for(Int32 loop = 1; loop < 6; loop++)
						{
							DateTime nextDay = today.AddDays(loop);
							if(Utils.IsBitSet(this.WorkDays, (Int32)nextDay.DayOfWeek))
								return (nextDay.Add(this.WorkHours.StartTimeSpan) - now).TotalMilliseconds;
						}
					}

					//TODO: Failed to calculate next timer time (Here you also need to take into account SleepHours)
					return this.Interval.TotalMilliseconds;
				}
			}
		}

		public TimerSettingsItem() { }

		public TimerSettingsItem(TimerSettingsItem original)
		{
			this.Interval = original.Interval;
			this.TimerName = original.TimerName;
			this.SingleInstance = original.SingleInstance;
			this.Type = original.Type;
			this.WorkDays = original.WorkDays;
			this.WorkHours = original.WorkHours;
		}

		public override Int32 GetHashCode()
			=> this.TimerName.GetHashCode() ^ this.Interval.GetHashCode();

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private Boolean SetField<T>(ref T field, T value, String propertyName)
		{
			if(EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
		#endregion INotifyPropertyChanged

		internal static String FixName(String value)
		{
			if(value == null)
				return null;

			for(Int32 loop = value.Length - 1; loop >= 0; loop--)
				if(!Char.IsLetterOrDigit(value[loop]))
					value = value.Remove(loop, 1);

			value = value.Trim();
			return value.Length == 0 ? null : value;
		}
	}
}