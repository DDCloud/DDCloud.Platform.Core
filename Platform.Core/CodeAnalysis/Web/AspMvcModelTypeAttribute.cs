using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Marks a parameter as referring to an MVC model type.
	/// </summary>
	/// <remarks>
	///		Use this attribute for custom wrappers similar to Controller.View(String, Object).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AspMvcModelTypeAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified parameter as referring to an MVC model type.
		/// </summary>
		public AspMvcModelTypeAttribute()
		{
		}
	}
}