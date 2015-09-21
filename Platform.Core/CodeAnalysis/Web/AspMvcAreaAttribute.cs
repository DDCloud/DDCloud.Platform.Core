using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter as referring to an MVC area name.
	/// </summary>
	/// <remarks>
	///		If applied to a method, the MVC area name is calculated implicitly from the context.
	///		Use this attribute for custom wrappers similar to ChildActionExtensions.RenderAction(HtmlHelper, String).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AspMvcAreaAttribute
		: PathReferenceAttribute
	{
		/// <summary>
		///		Mark the specified parameter as referring to an MVC area name.
		/// </summary>
		[UsedImplicitly]
		public AspMvcAreaAttribute()
		{
		}

		/// <summary>
		///		Mark the specified property of the specified parameter's type as referring to an MVC area name.
		/// </summary>
		/// <param name="anonymousProperty">
		///		The name of the anonymous type property that refers to the MVC area name.
		/// </param>
		/// <remarks>
		///		Used for parameters whose type is anonymous.
		/// </remarks>
		public AspMvcAreaAttribute(string anonymousProperty)
		{
			AnonymousProperty = anonymousProperty;
		}

		/// <summary>
		///		The name of the anonymous type property that refers to the MVC area name.
		/// </summary>
		[UsedImplicitly]
		public string AnonymousProperty
		{
			get;
			private set;
		}
	}
}