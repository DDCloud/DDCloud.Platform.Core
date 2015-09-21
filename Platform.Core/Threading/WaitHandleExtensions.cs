using System;
using System.Threading;

namespace DDCloud.Platform.Core.Threading
{
	/// <summary>
	///		Extension methods for <see cref="WaitHandle"/>s.
	/// </summary>
	public static class WaitHandleExtensions
	{
		/// <summary>
		///		Set the <see cref="ManualResetEvent"/> to signaled if the specified <see cref="CancellationToken"/> is cancelled.
		/// </summary>
		/// <param name="waitHandle">
		///		The <see cref="ManualResetEvent"/>.
		/// </param>
		/// <param name="cancellationToken">
		///		The cancellation token.
		/// </param>
		public static void SetIfCancelled(this ManualResetEvent waitHandle, CancellationToken cancellationToken)
		{
			if (waitHandle == null)
				throw new ArgumentNullException(nameof(waitHandle));

			if (cancellationToken.IsCancellationRequested)
			{
				waitHandle.Set();

				return;
			}

			cancellationToken.Register(
				() =>
				{
					try
					{
						waitHandle.Set();
					}
					catch (ObjectDisposedException)
					{
						// That's fine, nothing needs to be done.
					}
				}
			);
		}

		/// <summary>
		///		Wait for the <see cref="ManualResetEvent"/> to enter the signaled state.
		/// </summary>
		/// <param name="waitHandle">
		///		The <see cref="ManualResetEvent"/>.
		/// </param>
		/// <param name="cancellationToken">
		///		A <see cref="CancellationToken"/> that can be used to cancel the wait operation.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the <see cref="ManualResetEvent"/> enters the signaled state before the <see cref="CancellationToken"/> is cancelled; otherwise, <c>false</c>.
		/// </returns>
		public static bool WaitOne(this ManualResetEvent waitHandle, CancellationToken cancellationToken)
		{
			if (waitHandle == null)
				throw new ArgumentNullException(nameof(waitHandle));

			using (ManualResetEvent timeout = new ManualResetEvent(false))
			{
				timeout.SetIfCancelled(cancellationToken);

                try
				{
					WaitHandle[] bothWaitHandles =
					{
						timeout,
						waitHandle
					};

					return WaitHandle.WaitAny(bothWaitHandles) == 1;
				}
				catch (TimeoutException)
				{
					return false;
				}
			}
		}

		/// <summary>
		///		Set the <see cref="AutoResetEvent"/> to signaled if the specified <see cref="CancellationToken"/> is cancelled.
		/// </summary>
		/// <param name="waitHandle">
		///		The <see cref="AutoResetEvent"/>
		/// </param>
		/// <param name="cancellationToken">
		///		The cancellation token.
		/// </param>
		public static void SetIfCancelled(this AutoResetEvent waitHandle, CancellationToken cancellationToken)
		{
			if (waitHandle == null)
				throw new ArgumentNullException(nameof(waitHandle));

			if (cancellationToken.IsCancellationRequested)
			{
				waitHandle.Set();

				return;
			}

			cancellationToken.Register(
				() =>
				{
					try
					{
						waitHandle.Set();
					}
					catch (ObjectDisposedException)
					{
						// That's fine, nothing needs to be done.
					}
				}
			);
		}

		/// <summary>
		///		Wait for the <see cref="AutoResetEvent"/> to enter the signaled state.
		/// </summary>
		/// <param name="waitHandle">
		///		The <see cref="AutoResetEvent"/>.
		/// </param>
		/// <param name="cancellationToken">
		///		A <see cref="CancellationToken"/> that can be used to cancel the wait operation.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the <see cref="AutoResetEvent"/> enters the signaled state before the <see cref="CancellationToken"/> is cancelled; otherwise, <c>false</c>.
		/// </returns>
		public static bool WaitOne(this AutoResetEvent waitHandle, CancellationToken cancellationToken)
		{
			if (waitHandle == null)
				throw new ArgumentNullException(nameof(waitHandle));

			using (AutoResetEvent timeout = new AutoResetEvent(false))
			{
				timeout.SetIfCancelled(cancellationToken);

				try
				{
					WaitHandle[] bothWaitHandles =
					{
						timeout,
						waitHandle
					};

					return WaitHandle.WaitAny(bothWaitHandles) == 1;
				}
				catch (TimeoutException)
				{
					return false;
				}
			}
		}
	}
}
