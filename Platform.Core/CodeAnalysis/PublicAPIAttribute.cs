using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark an element as being part of the publicly available API (and so should be both treated as used, and not be removed).
	/// </summary>
	[MeansImplicitUse]
	[
		AttributeUsage(
			AttributeTargets.Class
				| AttributeTargets.Struct
				| AttributeTargets.Interface
				| AttributeTargets.Enum
				| AttributeTargets.Delegate
				| AttributeTargets.Constructor
				| AttributeTargets.Event
				| AttributeTargets.Field
				| AttributeTargets.Method
				| AttributeTargets.Property,
			AllowMultiple = true,
			Inherited = false
		)
	]
	public sealed class PublicAPIAttribute
		: Attribute
	{
		/// <summary>
		///		A comment relating to the member being part of the public API.
		/// </summary>
		readonly string _comment;

		/// <summary>
		///		Mark the specified element as being part of the publicly available API (and so should be both treated as used, and not be removed).
		/// </summary>
		public PublicAPIAttribute()
		{
		}

		/// <summary>
		///		Mark the specified element as being part of the publicly available API (and so should be both treated as used, and not be removed).
		/// </summary>
		/// <param name="comment">
		///		A comment relating to the member being part of the public API.
		/// </param>
		public PublicAPIAttribute(string comment)
		{
			_comment = comment;
		}

		/// <summary>
		///		A comment relating to the member being part of the public API.
		/// </summary>
		public string Comment
		{
			get
			{
				return _comment;
			}
		}
	}
}