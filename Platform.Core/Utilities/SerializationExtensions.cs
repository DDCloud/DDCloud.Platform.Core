using System;
using System.Runtime.Serialization;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		Extension methods for working with <see cref="SerializationInfo"/>.
	/// </summary>
	public static class SerializationExtensions
	{
		/// <summary>
		///		Get the specified value from the <see cref="SerializationInfo"/>.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value to retrieve.
		/// </typeparam>
		/// <param name="serializationInfo">
		///		The <see cref="SerializationInfo"/>.
		/// </param>
		/// <param name="name">
		///		The name of the value to retrieve.
		/// </param>
		/// <returns>
		///		The value, as a <typeparamref name="TValue"/>.
		/// </returns>
		public static TValue GetValue<TValue>(this SerializationInfo serializationInfo, string name)
		{
			if (serializationInfo == null)
				throw new ArgumentNullException(nameof(serializationInfo));

			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(name));

			return (TValue)serializationInfo.GetValue(name, typeof(TValue));
		}
	}
}
