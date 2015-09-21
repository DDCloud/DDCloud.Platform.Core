using System.Collections.Generic;
using System.Linq;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///     Simple implementation of hash codes for aggregate values.
	/// </summary>
	public static class HashCodeCombiner
	{
		#region Constants

		/// <summary>
		///     Magic number that acts as a seed value for hash-codes.
		/// </summary>
		/// <remarks>
		///     AF: It's a prime number; please don't ask me why this works, I don't remember the maths behind it.
		/// </remarks>
		public const int HashCodeSeed		= 17;

		/// <summary>
		///     Magic number that acts as a multiplier after each hash-code addition.
		/// </summary>
		/// <remarks>
		///     See the remarks for <see cref="HashCodeSeed" />.
		/// </remarks>
		public const int HashCodeMultiplier	= 37;

		#endregion Constants

		#region Public methods

		/// <summary>
		///		Combine the hash-codes of the specified objects.
		/// </summary>
		/// <param name="values">
		///		The objects to combine.
		/// </param>
		/// <returns>
		///		The combined hash-code (generated using addition combined with prime multiplication).
		/// </returns>
		public static int CombineHashCodes(params object[] values)
		{
			return
				values != null ?
					CombineHashCodes(values.AsEnumerable())
					:
					0;
		}

		/// <summary>
		///		Combine the hash-codes of the specified objects.
		/// </summary>
		/// <param name="objects">
		///		The objects whose hash-codes are to be combined.
		/// </param>
		/// <returns>
		///		The combined hash-code (generated using addition combined with prime multiplication).
		/// </returns>
		public static int CombineHashCodes(this IEnumerable<object> objects)
		{
			if (objects == null)
				return 0;

			int hashCode = HashCodeSeed;
			foreach (object currentObject in objects)
			{
				// Wrap around on overflow.
				unchecked
				{
					if (currentObject != null)
						hashCode += currentObject.GetHashCode();

					hashCode *= HashCodeMultiplier;
				}
			}

			return hashCode;
		}

		/// <summary>
		///		Combine the hash-codes of the specified objects, using the specified <see cref="IEqualityComparer{T}">equality comparer</see> to generate a hash code for each object.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects whose hash-codes are to be combined.
		/// </typeparam>
		/// <param name="objects">
		///		The objects whose hash-codes are to be combined.
		/// </param>
		/// <param name="comparer">
		///		An <see cref="IEqualityComparer{T}"/> to use for generating hash-codes for each <typeparamref name="T"/> in <paramref name="objects"/>.
		/// </param>
		/// <returns>
		///		The combined hash-code (generated using addition combined with prime multiplication).
		/// </returns>
		public static int CombineHashCodes<T>(this IEnumerable<T> objects, IEqualityComparer<T> comparer)
		{
			if (objects == null)
				return 0;

			if (comparer == null)
				comparer = EqualityComparer<T>.Default;

			int hashCode = HashCodeSeed;
			foreach (T currentObject in objects)
			{
				// Wrap around on overflow.
				unchecked
				{
					if (currentObject != null)
						hashCode += comparer.GetHashCode(currentObject);

					hashCode *= HashCodeMultiplier;
				}
			}

			return hashCode;
		}

		#endregion // Public methods
	}
}
