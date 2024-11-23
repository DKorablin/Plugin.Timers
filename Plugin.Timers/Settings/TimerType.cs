using System;

namespace Plugin.Timers.Settings
{
	/// <summary>Тип таймера</summary>
	public enum TimerType
	{
		/// <summary>System.Timers.Timer</summary>
		TimersTimer,
		/// <summary>System.Threading.Timer</summary>
		ThreadingTimer,
		/// <summary>This timer is optimized for use in Windows Forms applications and must be used in a window.</summary>
		/// <remarks>System.Windows.Forms.Timer</remarks>
		WindowsTimer,
	}
}