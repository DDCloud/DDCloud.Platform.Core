using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark an element as requiring localization (or not).
	/// </summary>
	/// <example>
	///     <code>
	///			[LocalizationRequiredAttribute(true)]
	///			public class Foo
	///			{
	///				private string str = "my string"; // Warning: Localizable string
	///			}
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class LocalizationRequiredAttribute : Attribute
	{
		/// <summary>
		///     Mark the specified element as requiring localization.
		/// </summary>
		public LocalizationRequiredAttribute()
			: this(true)
		{
		}

		/// <summary>
		///     Mark the specified element as requiring localization.
		/// </summary>
		/// <param name="required">
		///     <c>true</c> if the element should be localized; otherwise, <c>false</c>.
		/// </param>
		public LocalizationRequiredAttribute(bool required)
		{
			Required = required;
		}

		/// <summary>
		///     Should the marked element localized?
		/// </summary>
		[UsedImplicitly]
		public bool Required
		{
			get;
			private set;
		}

		/// <summary>
		///     Determine whether the value of the specified object is equal to the current <see cref="LocalizationRequiredAttribute" />.
		/// </summary>
		/// <param name="obj">
		///		The object to test for equality.
		/// </param>
		/// <returns>
		///     <c>true</c> if the value of the given object is equal to that of the current; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			var attribute = obj as LocalizationRequiredAttribute;
			return attribute != null && attribute.Required == Required;
		}

		/// <summary>
		///     Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		///     A hash code for the current <see cref="LocalizationRequiredAttribute" />.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}