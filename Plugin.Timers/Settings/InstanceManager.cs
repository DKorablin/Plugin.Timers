using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;

namespace Plugin.Timers.Settings
{
	/// <summary>Application Instance Manager</summary>
	public static class InstanceManager
	{
		/// <summary>Creates the single instance.</summary>
		/// <param name="timerName">The name.</param>
		/// <param name="callback">The callback.</param>
		/// <returns></returns>
		public static Boolean CreateSingleInstance(String timerName, EventHandler<InstanceCallbackEventArgs> callback)
		{
			EventWaitHandle eventWaitHandle = null;
			String eventName = String.Join("-", Environment.MachineName, timerName);

			InstanceProxy.IsFirstInstance = false;
			InstanceProxy.TimerName = timerName;

			try
			{
				// try opening existing wait handle
				eventWaitHandle = EventWaitHandle.OpenExisting(eventName);
			} catch(WaitHandleCannotBeOpenedException)
			{
				// got exception = handle wasn't created yet
				InstanceProxy.IsFirstInstance = true;
			}

			if(InstanceProxy.IsFirstInstance)
			{
				// init handle
				eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, eventName);

				// register wait handle for this instance (process)
				ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, WaitOrTimerCallback, callback, Timeout.Infinite, false);
				eventWaitHandle.Close();

				try
				{
					// register shared type (used to pass data between processes)
					InstanceManager.RegisterRemoteType(timerName);
				} catch(RemotingException)
				{//Channel already registered
					InstanceProxy.IsFirstInstance = false;
				}
			} else
			{
				// pass console arguments to shared object
				InstanceManager.UpdateRemoteObject(timerName);

				// invoke (signal) wait handle on other process
				if(eventWaitHandle != null)
					eventWaitHandle.Set();
			}

			return InstanceProxy.IsFirstInstance;
		}

		/// <summary>Updates the remote object.</summary>
		/// <param name="uri">The remote URI.</param>
		private static void UpdateRemoteObject(String uri)
		{
			// register net-pipe channel
			IpcClientChannel clientChannel = new IpcClientChannel();
			ChannelServices.RegisterChannel(clientChannel, true);

			// get shared object from other process
			InstanceProxy proxy =
				Activator.GetObject(typeof(InstanceProxy),
				$"ipc://{Environment.MachineName}{uri}/{uri}") as InstanceProxy;

			// pass current command line args to proxy
			if(proxy != null)
				proxy.SetCommandLineArgs(InstanceProxy.IsFirstInstance, InstanceProxy.TimerName);

			// close current client channel
			ChannelServices.UnregisterChannel(clientChannel);
		}

		/// <summary>Registers the remote type.</summary>
		/// <param name="uri">The URI.</param>
		private static void RegisterRemoteType(String uri)
		{
			// register remote channel (net-pipes)
			IpcServerChannel serverChannel = new IpcServerChannel(Environment.MachineName + uri);
			ChannelServices.RegisterChannel(serverChannel, true);

			// register shared type
			RemotingConfiguration.RegisterWellKnownServiceType(
				typeof(InstanceProxy), uri, WellKnownObjectMode.Singleton);

			// close channel, on process exit
			Process process = Process.GetCurrentProcess();
			process.Exited += delegate { ChannelServices.UnregisterChannel(serverChannel); };
		}

		/// <summary>Wait Or Timer Callback Handler</summary>
		/// <param name="state">The state.</param>
		/// <param name="timedOut">if set to <c>true</c> [timed out].</param>
		private static void WaitOrTimerCallback(Object state, Boolean timedOut)
		{
			// cast to event handler
			EventHandler<InstanceCallbackEventArgs> callback = state as EventHandler<InstanceCallbackEventArgs>;
			if(callback == null)
				return;

			// invoke event handler on other process
			callback(state, new InstanceCallbackEventArgs(InstanceProxy.IsFirstInstance, InstanceProxy.TimerName));
		}
	}
}