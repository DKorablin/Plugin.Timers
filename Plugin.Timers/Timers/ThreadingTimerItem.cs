using System;
using System.Diagnostics;
using System.Threading;
using Plugin.Timers.Settings;

namespace Plugin.Timers
{
	/// <summary>Timer from <see cref="System.Threading.Timer"/></summary>
	internal class ThreadingTimerItem : TimerItemBase<System.Threading.Timer>, ITimerItem
	{
		/// <summary>Create timer instance <see cref="System.Threading.Timer"/></summary>
		/// <param name="trace">Trace logic to store plugin exceptions</param>
		/// <param name="settings">Timer settings</param>
		/// <param name="state">Object that will be passed to target plugin on timer invocation</param>
		/// <param name="callback">Callback method that will be invoked on plugin trigger</param>
		public ThreadingTimerItem(TraceSource trace, TimerSettingsItem settings, Object state, EventHandler<EventArgs> callback)
			: base(trace, settings, state, callback)
			=> base.Timer = new Timer(this.Timer_InvokeTimer, base.State, (Int32)base.StartInterval, Timeout.Infinite);

		private void Timer_InvokeTimer(Object state)
		{
			try
			{
				base.InvokeCallback();
			} finally
			{
				Double interval = base.WorkInterval;
				if(base.Timer != null && interval > 0)//Check for not null (!=null), because of timer remove risk
					base.Timer.Change((Int32)interval, Timeout.Infinite);
			}
		}
	}
}