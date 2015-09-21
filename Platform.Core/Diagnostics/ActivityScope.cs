using System;

namespace DDCloud.Platform.Core.Diagnostics
{
	/// <summary>
	///		Represents a scope for an activity.
	/// </summary>
	/// <remarks>
	///		When the scope is disposed, the previous activity Id (if any) will be restored.
	/// </remarks>
	/// <seealso cref="ActivityCorrelationManager"/>.
	public sealed class ActivityScope
		: DisposableObject
	{
		/// <summary>
		///		The current activity Id (if any).
		/// </summary>
		readonly Guid?	_activityId;

		/// <summary>
		///		The previous activity Id (if any).
		/// </summary>
		readonly Guid? _previousActivityId;

		/// <summary>
		///		Create a new activity scope.
		/// </summary>
		/// <param name="activityId">
		///		The current activity Id (if any).
		/// </param>
		/// <param name="previousActivityId">
		///		The previous activity Id (if any).
		/// </param>
		internal ActivityScope(Guid? activityId = null, Guid? previousActivityId = null)
		{
			_activityId = activityId;
			_previousActivityId = previousActivityId;

			if (_activityId.HasValue)
				ActivityCorrelationManager.SetCurrentActivityId(_activityId.Value);
			else
				ActivityCorrelationManager.ClearCurrentActivityId();

			ActivityCorrelationManager.SynchronizeEventSourceActivityIds();
		}

		/// <summary>
		///		Dispose of resources being used by the object.
		/// </summary>
		/// <param name="disposing">
		///		Explicit disposal?
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// If the correlation manager does not have the expected activity Id, it's safer to not clean up.
				if (ActivityCorrelationManager.GetCurrentActivityId() == _activityId)
				{
					// Restore previous activity Id (if any).
					if (_previousActivityId.HasValue)
						ActivityCorrelationManager.SetCurrentActivityId(_previousActivityId.Value);
					else
						ActivityCorrelationManager.ClearCurrentActivityId();

					ActivityCorrelationManager.SynchronizeEventSourceActivityIds();
				}
			}

			base.Dispose(disposing);
		}

		/// <summary>
		///		The current activity Id (if any).
		/// </summary>
		public Guid? ActivityId
		{
			get
			{
				CheckDisposed();

				return _activityId;
			}
		}

		/// <summary>
		///		The previous activity Id (if any).
		/// </summary>
		public Guid? PreviousActivityId
		{
			get
			{
				CheckDisposed();

				return _previousActivityId;
			}
		}
	}
}
