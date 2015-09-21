using System;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		Helper methods for working with strings.
	/// </summary>
	public static class StringHelper
	{
		/// <summary>
		///		An cached empty string array to be returned from helper methods.
		/// </summary>
		static readonly string[] EmptyStringArray = new string[0];

		/// <summary>
		///		Truncate the string to the specified number of characters.
		/// </summary>
		/// <param name="toTruncate">
		///		The string to truncate.
		/// </param>
		/// <param name="maxLength">
		///		The maximum number of characters that the string should contain.
		/// </param>
		/// <returns>
		///		The truncated string, unless the string is already less than or equal to the specified maximum length (in which case the original string is returned).
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="toTruncate"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="maxLength"/> is less than 0.
		/// </exception>
		public static string Truncate(this string toTruncate, int maxLength)
		{
			if (toTruncate == null)
				throw new ArgumentNullException(nameof(toTruncate));

			if (maxLength < 0)
				throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum string length cannot be less than 0.");

			if (maxLength == 0)
				return String.Empty;

			if (toTruncate.Length <= maxLength)
				return toTruncate;

			return toTruncate.Substring(0, maxLength);
		}

		/// <summary>
		///		Safely trim leading and trailing whitespace from the string, returning <c>null</c> if it is <c>null</c>.
		/// </summary>
		/// <param name="toTrim">
		///		The string to trim.
		/// </param>
		/// <returns>
		///		The trimmed string, or <c>null</c> if <paramref name="toTrim"/> is <c>null</c>.
		/// </returns>
		public static string SafeTrim(this string toTrim)
		{
			return toTrim?.Trim();
		}

		/// <summary>
		///		Safely split the string, returning an empty array if it is <c>null</c>.
		/// </summary>
		/// <param name="toSplit">
		///		The string to split.
		/// </param>
		/// <param name="splitOn">
		///		The separator character on which the split the string.
		/// </param>
		/// <param name="includeEmptyValues">
		///		Include empty values in the resulting array?
		/// 
		///		Default is <c>true</c>.
		/// </param>
		/// <returns>
		///		An array of strings representing the split string. If <paramref name="toSplit"/> is <c>null</c>, an empty array.
		/// </returns>
		public static string[] SafeSplit(this string toSplit, char splitOn, bool includeEmptyValues = false)
		{
			if (toSplit == null)
				return EmptyStringArray;

			return
				toSplit.Split(
					new char[]
					{
						splitOn
					},
					includeEmptyValues ?
						StringSplitOptions.None
						:
						StringSplitOptions.RemoveEmptyEntries
				);
		}
	}
}
