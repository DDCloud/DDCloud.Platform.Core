using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{	
	/// <summary>
	///     Mark a parameter or method as referring to an MVC partial view.
	/// </summary>
	/// <remarks>
	///		If applied to a method, the MVC partial view name is calculated implicitly from the context.
	///		Use this attribute for custom wrappers similar to RenderPartialExtensions.RenderPartial(HtmlHelper, String).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	public sealed class AspMvcPartialViewAttribute
		: PathReferenceAttribute
	{
		/// <summary>
		///		Mark the specified parameter or method as referring to an MVC partial view.
		/// </summary>
		public AspMvcPartialViewAttribute()
		{
		}
	}
}