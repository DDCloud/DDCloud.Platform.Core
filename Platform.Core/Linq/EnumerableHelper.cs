using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DDCloud.Platform.Core.Linq
{
	/// <summary>
	///		Helper methods for <see cref="IEnumerable{T}"/>s.
	/// </summary>
	public static class EnumerableHelper
	{
		/// <summary>
		///		Perform an action for each element in the <see cref="IEnumerable{T}"/> sequence.
		/// </summary>
		/// <typeparam name="T">
		///		The sequence element type.
		/// </typeparam>
		/// <param name="enumerable">
		///		The sequence to enumerate.
		/// </param>
		/// <param name="forEachElement">
		///		An <see cref="Action{T}"/> delegate to be called once for each element in the sequence.
		/// </param>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> forEachElement)
		{
			if (enumerable == null)
				throw new ArgumentNullException(nameof(enumerable));

			if (forEachElement == null)
				throw new ArgumentNullException(nameof(forEachElement));

			foreach (T element in enumerable)
				forEachElement(element);
		}

		/// <summary>
		///		Perform an action for each element in the <see cref="IEnumerable{T}"/> sequence.
		/// </summary>
		/// <typeparam name="T">
		///		The sequence element type.
		/// </typeparam>
		/// <param name="enumerable">
		///		The sequence to enumerate.
		/// </param>
		/// <param name="forEachElement">
		///		A <see cref="Func{T1, TResult}"/> delegate to be called once for each element in the sequence. Enumeration will continue as long as the delegate returns <c>true</c>.
		/// </param>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Func<T, bool> forEachElement)
		{
			if (enumerable == null)
				throw new ArgumentNullException(nameof(enumerable));

			if (forEachElement == null)
				throw new ArgumentNullException(nameof(forEachElement));

			foreach (T element in enumerable)
			{
				if (!forEachElement(element))
					break;
			}
		}

		/// <summary>
		///		Skip all elements in the source sequence that are <c>null</c>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The source sequence element type.
		/// </typeparam>
		/// <param name="source">
		///		The source sequence.
		/// </param>
		/// <returns>
		///		A sequence that yields only non-null elements from the sequence.
		/// </returns>
		public static IEnumerable<TSource> SkipNull<TSource>(this IEnumerable<TSource> source)
			where TSource : class
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			return source.Where(
				sourceElement =>
					sourceElement != null
			);
		}

		/// <summary>
		///		Wrap the <see cref="IEnumerator{T}">typed enumerator</see> in a non-thread-safe <see cref="IEnumerable{T}">typed enumerable</see>.
		/// </summary>
		/// <typeparam name="T">
		///		The enumerator element type.
		/// </typeparam>
		/// <param name="enumerator">
		///		The typed enumerator.
		/// </param>
		/// <param name="dispose">
		///		<see cref="IDisposable.Dispose"/> of the <paramref name="enumerator"/> when the resulting sequence is complete?
		/// 
		///		Default is <c>false</c>.
		/// </param>
		/// <param name="reset">
		///		<see cref="IEnumerator.Reset"/> the enumerator when starting to enumerate the <see cref="IEnumerable{T}">enumerable</see>?
		/// 
		///		Default is <c>true</c>.
		/// </param>
		/// <returns>
		///		An <see cref="IEnumerable{T}"/> representing the sequence yielded by the <paramref name="enumerator"/>.
		/// </returns>
		/// <remarks>
		///		AF: A note on thread-safety: calling this method more than once on the same <see cref="IEnumerator{T}">enumerator</see> will yield undefined behaviour if multiple instances of the returned <see cref="IEnumerable{T}">enumerable</see> are used concurrently (they all target the same enumerator). As an almost universal rule, DON'T do it. You'll be sorry.
		/// </remarks>
		public static IEnumerable<T> AsEnumerableNotThreadSafe<T>(this IEnumerator<T> enumerator, bool dispose = false, bool reset = true)
		{
			if (enumerator == null)
				throw new ArgumentNullException(nameof(enumerator));

			try
			{
				if (reset)
					enumerator.Reset();

				while (enumerator.MoveNext())
					yield return enumerator.Current;
			}
			finally
			{
				if (dispose)
					enumerator.Dispose();
			}
		}

		/// <summary>
		///		Create an <see cref="IEnumerable{T}"/> sequence that yields a single value.
		/// </summary>
		/// <typeparam name="T">
		///		The type of value to yield.
		/// </typeparam>
		/// <param name="value">
		///		The value to yield.
		/// </param>
		/// <returns>
		///		The <see cref="IEnumerable{T}"/> sequence.
		/// </returns>
		public static IEnumerable<T> YieldSingleValue<T>(this T value)
		{
			yield return value;
		}

		/// <summary>
		///		Sort the items in the sequence using each item as its own sort key.
		/// </summary>
		/// <typeparam name="T">
		///		The sequence element type.
		/// </typeparam>
		/// <param name="source">
		///		The sequence to sort.
		/// </param>
		/// <returns>
		///		An <see cref="IOrderedEnumerable{TElement}"/> representing the sorted sequence.
		/// </returns>
		public static IOrderedEnumerable<T> Sort<T>(this IEnumerable<T> source)
		{
			if (source == null)
				throw new System.ArgumentNullException(nameof(source));

			return source.OrderBy(item => item);
		}

		/// <summary>
		///		Sort the items in the sequence using each item as its own sort key.
		/// </summary>
		/// <typeparam name="T">
		///		The sequence element type.
		/// </typeparam>
		/// <param name="source">
		///		The sequence to sort.
		/// </param>
		/// <param name="comparer">
		///		A comparer to use for determining relative item order.
		/// </param>
		/// <returns>
		///		An <see cref="IOrderedEnumerable{TElement}"/> representing the sorted sequence.
		/// </returns>
		public static IOrderedEnumerable<T> Sort<T>(this IEnumerable<T> source, IComparer<T> comparer)
		{
			if (source == null)
				throw new System.ArgumentNullException(nameof(source));

			if (comparer == null)
				throw new System.ArgumentNullException(nameof(comparer));

			return source.OrderBy(item => item, comparer);
		}

		/// <summary>
		///		Sort the items in the sequence in descending order using each item as its own sort key.
		/// </summary>
		/// <typeparam name="T">
		///		The sequence element type.
		/// </typeparam>
		/// <param name="source">
		///		The sequence to sort.
		/// </param>
		/// <returns>
		///		An <see cref="IOrderedEnumerable{TElement}"/> representing the sorted sequence.
		/// </returns>
		public static IOrderedEnumerable<T> SortDescending<T>(this IEnumerable<T> source)
		{
			if (source == null)
				throw new System.ArgumentNullException(nameof(source));

			return source.OrderByDescending(item => item);
		}

		/// <summary>
		///		Sort the items in the sequence in descending order using each item as its own sort key.
		/// </summary>
		/// <typeparam name="T">
		///		The sequence element type.
		/// </typeparam>
		/// <param name="source">
		///		The sequence to sort.
		/// </param>
		/// <param name="comparer">
		///		A comparer to use for determining relative item order.
		/// </param>
		/// <returns>
		///		An <see cref="IOrderedEnumerable{TElement}"/> representing the sorted sequence.
		/// </returns>
		public static IOrderedEnumerable<T> SortDescending<T>(this IEnumerable<T> source, IComparer<T> comparer)
		{
			if (source == null)
				throw new System.ArgumentNullException(nameof(source));

			if (comparer == null)
				throw new System.ArgumentNullException(nameof(comparer));

			return source.OrderByDescending(item => item, comparer);
		}

		/// <summary>
		///		Prepend an item to the sequence.
		/// </summary>
		/// <typeparam name="T">
		///		The sequence item type.
		/// </typeparam>
		/// <param name="source">
		///		The sequence to which the item will be appended.
		/// </param>
		/// <param name="prependItem">
		///		The item to append.
		/// </param>
		/// <returns>
		///		A sequence that <paramref name="prependItem"/>, then yields all the items of the source sequence.
		/// </returns>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T prependItem)
		{
			if (source == null)
				throw new System.ArgumentNullException(nameof(source));

			yield return prependItem;

			foreach (T item in source)
				yield return item;
		}

		/// <summary>
		///		Append an item to the sequence.
		/// </summary>
		/// <typeparam name="T">
		///		The sequence item type.
		/// </typeparam>
		/// <param name="source">
		///		The sequence to which the item will be appended.
		/// </param>
		/// <param name="appendItem">
		///		The item to append.
		/// </param>
		/// <returns>
		///		A sequence that yields all the items of the source sequence, then yields <paramref name="appendItem"/>.
		/// </returns>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T appendItem)
		{
			if (source == null)
				throw new System.ArgumentNullException(nameof(source));

			foreach (T item in source)
				yield return item;

			yield return appendItem;
		}
	}
}
