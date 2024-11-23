using System;
using System.Diagnostics;
using Plugin.Timers.Settings;

namespace Plugin.Timers
{
	/// <summary>Create windows timer that is optimized for use in Windows Forms applications</summary>
	internal class WindowsTimerItem : TimerItemBase<System.Windows.Forms.Timer>, ITimerItem
	{
		/// <summary>Create instance of <see cref="System.Windows.Forms.Timer"/></summary>
		/// <param name="trace">Trace source to store plugins exceptions</param>
		/// <param name="settings">Настройки таймеров</param>
		/// <param name="state">Указатель, который будет передан при срабатывании таймера</param>
		/// <param name="callback">Указатель на метод обратного вызова</param>
		public WindowsTimerItem(TraceSource trace, TimerSettingsItem settings, Object state, EventHandler<EventArgs> callback)
			: base(trace, settings, state, callback)
		{
			base.Timer = new System.Windows.Forms.Timer();
			base.Timer.Tick += this.Timer_InvokeTimer;
			base.Timer.Interval = (Int32)base.StartInterval;
			base.Timer.Start();
		}

		private void Timer_InvokeTimer(Object sender, EventArgs e)
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
					base.Timer.Interval = (Int32)interval;
					base.Timer.Start();
				}
			}
		}
	}
}