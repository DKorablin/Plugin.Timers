using System;
using System.Threading;

namespace Plugin.Timers.Settings
{
	/// <summary>Application Instance Manager</summary>
	public static class InstanceManager
	{
		private static Mutex _mutex;

		/// <summary>Creates the single instance.</summary>
		/// <param name="timerName">The name.</param>
		/// <param name="callback">The callback.</param>
		/// <returns></returns>
		public static Boolean CreateSingleInstance(String timerName, EventHandler<InstanceCallbackEventArgs> callback)
		{
			if(String.IsNullOrEmpty(timerName))
				throw new ArgumentNullException(nameof(timerName));

			String mutexName = $"Global\\{Environment.MachineName}-{timerName}";

			try
			{
				// Try to create a new mutex
				_mutex = new Mutex(true, mutexName, out Boolean createdNew);

				if(createdNew)
				{
					// This is the first instance
					InstanceProxy.IsFirstInstance = true;
					InstanceProxy.TimerName = timerName;

					// Invoke callback if provided
					if(callback != null)
					{
						callback(null, new InstanceCallbackEventArgs(true, timerName));
					}

					return true;
				}
				else
				{
					// Another instance already exists
					InstanceProxy.IsFirstInstance = false;
					InstanceProxy.TimerName = timerName;

					// Try to signal the existing instance (optional)
					if(callback != null)
					{
						callback(null, new InstanceCallbackEventArgs(false, timerName));
					}

					// Release the mutex as we're not the first instance
					_mutex?.Dispose();
					_mutex = null;

					return false;
				}
			}
			catch(UnauthorizedAccessException)
			{
				// Mutex exists but we don't have access
				InstanceProxy.IsFirstInstance = false;
				InstanceProxy.TimerName = timerName;
				return false;
			}
			catch(Exception)
			{
				// On any error, assume we're not the first instance to be safe
				InstanceProxy.IsFirstInstance = false;
				InstanceProxy.TimerName = timerName;
				_mutex?.Dispose();
				_mutex = null;
				return false;
			}
		}

		/// <summary>Releases the mutex for the current instance.</summary>
		public static void ReleaseMutex()
		{
			try
			{
				_mutex?.ReleaseMutex();
				_mutex?.Dispose();
				_mutex = null;
			}
			catch
			{
				// Ignore errors during cleanup
			}
		}
	}
}