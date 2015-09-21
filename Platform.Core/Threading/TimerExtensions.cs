using System;
using System.Threading;

namespace DDCloud.Platform.Core.Threading
{
	/// <summary>
	///		Extension methods for <see cref="Timer"/>.
	/// </summary>
	public static class TimerExtensions
	{
		/// <summary>
		///		Start the timer.
		/// </summary>
		/// <param name="timer">
		///		The timer.
		/// </param>
		/// <param name="period">
		///		A <see cref="TimeSpan"/> representing the span of time between successive invocations of the timer callback.
		/// </param>
		public static void Start(this Timer timer, TimeSpan period)
		{
			if (timer == null)
				throw new ArgumentNullException(nameof(timer));

			if (period == TimeSpan.Zero)
				throw new ArgumentOutOfRangeException(nameof(period), period, "Timer period cannot be 0.");

			timer.Change(TimeSpan.Zero, period);
		}

		/// <summary>
		///		STop the timer.
		/// </summary>
		/// <param name="timer">
		///		The timer.
		/// </param>
		public static void Stop(this Timer timer)
		{
			if (timer == null)
				throw new ArgumentNullException(nameof(timer));

			timer.Change(TimeSpan.Zero, TimeSpan.Zero);
		}
	}
}
