using System;
using System.Collections.Generic;

namespace DDCloud.Platform.Core.Collections
{
	/// <summary>
	///		Helper methods for working with <see cref="IDictionary{TKey,TValue}">dictionaries</see>.
	/// </summary>
	public static class DictionaryHelper
	{
		/// <summary>
		///		Try to retrieve a value from the dictionary, and down-cast it to the specified type.
		/// </summary>
		/// <typeparam name="TKey">
		///		The type of key used to identify items in the dictionary.
		/// </typeparam>
		/// <typeparam name="TValueLimit">
		///		The limiting (least derived) type of value contained in the dictionary.
		/// </typeparam>
		/// <typeparam name="TValue">
		///		The type of value to retrieve.
		/// </typeparam>
		/// <param name="dictionary">
		///		The dictionary.
		/// </param>
		/// <param name="key">
		///		The key that identifies the item to retrieve.
		/// </param>
		/// <param name="value">
		///		Receives the value to retrieve, or <c>default(</c><typeparamref name="TValue"/><c>)</c> if the <paramref name="key"/> is not present in the dictionary.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the <paramref name="key"/> is present in the dictionary, otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="InvalidCastException">
		///		The retrieved value could not be down-cast from <typeparamref name="TValueLimit"/> to <typeparamref name="TValue"/>.
		/// </exception>
		public static bool TryCastValue<TKey, TValueLimit, TValue>(this IDictionary<TKey, TValueLimit> dictionary, TKey key, out TValue value)
			where TValueLimit : class
			where TValue : TValueLimit
		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			TValueLimit uncastValue;
			if (!dictionary.TryGetValue(key, out uncastValue))
			{
				value = default(TValue);

				return false;
			}

			value = (TValue)uncastValue;

			return true;
		}
	}
}
