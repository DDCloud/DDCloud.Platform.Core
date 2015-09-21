using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Supresses all inspections for MVC views within a class or a method.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class AspMvcSupressViewErrorAttribute
		: Attribute
	{
		/// <summary>
		///		Suppress all inspections for MVC views within the specified class or method.
		/// </summary>
		public AspMvcSupressViewErrorAttribute()
		{
		}
	}
}