using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace DDCloud.Platform.Core
{
	using Diagnostics;
	using Threading;

	/// <summary>
	///		Handles activity correlation in an <c>async</c>/<c>await</c>-friendly manner.
	/// </summary>
	public static class ActivityCorrelationManager
	{
		/// <summary>
		///		The Platform activity Id call-context key.
		/// </summary>
		const string ActivityIdCallContextKey = "DDCloud.Platform.Diagnostics.ActivityId";

		/// <summary>
		///		The Platform activity Id name.
		/// </summary>
		public const string ActivityIdHeaderName = "X-DDCloud-ActivityId";

		/// <summary>
		///		The key used to store the current activity in <see cref="Exception"/>.<see cref="Exception.Data"/>.
		/// </summary>
		public const string ActivityIdExceptionDataKey = ActivityIdCallContextKey;

		/// <summary>
		///		The System.Diagnostics correlation manager.
		/// </summary>
		static readonly CorrelationManager SystemCorrelationManager = Trace.CorrelationManager;

		/// <summary>
		///		Get the current activity Id (if any).
		/// </summary>
		/// <returns>
		///		The current activity Id, or <c>null</c> if no activity Id is currently set.
		/// </returns>
		public static Guid? GetCurrentActivityId()
		{
			return LogicalCallContext.Get<Guid?>(ActivityIdCallContextKey);
		}

		/// <summary>
		///		Create a new activity Id, and set it as the current activity Id.
		/// </summary>
        /// <returns>
        ///		The new activity Id.
        /// </returns>
		public static Guid CreateActivityId()
		{
			Guid newActivityId = Guid.NewGuid();
			SetCurrentActivityId(newActivityId);

			return newActivityId;
		}

		/// <summary>
		///		Set the current activity Id.
		/// </summary>
		/// <param name="activityId">
		///		The activity Id.
		/// </param>
		/// <exception cref="ArgumentException">
		///		<paramref name="activityId"/> is <see cref="Guid.Empty"/>.
		/// </exception>
		public static void SetCurrentActivityId(Guid activityId)
		{
			if (activityId == Guid.Empty)
				throw new ArgumentException("GUID cannot be empty: 'activityId'.", nameof(activityId));

			LogicalCallContext.Set<Guid?>(ActivityIdCallContextKey, activityId);
		}

		/// <summary>
		///		Clear the current activity Id.
		/// </summary>
		public static void ClearCurrentActivityId()
		{
			LogicalCallContext.Set<Guid?>(ActivityIdCallContextKey, null);
		}

		/// <summary>
		///		Create an activity scope.
		/// </summary>
		/// <param name="activityId">
		///		An optional specific activity Id to use.
		/// 
		///		If not specified, a new activity Id is generated.
		/// </param>
		/// <returns>
		///		The new activity scope.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="activityId"/> is <see cref="Guid.Empty"/>.
		/// </exception>
		/// <remarks>
		///		When the scope is disposed, the previous activity Id (if any) will be restored.
		/// </remarks>
		public static ActivityScope BeginActivityScope(Guid? activityId = null)
		{
			if (activityId == Guid.Empty)
				throw new ArgumentException("GUID cannot be empty: 'activityId'.", nameof(activityId));

			return new ActivityScope(
				activityId: activityId ?? Guid.NewGuid(),
				previousActivityId: GetCurrentActivityId()
			);
		}

		/// <summary>
		///		Create an activity scope that ensures a that there is a current activity.
		/// </summary>
		/// <returns>
		///		The new activity scope.
		/// </returns>
		/// <remarks>
		///		If there is already an ambient activity, the scope will maintain it.
		/// 
		///		When the scope is disposed, the previous activity Id (if any) will be restored.
		/// </remarks>
		public static ActivityScope RequireActivityScope()
		{
			Guid? previousActivityId = GetCurrentActivityId();

			return new ActivityScope(
				activityId: previousActivityId ?? Guid.NewGuid(),
				previousActivityId: previousActivityId
			);
		}

		/// <summary>
		///		Create a new activity scope that suppresses the ambient activity.
		/// </summary>
		/// <returns>
		///		The new activity scope.
		/// </returns>
		/// <remarks>
		///		When the scope is disposed, the previous activity Id (if any) will be restored.
		/// </remarks>
		public static ActivityScope BeginSuppressActivityScope()
		{
			return new ActivityScope(
				activityId: null,
				previousActivityId: GetCurrentActivityId()
			);
		}

		/// <summary>
		///		Update event source activity Ids with the current Platform activity Id (if one is currently set).
		/// </summary>
		/// <param name="correlationSource">
		///		A <see cref="CorrelationSource"/> value representing the source of activity-correlation information.
		/// 
		///		Default is <see cref="CorrelationSource.Platform"/>.
		/// </param>
		public static void SynchronizeEventSourceActivityIds(CorrelationSource correlationSource = CorrelationSource.Platform)
		{
			switch (correlationSource)
			{
				case CorrelationSource.Platform:
				{
					Guid? currentPlatformActivityId = GetCurrentActivityId() ?? Guid.Empty;
					
					if (EventSource.CurrentThreadActivityId != currentPlatformActivityId)
						EventSource.SetCurrentThreadActivityId(currentPlatformActivityId.Value);

					if (SystemCorrelationManager.ActivityId != currentPlatformActivityId)
						SystemCorrelationManager.ActivityId = currentPlatformActivityId.Value;

					break;
				}
				case CorrelationSource.EventSource:
				{
					Guid currentActivityId = EventSource.CurrentThreadActivityId;
					if (currentActivityId != Guid.Empty)
						SetCurrentActivityId(currentActivityId);
					else
						ClearCurrentActivityId();

					SystemCorrelationManager.ActivityId = currentActivityId;

					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException(
						nameof(correlationSource),
						String.Format(
							"Unsupported correlation source: '{0}'.",
							correlationSource
						)
					);
				}
			}
		}

		/// <summary>
		///		Capture the current activity Id (if any), and add it to the specified <see cref="Exception"/>'s <see cref="Exception.Data">custom property dictionary</see>.
		/// </summary>
		/// <typeparam name="TException">
		///		The exception type.
		/// </typeparam>
		/// <param name="exception">
		///		The exception to add the current activity Id to.
		/// </param>
		/// <returns>
		///		The <paramref name="exception"/> (enables method-chaining / inline use).
		/// </returns>
		public static TException CaptureCurrentActityId<TException>(this TException exception)
			where TException : Exception
		{
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));

			exception.Data[ActivityIdExceptionDataKey] = GetCurrentActivityId();

			return exception;
		}

		/// <summary>
		///		Capture the current activity Id (if any), and add it to the specified <see cref="Exception"/>'s <see cref="Exception.Data">custom property dictionary</see>.
		/// </summary>
		/// <typeparam name="TException">
		///		The exception type.
		/// </typeparam>
		/// <param name="exception">
		///		The exception to add the current activity Id to.
		/// </param>
		/// <returns>
		///		A <see cref="Nullable{T}"/> <see cref="Guid"/> representing the captured activity Id (<c>null</c>, if no captured activity Id is present in the exception's custom data).
		/// </returns>
		public static Guid? GetCapturedActityId<TException>(this TException exception)
			where TException : Exception
		{
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));

			return (Guid?)exception.Data[ActivityIdExceptionDataKey];
		}
	}
}
