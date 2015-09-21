using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter as referring to, or a method as implementing, an ASP.NET MVC action.
	/// </summary>
	/// <remarks>
	///		If applied to a method, the MVC action name is calculated implicitly from the context.
	/// 
	///     Use this attribute for custom wrappers similar to ChildActionExtensions.RenderAction(HtmlHelper, String).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
	public sealed class AspMvcActionAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the target member as referring to or implementing an ASP.NET MVC action.
		/// </summary>
		public AspMvcActionAttribute()
		{
		}

		/// <summary>
		///		Mark the specified property of the target parameter as referring to an ASP.NET MVC action.
		/// </summary>
		/// <param name="anonymousProperty">
		///		The property on the anonymous type that refers to the action.
		/// </param>
		/// <remarks>
		///		Use this overload for parameters whose type is anonymous.
		/// </remarks>
		public AspMvcActionAttribute(string anonymousProperty)
		{
			AnonymousProperty = anonymousProperty;
		}

		/// <summary>
		///		The property on the anonymous type that refers to the action.
		/// </summary>
		[UsedImplicitly]
		public string AnonymousProperty
		{
			get;
			private set;
		}
	}
}