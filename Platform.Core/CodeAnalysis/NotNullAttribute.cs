using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark an element as never being <c>null</c>.
	/// </summary>
	/// <example>
	///     <code>
	///			[NotNull]
	///			public object Foo()
	///			{
	///				return null; // Warning: Possible 'null' assignment
	///			}
	///		</code>
	/// </example>
	/// <remarks>
	///		When applied to a method, indicates that the return value cannot be null.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class NotNullAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified element as never being <c>null</c>.
		/// </summary>
		public NotNullAttribute()
		{
		}
	}
}