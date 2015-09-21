using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark a method as building strings using a format pattern and (optional) arguments.
	/// </summary>
	/// <example>
	///     <code>
	///			[StringFormatMethod("message")]
	///			public void ShowError(string message, params object[] args)
	///			{
	///				//Do something
	///			}
	/// 
	///			public void Foo()
	///			{
	///				ShowError("Failed: {0}"); // Warning: Non-existing argument in format string
	///			}
	///		</code>
	/// </example>
	/// <remarks>
	///		The format string should be in <see cref="String.Format(IFormatProvider,String,Object[])" />-like form
	/// </remarks>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class StringFormatMethodAttribute
		: Attribute
	{
		/// <summary>
		///     Mark the specified method as building strings using a format pattern and (optional) arguments.
		/// </summary>
		/// <param name="formatParameterName">
		///		The name of the method parameter that should be treated as a format-string.
		/// </param>
		public StringFormatMethodAttribute(string formatParameterName)
		{
			FormatParameterName = formatParameterName;
		}

		/// <summary>
		///     The name of the method parameter that should be treated as a format-string.
		/// </summary>
		[UsedImplicitly]
		public string FormatParameterName
		{
			get;
			private set;
		}
	}
}