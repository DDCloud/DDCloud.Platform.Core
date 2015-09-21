using System;

namespace DDCloud.Platform.Core.Diagnostics
{
	/// <summary>
	///		Represents a source of activity correlation information.
	/// </summary>
	[Serializable]
	public enum CorrelationSource
	{
		/// <summary>
		///		The source of activity-correlation information is unknown.
		/// </summary>
		/// <remarks>
		///		Used to detect uninitialised values; do not use directly.
		/// </remarks>
		Unknown		= 0,

		/// <summary>
		///		Activity-correlation information comes from the platform.
		/// </summary>
		Platform	= 1,

		/// <summary>
		///		Activity-correlation information comes from <see cref="EventSource">ETW</see>'s <see cref="System.Diagnostics.Tracing.EventSource.CurrentThreadActivityId"/>.
		/// </summary>
		EventSource			= 2
	}
}
