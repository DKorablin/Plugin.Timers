using System;
using System.Diagnostics;
using System.Timers;
using Plugin.Timers.Settings;

namespace Plugin.Timers
{
	/// <summary>Timer from <see cref="System.Timers.Timer"/></summary>
	internal class TimersTimerItem : TimerItemBase<System.Timers.Timer>, ITimerItem
	{
		/// <summary>Create instace of <see cref="System.Timers.Timer"/></summary>
		/// <param name="trace">Trace logic to store plugin exceptions</param>
		/// <param name="settings">Timer settings</param>
		/// <param name="state">Pointer to object that will be passed to plugin on timer invocation</param>
		/// <param name="callback">Pointer to method callback on timer invocation</param>
		public TimersTimerItem(TraceSource trace, TimerSettingsItem settings, Object state, EventHandler<EventArgs> callback)
			: base(trace, settings, state, callback)
		{
			base.Timer = new Timer(base.StartInterval);
			base.Timer.Elapsed += this.Timer_InvokeTimer;
			base.Timer.Start();
		}

		private void Timer_InvokeTimer(Object sender, ElapsedEventArgs e)
		{
			base.Timer.Stop();
			try
			{
				base.InvokeCallback();
			} finally
			{
				Double interval = base.WorkInterval;
				if(base.Timer != null && interval > 0)
				{//Check for not null (!=null), because of timer remove risk
					base.Timer.Interval = interval;
					base.Timer.Start();
				}
			}
		}
	}
}