using System;
using System.IO;

namespace DDCloud.Platform.Core.Utilities
{
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	///		Helper methods for working with paths.
	/// </summary>
	public static class PathHelper
	{
		/// <summary>
		///		Get the path of the directory, relative to another directory.
		/// </summary>
		/// <param name="directory">
		///		The directory to calculate a relative path for.
		/// </param>
		/// <param name="relativeToDirectory">
		///		The directory that the calculated path will be relative to.
		/// </param>
		/// <returns>
		///		The relative directory path.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "AF: No, the distinction is semantically significant.")]
		public static string GetRelativePath(this DirectoryInfo directory, DirectoryInfo relativeToDirectory)
		{
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));

			if (relativeToDirectory == null)
				throw new ArgumentNullException(nameof(relativeToDirectory));

			Uri directoryUri = new Uri(directory.FullName, UriKind.Absolute);
			Uri relativeToDirectoryUri = new Uri(relativeToDirectory.FullName, UriKind.Absolute);

			return
				relativeToDirectoryUri
					.MakeRelativeUri(directoryUri)
					.LocalPath;
		}

		/// <summary>
		///		Get the path of the file, relative to the specified directory.
		/// </summary>
		/// <param name="file">
		///		The file to calculate a relative path for.
		/// </param>
		/// <param name="relativeToDirectory">
		///		The directory that the calculated path will be relative to.
		/// </param>
		/// <returns>
		///		The relative file path.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "AF: No, the distinction is semantically significant.")]
		public static string GetRelativePath(this FileInfo file, DirectoryInfo relativeToDirectory)
		{
			if (file == null)
				throw new ArgumentNullException(nameof(file));

			if (relativeToDirectory == null)
				throw new ArgumentNullException(nameof(relativeToDirectory));

			Uri directoryUri = new Uri(file.FullName, UriKind.Absolute);
			Uri relativeToDirectoryUri = new Uri(relativeToDirectory.FullName, UriKind.Absolute);

			return
				relativeToDirectoryUri
					.MakeRelativeUri(directoryUri)
					.LocalPath;
		}
	}
}
