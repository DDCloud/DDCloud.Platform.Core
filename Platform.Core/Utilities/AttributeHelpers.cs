using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		Helper methods for working with custom attributes.
	/// </summary>
	public static class AttributeHelpers
	{
		/// <summary>
		///		Get the valid <see cref="AttributeTargets">attribute targets</see> value for the specified custom attribute type.
		/// </summary>
		/// <typeparam name="TAttribute">
		///		The custom attribute type.
		/// </typeparam>
		/// <returns>
		///		An	<see cref="AttributeTargets"/> value representing the attribute's valid targets.
		/// </returns>
		public static AttributeTargets GetAttributeTargets<TAttribute>()
			where TAttribute : Attribute
		{
			Type attributeType = typeof(TAttribute);
			AttributeUsageAttribute usageAttribute =
				attributeType
					.GetCustomAttribute<AttributeUsageAttribute>();

			return
				usageAttribute == null ?
					AttributeTargets.All
					:
					usageAttribute.ValidOn;
		}

		/// <summary>
		///		Does the specified member have a custom attribute.
		/// </summary>
		/// <typeparam name="TAttribute">
		///		The custom attribute type.
		/// </typeparam>
		/// <param name="member">
		///		The member.
		/// </param>
		/// <param name="inherited">
		///		Include inherited attributes?
		///		Default is false.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the <paramref name="member"/> has a <typeparamref name="TAttribute"/> custom attribute applied.
		/// </returns>
		public static bool HasCustomAttribute<TAttribute>(this MemberInfo member, bool inherited = false)
			where TAttribute : Attribute
		{
			if (member == null)
				throw new ArgumentNullException(nameof(member));

			return
				member
					.GetCustomAttributes<TAttribute>(inherited)
					.Any();
		}

		/// <summary>
		///		Return the value from the <see cref="DataMemberAttribute"/>'s name.
		/// </summary>
		/// <param name="objectType">
		///		The object type to get the attribute from.
		/// </param>
		/// <param name="propertyName">
		///		The property name to get the attribute from.
		/// </param>
		/// <returns>
		///		The value from <see cref="DataMemberAttribute"/>'s name, or null if not defined.
		/// </returns>
		public static string GetDataMemberName(this Type objectType, string propertyName)
		{
			if (objectType == null)
				throw new ArgumentNullException(nameof(objectType));

			if (String.IsNullOrEmpty(propertyName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'propertyName'.", nameof(propertyName));

			PropertyInfo property = objectType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				throw new MissingMemberException(
					$"Property '{propertyName}' not found on type '{objectType.FullName}'."
				);
			}

			DataMemberAttribute attribute = property.GetCustomAttribute<DataMemberAttribute>();

			return attribute?.Name;
		}
	}
}