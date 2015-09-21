using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark a parameter as being completely handled before a method returns.
	/// </summary>
	/// <remarks>
	///		If the parameter is a delegate, indicates that delegate is executed before the method returns.
	///     If the parameter is an enumerable, indicates that it is enumerated before method returns.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
	public sealed class InstantHandleAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified parameter as being completely handled before the method returns.
		/// </summary>
		public InstantHandleAttribute()
		{
		}
	}
}