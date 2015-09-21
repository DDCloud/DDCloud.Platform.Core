using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter as referring to an MVC master page.
	/// </summary>
	/// <remarks>
	///		Use this attribute for custom wrappers similar to Controller.View(String, String).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AspMvcMasterAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified parameter as referring to an MVC master page.
		/// </summary>
		public AspMvcMasterAttribute()
		{
		}
	}
}