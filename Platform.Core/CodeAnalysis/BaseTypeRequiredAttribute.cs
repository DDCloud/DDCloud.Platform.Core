using System;
using System.Diagnostics.CodeAnalysis;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Marks a type as requiring inheritors / implementors to inherit a specific base type or implement an interface.
	/// </summary>
	/// <example>
	///     <code>
	///			[BaseTypeRequired(typeof(IComponent)] // Specify requirement
	///			public class ComponentAttribute
	///				: Attribute 
	///			{
	///			}
	/// 
	///			[Component] // ComponentAttribute requires implementing IComponent interface
	///			public class MyComponent
	///				: IComponent
	///			{
	///			}
	/// </code>
	/// </example>
	[BaseTypeRequired(typeof(Attribute))]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class BaseTypeRequiredAttribute
		: Attribute
	{
		/// <summary>
		///		The base type.
		/// </summary>
		readonly Type		_baseType;

		/// <summary>
		///		Base types.
		/// </summary>
		/// <remarks>
		///		Used by Resharper's code-analysis engine.
		/// </remarks>
		readonly Type[]		_baseTypes = new Type[1];

		/// <summary>
		///     Mark the specified type as requiring inheritors / implementors to inherit a specific base type or implement an interface.
		/// </summary>
		/// <param name="baseType">
		///		The type of base class or type that inheritors must implement.
		/// </param>
		public BaseTypeRequiredAttribute(Type baseType)
		{
			if (baseType == null)
				throw new ArgumentNullException(nameof(baseType));

			_baseTypes[0] = _baseType = baseType;
		}

		/// <summary>
		///		The base type.
		/// </summary>
		public Type BaseType
		{
			get
			{
				return _baseType;
			}
		}

		/// <summary>
		///     The required base types.
		/// </summary>
		/// <remarks>
		///		Used by Resharper's code-analysis engine.
		/// </remarks>
		[
			SuppressMessage(
				"Microsoft.Performance",
				"CA1819:PropertiesShouldNotReturnArrays",
				Justification = "Required by Resharper"
			)
		]
		public Type[] BaseTypes
		{
			get
			{
				return _baseTypes;
			}
		}
	}
}