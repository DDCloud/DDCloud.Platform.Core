using System.Collections.Concurrent;

namespace DDCloud.Platform.Core.Threading
{
	/// <summary>
	///		An abstraction over logical call-context storage.
	/// </summary>
	public interface ILogicalCallContextStorage
	{
		/// <summary>
		///		Determine whether storage been allocated for the current logical call-context.
		/// </summary>
		/// <returns>
		///		<c>true</c>, if storage has been allocated; otherwise, <c>false</c>.
		/// </returns>
		bool IsStorageCreated();

		/// <summary>
		///		Release call-context storage.
		/// </summary>
		void ReleaseStorage();

		/// <summary>
		///		Get the logical call-context table (if it exists).
		/// </summary>
		/// <returns>
		///		A <see cref="ConcurrentDictionary{TKey,TValue}"/> of <see cref="object"/>s (keyed by call-context key), or <c>null</c> if no logical call-context is present.
		/// </returns>
		ConcurrentDictionary<string, object> GetStorage();

		/// <summary>
		///		Get or create the logical call-context table.
		/// </summary>
		/// <returns>
		///		A <see cref="ConcurrentDictionary{TKey,TValue}"/> of <see cref="object"/>s (keyed by call-context key).
		/// </returns>
		ConcurrentDictionary<string, object> GetOrCreateStorage();
	}
}
