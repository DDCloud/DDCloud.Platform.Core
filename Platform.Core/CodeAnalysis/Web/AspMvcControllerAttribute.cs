using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter or method as referring to an MVC controller name.
	/// </summary>
	/// <remarks>
	///		If applied to a method, the MVC controller name is calculated implicitly from the context.
	/// 
	///		Use this attribute for custom wrappers similar to ChildActionExtensions.RenderAction(HtmlHelper, String, String).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	public sealed class AspMvcControllerAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified member as referring to an MVC controller.
		/// </summary>
		public AspMvcControllerAttribute()
		{
		}

		/// <summary>
		///		Mark the specified member as referring to an MVC controller name.
		/// </summary>
		/// <param name="anonymousProperty">
		///		The property on the anonymous member type that refers to the controller.
		/// </param>
		/// <remarks>
		///		Use this overload for members whose type is anonymous.
		/// </remarks>
		public AspMvcControllerAttribute(string anonymousProperty)
		{
			AnonymousProperty = anonymousProperty;
		}

		/// <summary>
		///		The property on the anonymous member type that refers to the controller.
		/// </summary>
		[UsedImplicitly]
		public string AnonymousProperty
		{
			get;
			private set;
		}
	}
}