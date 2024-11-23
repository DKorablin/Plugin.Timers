﻿using System;
using System.Security;
using System.Security.Permissions;

namespace Plugin.Timers.Settings
{
	/// <summary>shared object for processes</summary>
	[Serializable]
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	internal class InstanceProxy : MarshalByRefObject
	{
		/// <summary>Gets a value indicating whether this instance is first instance.</summary>
		/// <value><c>true</c> if this instance is first instance; otherwise, <c>false</c>.</value>
		public static Boolean IsFirstInstance { get; internal set; }

		/// <summary>Gets the command line args.</summary>
		/// <value>The command line args.</value>
		public static String TimerName { get; internal set; }

		/// <summary>Sets the command line args.</summary>
		/// <param name="isFirstInstance">if set to <c>true</c> [is first instance].</param>
		/// <param name="commandLineArgs">The command line args.</param>
		public void SetCommandLineArgs(Boolean isFirstInstance, String timerName)
		{
			InstanceProxy.IsFirstInstance = isFirstInstance;
			InstanceProxy.TimerName = timerName;
		}
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
		/// <param name="commandLineArgs">The command line args.</param>
		internal InstanceCallbackEventArgs(Boolean isFirstInstance, String timerName)
		{
			this.IsFirstInstance = isFirstInstance;
			this.TimerName = timerName;
		}
	}
}
