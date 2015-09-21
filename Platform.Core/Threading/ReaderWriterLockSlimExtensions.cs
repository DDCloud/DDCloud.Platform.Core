using System;
using System.Threading;

namespace DDCloud.Platform.Core.Threading
{
	/// <summary>
	///		Extension methods for <see cref="ReaderWriterLockSlim"/>.
	/// </summary>
	/// <remarks>
	///		<see cref="ReaderWriterLockSlim"/> is a useful type, but all those <c>try</c>..<c>finally</c> blocks are ugly to look at.
	///	
	///		These extension methods enable us to simply use <c>using</c> blocks instead.
	/// </remarks>
	public static class ReaderWriterLockSlimExtensions
	{
		/// <summary>
		///		Enter the reader / writer lock's read lock.
		/// </summary>
		/// <param name="readerWriterLock">
		///		The reader / writer lock.
		/// </param>
		/// <returns>
		///		An <see cref="IDisposable"/> implementation which, when disposed, exits the lock.
		/// </returns>
		public static IDisposable ReadLock(this ReaderWriterLockSlim readerWriterLock)
		{
			if (readerWriterLock == null)
				throw new ArgumentNullException(nameof(readerWriterLock));

			readerWriterLock.EnterReadLock();

			return new DisposableLock(readerWriterLock, DisposableLock.LockType.Read);
		}

		/// <summary>
		///		Enter the reader / writer lock's read lock.
		/// </summary>
		/// <param name="readerWriterLock">
		///		The reader / writer lock.
		/// </param>
		/// <returns>
		///		An <see cref="IDisposable"/> implementation which, when disposed, exits the lock.
		/// </returns>
		public static IDisposable UpgradeableReadLock(this ReaderWriterLockSlim readerWriterLock)
		{
			if (readerWriterLock == null)
				throw new ArgumentNullException(nameof(readerWriterLock));

			readerWriterLock.EnterUpgradeableReadLock();

			return new DisposableLock(readerWriterLock, DisposableLock.LockType.UpgradeableRead);
		}

		/// <summary>
		///		Enter the reader / writer lock's write lock.
		/// </summary>
		/// <param name="readerWriterLock">
		///		The reader / writer lock.
		/// </param>
		/// <returns>
		///		An <see cref="IDisposable"/> implementation which, when disposed, exits the lock.
		/// </returns>
		public static IDisposable WriteLock(this ReaderWriterLockSlim readerWriterLock)
		{
			if (readerWriterLock == null)
				throw new ArgumentNullException(nameof(readerWriterLock));

			readerWriterLock.EnterWriteLock();

			return new DisposableLock(readerWriterLock, DisposableLock.LockType.Write);
		}

		/// <summary>
		///		Wraps up the exit of a read-lock in an <see cref="IDisposable"/> implementation.
		/// </summary>
		struct DisposableLock
			: IDisposable
		{
			/// <summary>
			///		The underlying lock.
			/// </summary>
			readonly ReaderWriterLockSlim	_readerWriterLock;

			/// <summary>
			///		The type of lock represented by the <see cref="DisposableLock"/>.
			/// </summary>
			readonly LockType				_lockType;

			/// <summary>
			///		Has the lock been disposed?
			/// </summary>
			bool							_isDisposed;

			/// <summary>
			///		Create a new read lock.
			/// </summary>
			/// <param name="readerWriterLock">
			///		The reader / writer lock whose read lock is to be entered.
			/// </param>
			/// <param name="lockType">
			///		The type of lock represented by the <see cref="DisposableLock"/>.
			/// </param>
			public DisposableLock(ReaderWriterLockSlim readerWriterLock, LockType lockType)
			{
				if (readerWriterLock == null)
					throw new ArgumentNullException(nameof(readerWriterLock));

				if (lockType == LockType.Unknown)
					throw new ArgumentOutOfRangeException(nameof(lockType), "Invalid lock type.");

				_readerWriterLock = readerWriterLock;
				_lockType = lockType;
				_isDisposed = false;
			}

			/// <summary>
			///		Dispose of the writer lock.
			/// </summary>
			public void Dispose()
			{
				if (_isDisposed)
					return;

				switch (_lockType)
				{
					case LockType.Read:
					{
						_readerWriterLock.ExitReadLock();

						break;
					}
					case LockType.UpgradeableRead:
					{
						_readerWriterLock.ExitUpgradeableReadLock();

						break;
					}
					case LockType.Write:
					{
						_readerWriterLock.ExitWriteLock();

						break;
					}
				}

				_isDisposed = true;
			}

			/// <summary>
			///		The type of lock that a given <see cref="DisposableLock"/> represents.
			/// </summary>
			public enum LockType
				: byte
			{
				/// <summary>
				///		An unknown lock type.
				/// </summary>
				/// <remarks>
				///		Used to detect uninitialised values; do not use directly.
				/// </remarks>
				Unknown			= 0,

				/// <summary>
				///		A read lock.
				/// </summary>
				Read			= 1,

				/// <summary>
				///		An upgradeable read lock.
				/// </summary>
				UpgradeableRead	= 2,

				/// <summary>
				///		A write lock.
				/// </summary>
				Write			= 3,
			}
		}
	}
}
