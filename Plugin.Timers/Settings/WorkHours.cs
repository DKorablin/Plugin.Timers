using System;
using System.Diagnostics;

namespace Plugin.Timers.Settings
{
	/// <summary>Timer operating hours</summary>
	[DebuggerDisplay(nameof(Code))]
	public class WorkHours
	{
		/// <summary>Maximum time interval: <c>23:59:58 - 23:59:59</c></summary>
		public const Int64 MaxWorkHours = 371080879493502;
		/// <summary>Minimum time interval: <c>0:00:00 - 23:59:59</c></summary>
		public const Int64 MinWorkHours = 0;
		/// <summary>Maximum number of seconds for the timer: <c>23:59:59</c></summary>
		private const Int32 MaxSeconds = 86399;

		/// <summary>Time interval storage</summary>
		public Int64 Code { get; set; }

		/// <summary>Start minutes count beginning from 0:00:00</summary>
		public Int32 StartMinutes
		{
			get => WorkHours.LoWord(this.Code);
			set
			{
				if(WorkHours.IsValid(value, this.EndMinutes))
					this.Code = WorkHours.GetCode(value, this.EndMinutes);
			}
		}

		/// <summary>End minutes count beginning from 0:00:00</summary>
		public Int32 EndMinutes
		{
			get
			{
				Int32 result = WorkHours.HiWord(this.Code);
				return result == 0 || result > WorkHours.MaxSeconds
					? WorkHours.MaxSeconds
					: result;
			}
			set
			{
				if(WorkHours.IsValid(this.StartMinutes, value))
					this.Code = WorkHours.GetCode(this.StartMinutes, value);
			}
		}

		/// <summary>Start time span</summary>
		public TimeSpan StartTimeSpan
		{
			get => new TimeSpan(0, 0, this.StartMinutes);
			set
			{
				if(WorkHours.IsValid(value, this.EndTimeSpan))
					this.Code = WorkHours.GetCode(value, this.EndTimeSpan);
			}
		}

		/// <summary>End time span</summary>
		/// <remarks>
		/// After timer triggers next time timer reach non-working interval,
		/// timer stops till <see cref="WorkHours.StartTimeSpan"/> + <see cref="TimerSettingsItem.Interval"/>
		/// </remarks>
		public TimeSpan EndTimeSpan
		{
			get => new TimeSpan(0, 0, this.EndMinutes);
			set
			{
				if(WorkHours.IsValid(this.StartTimeSpan, value))
					this.Code = WorkHours.GetCode(this.StartTimeSpan, value);
			}
		}

		/// <summary>Checking that the timer is running at this time. Time can be either night or day</summary>
		/// <returns>Timer in active state</returns>
		public Boolean IsWorkTime
		{
			get
			{
				DateTime now = DateTime.Now;
				TimeSpan ts = now - now.Date;

				Boolean result = this.StartTimeSpan < ts && this.EndTimeSpan > ts;

				if(!result && this.StartTimeSpan > this.EndTimeSpan)
					result = this.StartTimeSpan < ts && this.EndTimeSpan < ts;

				return result;
			}
		}

		/// <summary>Create instance of WorkHours class and specify start and end time</summary>
		/// <param name="start">Start interval</param>
		/// <param name="end">End interval</param>
		public WorkHours(TimeSpan start, TimeSpan end)
			: this(WorkHours.GetCode(start, end))
		{ }

		/// <summary>ctor</summary>
		/// <param name="code">Code from the base</param>
		/// <exception cref="OverflowException">Invalid code specified</exception>
		public WorkHours(Int64 code)
			=>this.Code = WorkHours.IsValid(code)
				? code
				: throw new OverflowException("Invalid code");

		public WorkHours()
			=> this.Code = 0;

		/// <summary>Gets value hash-code</summary>
		/// <returns>Unique workHours hash-code value</returns>
		public override Int32 GetHashCode()
			=> this.Code.GetHashCode();

		/// <summary>String representation of service operation hours</summary>
		/// <returns>String representation of service operation hours</returns>
		public override String ToString()
		{
			TimeSpan start = this.StartTimeSpan;
			TimeSpan end = this.EndTimeSpan;

			return start.TotalSeconds == 0 && end.TotalSeconds == WorkHours.MaxSeconds
				? "Empty"
				: String.Join(" — ", new String[] { $"{start.Hours}:{start.Minutes:D2}", $"{end.Hours}:{end.Minutes:D2}", });
		}

		private static Boolean IsValid(Int64 code)
		{
			if(code == 0)
				return true;//By default, time is transmitted in the format from 0 to 23:59
			if(code < WorkHours.MinWorkHours || code > WorkHours.MaxWorkHours)
				return false;//0:00:00 - 23:59:59

			Int32 start = WorkHours.LoWord(code);
			Int32 end = WorkHours.HiWord(code);
			return WorkHours.IsValid(start, end);
		}

		private static Boolean IsValid(TimeSpan start, TimeSpan end)
			=> WorkHours.IsValid((Int32)start.TotalSeconds, (Int32)end.TotalSeconds);

		/// <summary>//Here we check not for start&lt;end, but for min/max. Because working hours can be from 10:00:00 PM to 9:00:00 AM</summary>
		private static Boolean IsValid(Int32 start, Int32 end)
			=> start >= 0 && start <= WorkHours.MaxSeconds
				&& end >= 0 && end <= WorkHours.MaxSeconds;

		private static Int64 GetCode(TimeSpan start, TimeSpan end)
			=> WorkHours.GetCode((Int32)start.TotalSeconds, (Int32)end.TotalSeconds);

		private static Int64 GetCode(Int32 start, Int32 end)
			=> WorkHours.MakeQword(start, end);

		/// <summary>Retrieves the high-order word from the specified 32-bit value.</summary>
		/// <param name="value">The value to be converted.</param>
		/// <returns>The return value is the high-order word of the specified value.</returns>
		private static Int32 HiWord(Int64 value)
			=> (Int32)(value >> 32);

		/// <summary>Retrieves the low-order word from the specified value.</summary>
		/// <param name="value">The value to be converted. </param>
		/// <returns>The return value is the low-order word of the specified value.</returns>
		private static Int32 LoWord(Int64 value)
			=> (Int32)(value & 0x00000000FFFFFFFF);

		/// <summary>Makes a 64 bit integer from two 32 bit shorts</summary>
		/// <param name="low">The low order value.</param>
		/// <param name="high">The high order value.</param>
		/// <returns></returns>
		private static Int64 MakeQword(Int32 low, Int32 high)
			=> (low + (((Int64)high) << 32));
	}
}