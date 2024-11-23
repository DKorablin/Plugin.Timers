using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Plugin.Timers
{
	internal static class Utils
	{
		public static Boolean IsBitSet(Int32 flags, Int32 bit)
			=> (flags & Convert.ToInt32(Math.Pow(2, bit))) != 0;

		public static Int32[] BitToInt(params Boolean[] bits)
		{
			Int32[] result = new Int32[] { };
			Int32 counter = 0;
			for(Int32 loop = 0; loop < bits.Length; loop++)
			{
				if(result.Length <= loop)
					Array.Resize<Int32>(ref result, result.Length + 1);

				for(Int32 innerLoop = 0; innerLoop < 32; innerLoop++)
				{
					result[loop] |= Convert.ToInt32(bits[counter++]) << innerLoop;
					if(counter >= bits.Length)
						break;
				}
				if(counter >= bits.Length)
					break;
			}
			return result;
		}

		/// <summary>Fatal exception that can't be handled by user code</summary>
		/// <param name="exception">Exception that we will check</param>
		/// <returns>Exception is fatal there is no reason to process it futher</returns>
		public static Boolean IsFatal(Exception exception)
		{
			while(exception != null)
			{
				if((exception is OutOfMemoryException && !(exception is InsufficientMemoryException))//No point to taking up more memory
					|| exception is ThreadAbortException//Error occurs when redirecting from one page to another
					|| exception is AccessViolationException
					|| exception is SEHException)
					return true;
				if(!(exception is TypeInitializationException) && !(exception is TargetInvocationException))
					break;
				exception = exception.InnerException;
			}
			return false;
		}
	}
}