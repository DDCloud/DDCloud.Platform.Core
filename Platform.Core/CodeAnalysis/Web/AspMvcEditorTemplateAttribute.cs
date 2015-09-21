using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter as referring to an MVC editor template.
	/// </summary>
	/// <remarks>
	///		Use this attribute for custom wrappers similar to EditorExtensions.EditorForModel(HtmlHelper, String).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AspMvcEditorTemplateAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified parameter as referring to an MVC editor template.
		/// </summary>
		public AspMvcEditorTemplateAttribute()
		{
		}
	}
}