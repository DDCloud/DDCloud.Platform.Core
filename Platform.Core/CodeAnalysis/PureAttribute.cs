using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark a method as not making any observable state changes.
	/// </summary>
	/// <example>
	///     <code>
	///			[Pure]
	///			 private int Multiply(int x, int y)
	///			{
	///				return x*y;
	///			}
	/// 
	///			public void Foo()
	///			{
	///				const int a=2, b=2;
	///				Multiply(a, b); // Waring: Return value of pure method is not used
	///			}
	///		</code>
	/// </example>
	/// <remarks>
	///		Effectively the same as <see cref="System.Diagnostics.Contracts.PureAttribute" />.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public sealed class PureAttribute
		: Attribute
	{
	}
}