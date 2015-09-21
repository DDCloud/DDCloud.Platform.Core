using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///    Marks a parameter as referring to an MVC display template.
	/// </summary>
	/// <remarks>
	///		Use this attribute for custom wrappers similar to DisplayExtensions.DisplayForModel(HtmlHelper, String).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AspMvcDisplayTemplateAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified parameter as referring to an MVC display template.
		/// </summary>
		public AspMvcDisplayTemplateAttribute()
		{
		}
	}
}