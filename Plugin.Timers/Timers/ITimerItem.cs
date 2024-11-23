using System;

namespace Plugin.Timers
{
	/// <summary>Timer interface marker</summary>
	internal interface ITimerItem
	{
		/// <summary>Name of the timer for which the settings where obtained</summary>
		String TimerName { get; }

		/// <summary>Method to invoke callback method manually</summary>
		void InvokeCallback();

		/// <summary>Stop timer</summary>
		void Stop();
	}
}