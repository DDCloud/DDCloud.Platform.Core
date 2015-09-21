using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace DDCloud.Platform.Core.Threading
{
	/// <summary>
	///		Logical call-context services.
	/// </summary>
	/// <remarks>
	///		The platform's implementation of logical call-context is not <see cref="AppDomain"/>-agile (it cannot traverse an <see cref="AppDomain"/> boundary, and will be <c>null</c> on the other side).
	/// 
	///		TODO: Implement a method to enable copy-on-write semantics for children of the current logical call.
	///		TODO: Consider replacing the storage protocol with one based on System.Collections.Immutable.
	/// </remarks>
	public static class LogicalCallContext
	{
		/// <summary>
		///		Locking object used to synchronise write access to the storage implementation.
		/// </summary>
		static readonly object StorageImplementationLock = new object();

		/// <summary>
		///		The current logical call-context storage implementation.
		/// </summary>
		static ILogicalCallContextStorage _storage = new RemotingCallContextStorage();

		/// <summary>
		///		Get data from the logical call-context.
		/// </summary>
		/// <typeparam name="TData">
		///		The call-context data type.
		/// </typeparam>
		/// <param name="key">
		///		The key that identifies the call-context data.
		/// </param>
		/// <returns>
		///		The data, or <c>default</c>(<typeparamref name="TData"/>) if the data is not present in the call-context.
		/// </returns>
		public static TData Get<TData>(string key)
		{
			if (String.IsNullOrWhiteSpace(key))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'key'.", nameof(key));

			ConcurrentDictionary<string, object> callContextTable = _storage.GetStorage();
			if (callContextTable == null)
				return default(TData);

			object data;
			if (!callContextTable.TryGetValue(key, out data))
				return default(TData);

			return (TData)data;
		}

		/// <summary>
		///		Get data from the logical call-context.
		/// </summary>
		/// <typeparam name="TData">
		///		The call-context data type.
		/// </typeparam>
		/// <param name="key">
		///		The key that identifies the call-context data.
		/// </param>
		/// <param name="data">
		///		The data.
		/// </param>
		public static void Set<TData>(string key, TData data)
		{
			if (String.IsNullOrWhiteSpace(key))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'key'.", nameof(key));

			ConcurrentDictionary<string, object> callContextTable = _storage.GetOrCreateStorage();
			callContextTable[key] = data;
		}

		/// <summary>
		///		Use the specified logical call-context storage implementation.
		/// </summary>
		/// <param name="storage">
		///		An implementation of <see cref="ILogicalCallContextStorage"/> used to store logical call-context data, or <c>null</c> to use the default implementation.
		/// </param>
		/// <param name="releasePreviousStorage">
		///		Release the previously configured storage (if any)?
		/// 
		///		Default is <c>true</c>.
		/// </param>
		/// <remarks>
		///		This is per-<see cref="AppDomain"/>.
		/// </remarks>
		public static void UseStorage(ILogicalCallContextStorage storage, bool releasePreviousStorage = true)
		{
			lock (StorageImplementationLock)
			{
				if (storage == null)
					storage = CreateDefaultStorage();

				ILogicalCallContextStorage previousStorage = _storage;
				_storage = storage;

				if (releasePreviousStorage)
					previousStorage.ReleaseStorage();
			}
		}

		/// <summary>
		///		Clear the current logical call-context.
		/// </summary>
		public static void ClearCallContext()
		{
			_storage.ReleaseStorage();
		}

		/// <summary>
		///		Create an instance of the default call-context storage provider.
		/// </summary>
		/// <returns>
		///		The new call-context storage provider.
		/// </returns>
		public static ILogicalCallContextStorage CreateDefaultStorage()
		{
			return new RemotingCallContextStorage();
		}

		/// <summary>
		///		Logical call-context storage using Remoting's <see cref="CallContext"/>.
		/// </summary>
		class RemotingCallContextStorage
			: ILogicalCallContextStorage
		{
			/// <summary>
			///		The <see cref="CallContext"/> key used by the platform's logical call-context.
			/// </summary>
			const string CallContextKey = "DDCloud.Platform.LogicalCallContext";

			/// <summary>
			///		Create a new <see cref="RemotingCallContextStorage"/>.
			/// </summary>
			public RemotingCallContextStorage()
			{
			}

			/// <summary>
			///		Determine whether storage been allocated for the current logical call-context.
			/// </summary>
			/// <returns>
			///		<c>true</c>, if storage has been allocated; otherwise, <c>false</c>.
			/// </returns>
			public bool IsStorageCreated()
			{
				return CallContext.LogicalGetData(CallContextKey) != null;
			}

			/// <summary>
			///		Get the logical call-context table (if it exists).
			/// </summary>
			/// <returns>
			///		A <see cref="ConcurrentDictionary{TKey,TValue}"/> of <see cref="object"/>s (keyed by call-context key), or <c>null</c> if no logical call-context is present.
			/// </returns>
			public ConcurrentDictionary<string, object> GetStorage()
			{
				ObjectHandle callContextTableHandle = (ObjectHandle)CallContext.LogicalGetData(CallContextKey);
				if (callContextTableHandle == null)
					return null;

				if (RemotingServices.IsTransparentProxy(callContextTableHandle))
					return null; // Must have crossed an AppDomain boundary.

				return (ConcurrentDictionary<string, object>)callContextTableHandle.Unwrap();
			}

			/// <summary>
			///		Get or create the logical call-context table.
			/// </summary>
			/// <returns>
			///		A <see cref="ConcurrentDictionary{TKey,TValue}"/> of <see cref="Object"/>s (keyed by call-context key).
			/// </returns>
			public ConcurrentDictionary<string, object> GetOrCreateStorage()
			{
				ConcurrentDictionary<string, object> callContextTable = GetStorage();
				if (callContextTable == null)
				{
					callContextTable = new ConcurrentDictionary<string, object>();
					CallContext.LogicalSetData(
						CallContextKey,
						new ObjectHandle(callContextTable) // We use ObjectHandles so we never unintentionally serialise call-context data when crossing AppDomain boundaries (eg. in Visual Studio unit-tests).
					);
				}

				return callContextTable;
			}

			/// <summary>
			///		Release call-context storage.
			/// </summary>
			public void ReleaseStorage()
			{
				CallContext.FreeNamedDataSlot(CallContextKey);
			}
		}
	}
}
