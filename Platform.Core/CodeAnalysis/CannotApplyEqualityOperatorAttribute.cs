using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark a type as not supporting comparison using the '==' or '!=' operators (<c>Equals()</c> should be used instead).
	/// </summary>
	/// <example>
	///     <code>
	///			[CannotApplyEqualityOperator]
	///			class NoEquality
	///			{
	///			}
	/// 
	///			class UsesNoEquality
	///			{
	///				public void Test()
	///				{
	///					NoEquality ne1 = new NoEquality();
	///					NoEquality ne2 = new NoEquality();
	/// 
	///					if (ne1 != null) // OK
	///					{
	///						bool condition = ne1 == ne2; // Warning
	///					}
	///				}
	///			}
	///		</code>
	/// </example>
	/// <remarks>
	///			Using '==' or '!=' for comparison with <c>null</c> is always permitted.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public sealed class CannotApplyEqualityOperatorAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified type as not supporting comparison using the '==' or '!=' operators (<c>Equals()</c> should be used instead).
		/// </summary>
		public CannotApplyEqualityOperatorAttribute()
		{
		}
	}
}