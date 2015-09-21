using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDCloud.Platform.Core.Linq
{
	/// <summary>
	///		Extension methods for <see cref="Task{TResult}">async</see> <see cref="IEnumerable{T}"/>s.
	/// </summary>
	public static class AsyncEnumerableExtensions
	{
		/// <summary>
		///		Asynchronously return the first element in the <see cref="IEnumerable{T}"/> sequence resulting from the <see cref="Task{TResult}"/>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncEnumerable">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IEnumerable{T}"/> sequence.
		/// </param>
		/// <returns>
		///		The first element in the sequence, or the default value for <typeparamref name="TSource"/> if the sequence contains no elements.
		/// </returns>
		public static async Task<TSource> FirstAsync<TSource>(this Task<IEnumerable<TSource>> asyncEnumerable)
		{
			if (asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			IEnumerable<TSource> enumerable = await asyncEnumerable;

			return enumerable.First();
		}

		/// <summary>
		///		Asynchronously return the first element in the <see cref="IEnumerable{T}"/> sequence resulting from the <see cref="Task{TResult}"/> for which the specified predicate is <c>true</c>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncEnumerable">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IEnumerable{T}"/> sequence.
		/// </param>
		/// <param name="predicate">
		///		The predicate used to test elements in the sequence.
		/// </param>
		/// <returns>
		///		The first element in the sequence, or the default value for <typeparamref name="TSource"/> if the sequence contains no elements.
		/// </returns>
		public static async Task<TSource> FirstAsync<TSource>(this Task<IEnumerable<TSource>> asyncEnumerable, Func<TSource, bool> predicate)
		{
			if (asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			IEnumerable<TSource> enumerable = await asyncEnumerable;

			return enumerable.First(predicate);
		}

		/// <summary>
		///		Asynchronously return the first element (or default <typeparamref name="TSource"/> value) in the <see cref="IEnumerable{T}"/> sequence resulting from the <see cref="Task{TResult}"/>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncEnumerable">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IEnumerable{T}"/> sequence.
		/// </param>
		/// <returns>
		///		The first element in the sequence, or the default value for <typeparamref name="TSource"/> if the sequence contains no elements.
		/// </returns>
		public static async Task<TSource> FirstOrDefaultAsync<TSource>(this Task<IEnumerable<TSource>> asyncEnumerable)
		{
			if (asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			IEnumerable<TSource> enumerable = await asyncEnumerable;

			return enumerable.FirstOrDefault();
		}

		/// <summary>
		///		Asynchronously return the first element (or default <typeparamref name="TSource"/> value) in the <see cref="IReadOnlyList{T}">read-only list</see> resulting from the <see cref="Task{TResult}"/>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncReadOnlyList">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IReadOnlyList{T}"/>.
		/// </param>
		/// <returns>
		///		The first element in the list, or the default value for <typeparamref name="TSource"/> if the list contains no elements.
		/// </returns>
		public static async Task<TSource> FirstOrDefaultAsync<TSource>(this Task<IReadOnlyList<TSource>> asyncReadOnlyList)
		{
			if (asyncReadOnlyList == null)
				throw new ArgumentNullException(nameof(asyncReadOnlyList));

			IEnumerable<TSource> enumerable = await asyncReadOnlyList;

			return enumerable.FirstOrDefault();
		}

		/// <summary>
		///		Asynchronously return the first element (or default <typeparamref name="TSource"/> value) in the <see cref="IEnumerable{T}"/> sequence resulting from the <see cref="Task{TResult}"/> for which the specified predicate is <c>true</c>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncEnumerable">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IEnumerable{T}"/> sequence.
		/// </param>
		/// <param name="predicate">
		///		The predicate used to test elements in the sequence.
		/// </param>
		/// <returns>
		///		The first element in the sequence, or the default value for <typeparamref name="TSource"/> if the sequence contains no elements.
		/// </returns>
		public static async Task<TSource> FirstOrDefaultAsync<TSource>(this Task<IEnumerable<TSource>> asyncEnumerable, Func<TSource, bool> predicate)
		{
			if (asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			IEnumerable<TSource> enumerable = await asyncEnumerable;

			return enumerable.FirstOrDefault(predicate);
		}

		/// <summary>
		///		Asynchronously return the only element in the <see cref="IEnumerable{T}"/> sequence resulting from the <see cref="Task{TResult}"/>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncEnumerable">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IEnumerable{T}"/> sequence.
		/// </param>
		/// <returns>
		///		The only element in the sequence, or the default value for <typeparamref name="TSource"/> if the sequence contains no elements.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The sequence does not contain exactly 1 element.
		/// </exception>
		public static async Task<TSource> SingleAsync<TSource>(this Task<IEnumerable<TSource>> asyncEnumerable)
		{
			if (asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			IEnumerable<TSource> enumerable = await asyncEnumerable;

			return enumerable.Single();
		}

		/// <summary>
		///		Asynchronously return the only element in the <see cref="IEnumerable{T}"/> sequence resulting from the <see cref="Task{TResult}"/> for which the specified predicate is <c>true</c>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncEnumerable">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IEnumerable{T}"/> sequence.
		/// </param>
		/// <param name="predicate">
		///		The predicate used to test elements in the sequence.
		/// </param>
		/// <returns>
		///		The only element in the sequence.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The sequence does not contain exactly 1 element for which the predicate is true.
		/// </exception>
		public static async Task<TSource> SingleAsync<TSource>(this Task<IEnumerable<TSource>> asyncEnumerable, Func<TSource, bool> predicate)
		{
			if (asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			IEnumerable<TSource> enumerable = await asyncEnumerable;

			return enumerable.Single(predicate);
		}

		/// <summary>
		///		Asynchronously return the only element (or default <typeparamref name="TSource"/> value) in the <see cref="IEnumerable{T}"/> sequence resulting from the <see cref="Task{TResult}"/>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncEnumerable">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IEnumerable{T}"/> sequence.
		/// </param>
		/// <returns>
		///		The only element in the sequence, or the default value for <typeparamref name="TSource"/> if the sequence contains no elements.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The sequence contains more than 1 element.
		/// </exception>
		public static async Task<TSource> SingleOrDefaultAsync<TSource>(this Task<IEnumerable<TSource>> asyncEnumerable)
		{
			if (asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			IEnumerable<TSource> enumerable = await asyncEnumerable;

			return enumerable.SingleOrDefault();
		}

		/// <summary>
		///		Asynchronously return the only element (or default <typeparamref name="TSource"/> value) in the <see cref="IEnumerable{T}"/> sequence resulting from the <see cref="Task{TResult}"/> for which the specified predicate is <c>true</c>.
		/// </summary>
		/// <typeparam name="TSource">
		///		The element type.
		/// </typeparam>
		/// <param name="asyncEnumerable">
		///		A <see cref="Task{TResult}"/> representing an asynchronous operation whose result is an <see cref="IEnumerable{T}"/> sequence.
		/// </param>
		/// <param name="predicate">
		///		The predicate used to test elements in the sequence.
		/// </param>
		/// <returns>
		///		The only element in the sequence, or the default value for <typeparamref name="TSource"/> if the sequence contains no elements.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The sequence contains more than 1 element for which the predicate is <c>true</c>.
		/// </exception>
		public static async Task<TSource> SingleOrDefaultAsync<TSource>(this Task<IEnumerable<TSource>> asyncEnumerable, Func<TSource, bool> predicate)
		{
			if (asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			IEnumerable<TSource> enumerable = await asyncEnumerable;

			return enumerable.SingleOrDefault(predicate);
		}
	}
}
