using System;
using System.Threading;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		An <see cref="IDisposable"/> implementation which calls an <see cref="Action"/> delegate when disposed.
	/// </summary>
	public sealed class DelegateDisposable
		: IDisposable
	{
		/// <summary>
		///		The <see cref="Action"/> to execute when the <see cref="DelegateDisposable"/> is disposed.
		/// </summary>
		Action _disposalAction;

		/// <summary>
		///		Create a new <see cref="DelegateDisposable"/> that calls the specified <see cref="Action"/> when disposed.
		/// </summary>
		/// <param name="disposalAction">
		///		The <see cref="Action"/> to execute when the <see cref="DelegateDisposable"/> is disposed.
		/// </param>
		public DelegateDisposable(Action disposalAction)
		{
			if (disposalAction == null)
				throw new ArgumentNullException(nameof(disposalAction));

			_disposalAction = disposalAction;
		}

		/// <summary>
		///		Dispose the <see cref="DelegateDisposable"/>, calling the disposal action, if it has not already been called.
		/// </summary>
		public void Dispose()
		{
			Action disposalAction = Interlocked.Exchange(ref _disposalAction, null);
			if (disposalAction == null)
				return;

			disposalAction();
		}
	}
}
