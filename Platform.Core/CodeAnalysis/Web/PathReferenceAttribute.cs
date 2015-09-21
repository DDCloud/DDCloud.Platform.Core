using System;

namespace DDCloud.Platform.Core.CodeAnalysis.Web
{
	/// <summary>
	///     Mark a parameter as referring to a file or folder path within a web project.
	/// </summary>
	/// <remarks>
	///		The path can be relative or absolute, starting from web root (~).
	/// </remarks>
	[AttributeUsage(AttributeTargets.Parameter)]
	public class PathReferenceAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified parameter as referring to a file or folder path within a web project.
		/// </summary>
		public PathReferenceAttribute()
		{
		}

		/// <summary>
		///		Mark the specified parameter as referring to a file or folder path within a web project.
		/// </summary>
		/// <param name="basePath">
		///		The base path, relative to which the path is evaluated.
		/// </param>
		[UsedImplicitly]
		public PathReferenceAttribute([PathReference] string basePath)
		{
			BasePath = basePath;
		}

		/// <summary>
		///		The base path, relative to which the path is evaluated.
		/// </summary>
		[UsedImplicitly]
		public string BasePath
		{
			get;
			private set;
		}
	}
}