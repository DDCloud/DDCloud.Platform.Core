using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter or method as referring to an MVC view.
	/// </summary>
	/// <remarks>
	/// 	If applied to a method, the MVC view name is calculated implicitly from the context.
	/// 
	/// 	Use this attribute for custom wrappers similar to Controller.View(Object).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	public sealed class AspMvcViewAttribute
		: PathReferenceAttribute
	{
		/// <summary>
		///		Mark the specified parameter or method as referring to an MVC view.
		/// </summary>
		public AspMvcViewAttribute()
		{
		}
	}
}