using System;

namespace Plugin.Timers.Settings
{
	/// <summary>Shared state for instance management</summary>
	internal static class InstanceProxy
	{
		/// <summary>Gets a value indicating whether this instance is first instance.</summary>
		/// <value><c>true</c> if this instance is first instance; otherwise, <c>false</c>.</value>
		public static Boolean IsFirstInstance { get; internal set; }

		/// <summary>Gets the timer name.</summary>
		/// <value>The timer name.</value>
		public static String TimerName { get; internal set; }
	}

	/// <summary></summary>
	public class InstanceCallbackEventArgs : EventArgs
	{
		/// <summary>Gets a value indicating whether this instance is first instance.</summary>
		/// <value><c>true</c> if this instance is first instance; otherwise, <c>false</c>.</value>
		public Boolean IsFirstInstance { get; private set; }

		/// <summary>Gets or sets the command line args.</summary>
		/// <value>The command line args.</value>
		public String TimerName { get; private set; }

		/// <summary>Initializes a new instance of the <see cref="InstanceCallbackEventArgs"/> class.</summary>
		/// <param name="isFirstInstance">if set to <c>true</c> [is first instance].</param>
		/// <param name="timerName">The timer name.</param>
		internal InstanceCallbackEventArgs(Boolean isFirstInstance, String timerName)
		{
			this.IsFirstInstance = isFirstInstance;
			this.TimerName = timerName;
		}
	}
}
