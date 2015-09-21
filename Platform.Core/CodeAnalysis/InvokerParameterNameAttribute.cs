using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark a parameter as requiring its value to be a string literal that matches one of the parameters of the calling method.
	/// </summary>
	/// <example>
	///     <code>
	///			public void Foo(string param)
	///			{
	///				if (param == null)
	///					throw new ArgumentNullException("par"); // Warning: Cannot resolve symbol
	///			}
	///		</code>
	/// </example>
	/// <remarks>
	///		 For example, ReSharper annotates the parameter of <see cref="ArgumentNullException" />.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public sealed class InvokerParameterNameAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified parameter as requiring its value to be a string literal that matches one of the parameters of the calling method.
		/// </summary>
		public InvokerParameterNameAttribute()
		{
		}
	}
}