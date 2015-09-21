using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter or property of a custom attribute as referring to an MVC action name.
	/// </summary>
	/// <example>
	///     <code>
	/// 		[ActionName("Foo")]
	/// 		public ActionResult Login(string returnUrl)
	/// 		{
	/// 			ViewBag.ReturnUrl = Url.Action("Foo"); // OK
	/// 
	/// 			return RedirectToAction("Bar"); // Error: Cannot resolve action
	/// 		}
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	public sealed class AspMvcActionSelectorAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified member as referring to an MVC action name.
		/// </summary>
		public AspMvcActionSelectorAttribute()
		{
		}
	}
}