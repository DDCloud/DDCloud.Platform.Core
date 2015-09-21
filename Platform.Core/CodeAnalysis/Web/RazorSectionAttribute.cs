using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter or method as referring to a Razor section.
	/// </summary>
	/// <remarks>
	///		Use this attribute for custom wrappers similar to WebPageBase.RenderSection(String).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method, Inherited = true)]
	public sealed class RazorSectionAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified parameter or method as referring to a Razor section.
		/// </summary>
		public RazorSectionAttribute()
		{
		}
	}
}
