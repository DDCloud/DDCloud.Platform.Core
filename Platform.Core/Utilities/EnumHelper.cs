using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
    ///		Helper methods for working with enums.
    /// </summary>
    /// <remarks>
    ///		Does not currently support enums with duplicate or negative values.
    /// </remarks>
    public static class EnumHelper
    {
		/// <summary>
		///		Get all members for the specified <see cref="Enum">enum</see> <typeparamref name="TEnum">type</typeparamref>.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> data-type.
		/// </typeparam>
		/// <returns>
		///		A read-only list of <typeparamref name="TEnum"/> member values.
		/// </returns>
		public static IReadOnlyList<TEnum> AllMembers<TEnum>()
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return MetadataFor<TEnum>.Members;
		}

		/// <summary>
		///		Is the specified <typeparamref name="TEnum"/> value equal to 0?
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="value">
		///		The value to compare.
		/// </param>
		/// <returns>
		///		<c>true</c>, the <paramref name="value"/>'s <see cref="Int32"/> equivalent is equal to 0.
		/// </returns>
		public static bool IsZero<TEnum>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return MetadataFor<TEnum>.Delegates.ToInt32(value) == 0; // AF: Yesyesyes, you could use value.Equals(default(TEnum)), but this is less obscure.
		}

        /// <summary>
        ///		Parse the specified value as a <typeparamref name="TEnum"/>.
        /// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
        /// <param name="value">
        ///		The value to parse.
        /// </param>
        /// <param name="ignoreCase">
        ///		Ignore case when parsing?
        /// </param>
        /// <returns>
        ///		The <typeparamref name="TEnum"/> value.
        /// </returns>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase = false)
			where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException(@"Argument cannot be null, empty, or composed entirely of whitespace: 'value'.", nameof(value));

            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        /// <summary>
        ///		Try to parse the specified value as a <typeparamref name="TEnum"/>.
        /// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
        /// <param name="value">
        ///		The value to parse.
        /// </param>
        /// <param name="defaultValue">
        ///		The default value to return if <paramref name="value"/> cannot be parsed into a <typeparamref name="TEnum"/>.
        /// </param>
        /// <param name="ignoreCase">
        ///		Ignore case when parsing?
        /// </param>
        /// <returns>
        ///		The <typeparamref name="TEnum"/> value, or <paramref name="defaultValue"/> if <paramref name="value"/> cannot be parsed as a <typeparamref name="TEnum"/>.
        /// </returns>
        public static TEnum ParseOrDefault<TEnum>(string value, TEnum defaultValue = default(TEnum), bool ignoreCase = false)
			where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            if (String.IsNullOrWhiteSpace(value))
                return defaultValue;

            TEnum parsedValue;
            if (Enum.TryParse(value, ignoreCase, out parsedValue))
                return parsedValue;

            return defaultValue;
        }

        /// <summary>
        ///		Determine whether the specified value is defined on <typeparamref name="TEnum"/>.
        /// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
        /// <param name="value">
        ///		The value.
        /// </param>
        /// <returns>
        ///		<c>true</c>, if <paramref name="value"/> represents either a value or a combination of values from <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined<TEnum>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            return Array.BinarySearch(MetadataFor<TEnum>.Values, value) >= 0;
        }

        /// <summary>
        ///		Determine whether the specified string value is defined on <typeparamref name="TEnum"/>.
        /// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
        /// <param name="value">
        ///		The value.
        /// </param>
        /// <param name="ignoreCase">
        ///		Ignore case when comparing value names?
        /// </param>
        /// <returns>
        ///		<c>true</c>, if <paramref name="value"/> represents either a value or a combination of values from <typeparamref name="TEnum"/>.
        /// </returns>
        public static bool IsDefined<TEnum>(string value, bool ignoreCase = false)
			where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value)); // No nulls.

            if (String.IsNullOrWhiteSpace(value))
                return false; // But empty or whitespace strings, we're OK with.

			// TODO: Store a HashSet<string> for better performance.
            return MetadataFor<TEnum>.ValueNames.Contains(
				value,
				ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal
			);
        }

	    /// <summary>
		///		Create a composite <typeparamref name="TEnum"/> value from the supplied <typeparamref name="TEnum"/> values.
	    /// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
	    /// <param name="values">
		///		The <typeparamref name="TEnum"/> values to compose.
	    /// </param>
	    /// <returns>
		///		The composite <typeparamref name="TEnum"/> value.
	    /// </returns>
	    public static TEnum Compose<TEnum>(params TEnum[] values)
			where TEnum : struct, IComparable, IFormattable, IConvertible
	    {
		    if (values == null)
			    throw new ArgumentNullException(nameof(values));

			if (values.Length == 0)
				throw new ArgumentException("Must supply at least one value to compose.");

		    if (values.Length == 1)
			    return values[0];

			return Compose(
				(IEnumerable<TEnum>)values
			);
	    }

		/// <summary>
		///		Create a composite <typeparamref name="TEnum"/> value from the supplied <typeparamref name="TEnum"/> values.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="values">
		///		The <typeparamref name="TEnum"/> values to compose.
		/// </param>
		/// <returns>
		///		The composite <typeparamref name="TEnum"/> value.
		/// </returns>
		public static TEnum Compose<TEnum>(IEnumerable<TEnum> values)
			where TEnum : struct, IComparable, IFormattable, IConvertible
	    {
			if (values == null)
				throw new ArgumentNullException(nameof(values));

			int combinedFlagValues =
				values
					.Select(MetadataFor<TEnum>.Delegates.ToInt32)
					.Aggregate(
						seed: 0,
						func: 
							(previous, current) => previous | current
					);

			return MetadataFor<TEnum>.Delegates.ToEnum(
				combinedFlagValues
			);
	    }

        /// <summary>
        ///		Decompose the specified enum value into its constituent enum values.
        /// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
        /// <param name="value">
        ///		The value to decompose.
        /// </param>
        /// <returns>
        ///		A sequence of 1 or more <typeparamref name="TEnum"/> values.
        /// </returns>
        /// <remarks>
        ///		AF: Currently does not handle enums with members that have negative values.
        /// </remarks>
        public static IEnumerable<TEnum> Decompose<TEnum>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            if (!MetadataFor<TEnum>.CanBeComposite)
                throw new InvalidOperationException(String.Format("Enum type '{0}' is not marked as a composable enum type.", typeof(TEnum).FullName));

			int remainder = MetadataFor<TEnum>.Delegates.ToInt32(value);

            // Logical short-circuit.
            if (remainder == 0)
            {
                yield return value; // Defined or not, it is what it is.

                yield break;
            }

            // Because values are in ascending order, we'll always hit (and eliminate) non-composite values first.
			for (int valueIndex = 0; valueIndex < MetadataFor<TEnum>.IntValues.Length; valueIndex++)
            {
				int testValue = MetadataFor<TEnum>.IntValues[valueIndex];
                if (testValue == 0)
                    continue;

                if (testValue > remainder)
                    yield break;

                if ((remainder & testValue) == 0)
                    continue;

                remainder -= testValue;

				yield return MetadataFor<TEnum>.Values[valueIndex]; // This is the corresponding value.
            }

            if (remainder == 0)
                yield break;

            // Return any non-zero, non-member, remainder.
			yield return MetadataFor<TEnum>.Delegates.ToEnum(remainder);
        }

        /// <summary>
        ///		Check if a <typeparamref name="TEnum"/> value is composed of more than one <typeparamref name="TEnum"/> member.
        /// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
        /// <param name="value">
        ///		The value to examine.
        /// </param>
        /// <returns>
        ///		<c>true</c>, if <paramref name="value"/> is composed of more than one <typeparamref name="TEnum"/> member; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsComposite<TEnum>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
        {
	        using (IEnumerator<TEnum> compositeValueEnumerator = Decompose(value).GetEnumerator())
	        {
				return
					compositeValueEnumerator.MoveNext() // First value
					&&
					compositeValueEnumerator.MoveNext(); // Ok, must be composite.
	        }
		}

		/// <summary>
		///		Perform a bitwise AND of a <typeparamref name="TEnum"/> value and an <see cref="Int32"/> value.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="value">
		///		The <typeparamref name="TEnum"/> value.
		/// </param>
		/// <param name="andWith">
		///		The <see cref="Int32"/> to combine the <typeparamref name="TEnum"/> value with.
		/// </param>
		/// <returns>
		///		The combined value, as a <typeparamref name="TEnum"/>.
		/// </returns>
		public static TEnum BitwiseAnd<TEnum>(TEnum value, int andWith)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!MetadataFor<TEnum>.CanBeComposite)
			{
				throw new NotSupportedException(
					String.Format(
						"Enum type '{0}' does not support composite values.",
						typeof(TEnum).FullName
					)
				);
			}

			return 
				MetadataFor<TEnum>.Delegates
					.BitwiseAndInt32(value, andWith);
		}

		/// <summary>
		///		Perform a bitwise AND of 2 <typeparamref name="TEnum"/> values.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="value">
		///		The first <typeparamref name="TEnum"/> value.
		/// </param>
		/// <param name="andWith">
		///		The second <typeparamref name="TEnum"/> value.
		/// </param>
		/// <returns>
		///		The combined value, as a <typeparamref name="TEnum"/>.
		/// </returns>
		public static TEnum BitwiseAnd<TEnum>(TEnum value, TEnum andWith)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!MetadataFor<TEnum>.CanBeComposite)
			{
				throw new NotSupportedException(
					String.Format(
						"Enum type '{0}' does not support composite values.",
						typeof(TEnum).FullName
					)
				);
			}

			return
				MetadataFor<TEnum>.Delegates
					.BitwiseAnd(value, andWith);
		}

		/// <summary>
		///		Perform a bitwise NOT of a <typeparamref name="TEnum"/> value.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="value">
		///		The <typeparamref name="TEnum"/> value.
		/// </param>
		/// <returns>
		///		The combined value, as a <typeparamref name="TEnum"/>.
		/// </returns>
		public static TEnum BitwiseNot<TEnum>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!MetadataFor<TEnum>.CanBeComposite)
			{
				throw new NotSupportedException(
					String.Format(
						"Enum type '{0}' does not support composite values.",
						typeof(TEnum).FullName
					)
				);
			}

			return
				MetadataFor<TEnum>.Delegates
					.BitwiseNot(value);
		}

		/// <summary>
		///		Perform a bitwise OR of a <typeparamref name="TEnum"/> value and an <see cref="Int32"/> value.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="value">
		///		The <typeparamref name="TEnum"/> value.
		/// </param>
		/// <param name="orWith">
		///		The <see cref="Int32"/> to combine the <typeparamref name="TEnum"/> value with.
		/// </param>
		/// <returns>
		///		The combined value, as a <typeparamref name="TEnum"/>.
		/// </returns>
		public static TEnum BitwiseOr<TEnum>(TEnum value, int orWith)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!MetadataFor<TEnum>.CanBeComposite)
			{
				throw new NotSupportedException(
					String.Format(
						"Enum type '{0}' does not support composite values.",
						typeof(TEnum).FullName
					)
				);
			}

			return
				MetadataFor<TEnum>.Delegates
					.BitwiseOrInt32(value, orWith);
		}

		/// <summary>
		///		Perform a bitwise OR of 2 <typeparamref name="TEnum"/> values.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="value">
		///		The first <typeparamref name="TEnum"/> value.
		/// </param>
		/// <param name="orWith">
		///		The second <typeparamref name="TEnum"/> value.
		/// </param>
		/// <returns>
		///		The combined value, as a <typeparamref name="TEnum"/>.
		/// </returns>
		public static TEnum BitwiseOr<TEnum>(TEnum value, TEnum orWith)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!MetadataFor<TEnum>.CanBeComposite)
			{
				throw new NotSupportedException(
					String.Format(
						"Enum type '{0}' does not support composite values.",
						typeof(TEnum).FullName
					)
				);
			}

			return
				MetadataFor<TEnum>.Delegates
					.BitwiseOr(value, orWith);
		}

		/// <summary>
		///		Perform a bitwise XOR of 2 <typeparamref name="TEnum"/> values.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="value">
		///		The first <typeparamref name="TEnum"/> value.
		/// </param>
		/// <param name="xorWith">
		///		The second <typeparamref name="TEnum"/> value.
		/// </param>
		/// <returns>
		///		The combined value, as a <typeparamref name="TEnum"/>.
		/// </returns>
		public static TEnum BitwiseExclusiveOr<TEnum>(TEnum value, TEnum xorWith)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!MetadataFor<TEnum>.CanBeComposite)
			{
				throw new NotSupportedException(
					String.Format(
						"Enum type '{0}' does not support composite values.",
						typeof(TEnum).FullName
					)
				);
			}

			return
				MetadataFor<TEnum>.Delegates
					.BitwiseExclusiveOr(value, xorWith);
		}

		/// <summary>
		///		Perform a bitwise XOR of a <typeparamref name="TEnum"/> value and an <see cref="Int32"/> value.
		/// </summary>
		/// <typeparam name="TEnum">
		///		The <see cref="Enum">enum</see> type.
		/// </typeparam>
		/// <param name="value">
		///		The <typeparamref name="TEnum"/> value.
		/// </param>
		/// <param name="xorWith">
		///		The <see cref="Int32"/> to combine the <typeparamref name="TEnum"/> value with.
		/// </param>
		/// <returns>
		///		The combined value, as a <typeparamref name="TEnum"/>.
		/// </returns>
		public static TEnum BitwiseExclusiveOr<TEnum>(TEnum value, int xorWith)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			if (!MetadataFor<TEnum>.CanBeComposite)
			{
				throw new NotSupportedException(
					String.Format(
						"Enum type '{0}' does not support composite values.",
						typeof(TEnum).FullName
					)
				);
			}

			return
				MetadataFor<TEnum>.Delegates
					.BitwiseExclusiveOrInt32(value, xorWith);
		}

		#region MetadataFor<TEnum>

		/// <summary>
		///		Represents the metadata for the specified <see cref="Enum">enum</see> <typeparamref name="TEnum">type</typeparamref>
		/// </summary>
		/// <typeparam name="TEnum">
		///		The type of <see cref="Enum">enum</see> to which the metadata applies.
		/// </typeparam>
		public static class MetadataFor<TEnum>
			where TEnum : struct, IComparable, IFormattable, IConvertible // The actual enum type restriction is done via reflection in the type initialiser for a given concretisation of this class.
		{
			/// <summary>
			///		All defined values for <typeparamref name="TEnum"/>, in ascending order of absolute numeric value.
			/// </summary>
			internal static readonly TEnum[] Values;

			/// <summary>
			///		All defined numeric values for <typeparamref name="TEnum"/>, in ascending order of absolute value.
			/// </summary>
			internal static readonly int[] IntValues;

			/// <summary>
			///		The names of all defined values for <typeparamref name="TEnum"/>, in ascending order of absolute numeric value.
			/// </summary>
			internal static readonly string[] ValueNames;

			/// <summary>
			///		<see cref="FieldInfo">member information</see> for each member of the enumeration.
			/// </summary>
			internal static readonly Dictionary<TEnum, FieldInfo> MemberInfo = new Dictionary<TEnum, FieldInfo>();

			/// <summary>
			///		All non-composite defined values for <typeparamref name="TEnum"/>, in ascending order of absolute numeric value.
			/// </summary>
			internal static readonly TEnum[] NonCompositeValues;

			/// <summary>
			///		Can a <typeparamref name="TEnum"/> value be composed of multiple <typeparamref name="TEnum"/> values (ie. is <typeparamref name="TEnum"/> marked with <see cref="FlagsAttribute"/>)?
			/// </summary>
			public static readonly bool CanBeComposite;

			/// <summary>
			///		The <typeparamref name="TEnum"/> value equivalent to 0.
			/// </summary>
			public static readonly TEnum Zero;

			/// <summary>
			///		Type initialiser.
			/// </summary>
			/// <remarks>
			///		Runs once for each concretisation of <see cref="EnumHelper.MetadataFor{TEnum}"/> for a specific <typeparamref name="TEnum">enum</typeparamref> type.
			/// </remarks>
			static MetadataFor()
			{
				Type enumType = typeof(TEnum);
				if (!enumType.IsEnum)
					throw new NotSupportedException(String.Format("'{0}' is not an enum.", enumType.FullName));

				if (Enum.GetUnderlyingType(enumType) != typeof(int))
					throw new NotSupportedException(String.Format("The numeric type underlying'{0}' is not a 32-bit integer.", enumType.FullName));

				CanBeComposite = enumType.GetCustomAttribute<FlagsAttribute>() != null;

				// AF: A useful fact to know is that all of these arrays are supplied to us pre-sorted in ascending order of absolute value.
				// AF: This makes decompsition a trivial exercise.

				Values =
					Enum
						.GetValues(enumType)
						.Cast<TEnum>()
						.ToArray();

				IntValues =
					Enum
						.GetValues(enumType)
						.Cast<int>()
						.ToArray();

				if (IntValues.Any(intValue => intValue < 0))
				{
					throw new NotSupportedException(
						String.Format(
							"Enum type '{0}' has one or more negative values; this is not currently supported.",
							typeof(TEnum).FullName
							)
						);
				}

				ValueNames = Enum.GetNames(enumType);

				// Catch duplicate values (not currently supported).
				var countsByValue =
					IntValues
						.GroupBy(
							intValue =>
								intValue
						)
						.Select(
							grouped =>
								new
								{
									Value = grouped.Key,
									Count = grouped.Count()
								}
						)
						.Where(
							countForValue =>
								countForValue.Count > 1
						);
				var duplicateValue =
					countsByValue
						.FirstOrDefault();
				if (duplicateValue != null)
				{
					int[] duplicateValueIndices =
						Array.FindAll(
							IntValues,
							intValue =>
								intValue == duplicateValue.Value
							);
					string duplicateNames =
						String.Join(
							", ",
							duplicateValueIndices
								.Select(
									duplicateValueIndex =>
										ValueNames[duplicateValueIndex]
								)
							);

					throw new NotSupportedException(
						String.Format(
							"Enum type '{0}' has multiple members ({1}) with the same numeric value ({2}). This is not currently supported by the enum helper.",
							enumType,
							duplicateNames,
							duplicateValue.Value
							)
						);
				}

				for (int memberIndex = 0; memberIndex < Values.Length; memberIndex++)
				{
					TEnum memberValue = Values[memberIndex];
					string memberName = ValueNames[memberIndex];

					FieldInfo memberInfo = enumType.GetField(
						memberName,
						BindingFlags.Public | BindingFlags.Static
						);
					Debug.Assert(memberInfo != null, "Cannot retrieve enum field member by name."); // AF: Should never happen.

					MemberInfo.Add(memberValue, memberInfo);
				}

				Zero = Delegates.ToEnum(0);

				if (CanBeComposite)
				{
					// AF: You could also use a BitVector32.
					NonCompositeValues =
						Values
							.Where(
								value =>
									!IsComposite(value)
							)
							.ToArray();
				}
				else
					NonCompositeValues = Values;
			}

			/// <summary>
			///		The members of <typeparamref name="TEnum"/>.
			/// </summary>
			public static IReadOnlyList<TEnum> Members
			{
				get
				{
					return Values;
				}
			}

			/// <summary>
			///		The non-composite members of <typeparamref name="TEnum"/>.
			/// </summary>
			public static IReadOnlyList<TEnum> NonCompositeMembers
			{
				get
				{
					return NonCompositeValues;
				}
			}

			/// <summary>
			///		The names of <typeparamref name="TEnum"/> members.
			/// </summary>
			public static IReadOnlyList<string> MemberNames
			{
				get
				{
					return ValueNames;
				}
			}

			/// <summary>
			///		The members of <typeparamref name="TEnum"/>, as <see cref="Int32"/>s.
			/// </summary>
			public static IReadOnlyList<int> IntMembers
			{
				get
				{
					return IntValues;
				}
			}

			/// <summary>
			///		Get the custom attribute of the specified type from the <typeparamref name="TEnum"/> member represented by the supplied <typeparamref name="TEnum"/> value.
			/// </summary>
			/// <typeparam name="TCustomAttribute">
			///		The type of custom attribute to retrieve.
			/// </typeparam>
			/// <param name="value">
			///		The <typeparamref name="TEnum"/> value representing the member.
			/// </param>
			/// <returns>
			///		The custom attribute, or <c>null</c> if the member does not have the attribute applied to it.
			/// </returns>
			public static TCustomAttribute GetMemberCustomAttribute<TCustomAttribute>(TEnum value)
				where TCustomAttribute : Attribute
			{
				if (CanBeComposite && IsComposite(value))
					throw new ArgumentException("Composite values are not supported for this operation.", nameof(value));

				FieldInfo memberInfo;
				if (!MemberInfo.TryGetValue(value, out memberInfo))
					return null;

				return memberInfo.GetCustomAttribute<TCustomAttribute>();
			}

// ReSharper disable MemberHidesStaticFromOuterClass

			#region Delegates

			/// <summary>
			///		Delegates for with enum types and integers.
			/// </summary>
			internal static class Delegates
			{
				/// <summary>
				///		A delegate that converts a TEnum value to an <see cref="Int32"/> value.
				/// </summary>
				public static readonly Func<TEnum, int> ToInt32 = CreateConverter<TEnum, int>();

				/// <summary>
				///		A delegate that converts an <see cref="Int32"/> value to a TEnum value.
				/// </summary>
				public static readonly Func<int, TEnum> ToEnum = CreateConverter<int, TEnum>();

				/// <summary>
				///		A delegate that performs a bitwise AND on its parameters.
				/// </summary>
				public static readonly Func<TEnum, TEnum, TEnum> BitwiseAnd = CreateBinaryOperation(Expression.And);

				/// <summary>
				///		A delegate that performs a bitwise AND on its parameters.
				/// </summary>
				public static readonly Func<TEnum, int, TEnum> BitwiseAndInt32 = CreateBinaryOperationWithInt32(Expression.And);

				/// <summary>
				///		A delegate that performs a bitwise OR on its parameters.
				/// </summary>
				public static readonly Func<TEnum, TEnum, TEnum> BitwiseOr = CreateBinaryOperation(Expression.Or);

				/// <summary>
				///		A delegate that performs a bitwise OR on its parameters.
				/// </summary>
				public static readonly Func<TEnum, int, TEnum> BitwiseOrInt32 = CreateBinaryOperationWithInt32(Expression.Or);

				/// <summary>
				///		A delegate that performs a bitwise XOR on its parameters.
				/// </summary>
				public static readonly Func<TEnum, TEnum, TEnum> BitwiseExclusiveOr = CreateBinaryOperation(Expression.ExclusiveOr);

				/// <summary>
				///		A delegate that performs a bitwise XOR on its parameters.
				/// </summary>
				public static readonly Func<TEnum, int, TEnum> BitwiseExclusiveOrInt32 = CreateBinaryOperationWithInt32(Expression.ExclusiveOr);

				/// <summary>
				///		A delegate that performs a bitwise NOT on its parameter.
				/// </summary>
				public static readonly Func<TEnum, TEnum> BitwiseNot = CreateUnaryOperation(Expression.Not);

				/// <summary>
				///		Create a converter delegate for casting from a source type to a destination type.
				/// </summary>
				/// <typeparam name="TSource">
				///		The source type.
				/// </typeparam>
				/// <typeparam name="TDestination">
				///		The destination type.
				/// </typeparam>
				/// <returns>
				///		A <see cref="Func{TSource,TDestination}"/> that takes a <typeparamref name="TSource"/> value and returns a <typeparamref name="TDestination"/> value.
				/// </returns>
				static Func<TSource, TDestination> CreateConverter<TSource, TDestination>()
				{
					ParameterExpression sourceParameter = Expression.Parameter(
						typeof(TSource),
						"source"
						);

					return
						Expression.Lambda<Func<TSource, TDestination>>(
							Expression.Convert(
								sourceParameter,
								typeof(TDestination)
								),
							sourceParameter
							)
							.Compile();
				}

				/// <summary>
				///		Create a delegate that performs a unary operation on its argument.
				/// </summary>
				/// <param name="unaryOperationSelector">
				///		A <see cref="Func{Expression,UnaryExpression}"/> that takes an <see cref="Int32"/> <see cref="Expression"/>, and returns a <see cref="UnaryExpression"/> that operates on it.
				/// </param>
				/// <returns>
				///		A <see cref="Func{TEnum,TEnum}"/> that takes 2 TEnum values, and returns the result of performing the unary operation on them.
				/// </returns>
				static Func<TEnum, TEnum> CreateUnaryOperation(Func<Expression, UnaryExpression> unaryOperationSelector)
				{
					if (unaryOperationSelector == null)
						throw new ArgumentNullException(nameof(unaryOperationSelector));

					ParameterExpression valueParameter = Expression.Parameter(
						typeof(TEnum),
						"value"
						);

					UnaryExpression unaryOperation = unaryOperationSelector(
						Expression.Convert(
							valueParameter,
							typeof(int)
							)
						);

					if (unaryOperation == null)
						throw new InvalidOperationException("Unary operation selector cannot return null.");

					return
						Expression.Lambda<Func<TEnum, TEnum>>(
							Expression.Convert(
								unaryOperation,
								typeof(TEnum)
								),
							valueParameter
							)
							.Compile();
				}

				/// <summary>
				///		Create a delegate that performs a binary operation on its arguments.
				/// </summary>
				/// <param name="binaryOperationSelector">
				///		A <see cref="Func{Expression,Expression,BinaryExpression}"/> that takes 2 <see cref="Int32"/> <see cref="ParameterExpression"/>s, and returns a <see cref="BinaryExpression"/> that operates on both of them.
				/// </param>
				/// <returns>
				///		A <see cref="Func{TEnum,TEnum,TEnum}"/> that takes 2 TEnum values, and returns the result of performing the binary operation on them.
				/// </returns>
				static Func<TEnum, TEnum, TEnum> CreateBinaryOperation(Func<Expression, Expression, BinaryExpression> binaryOperationSelector)
				{
					if (binaryOperationSelector == null)
						throw new ArgumentNullException(nameof(binaryOperationSelector));

					ParameterExpression valueParameter = Expression.Parameter(
						typeof(TEnum),
						"value"
					);

					ParameterExpression otherValueParameter = Expression.Parameter(
						typeof(TEnum),
						"otherValueParameter"
					);

					BinaryExpression binaryOperation = binaryOperationSelector(
						Expression.Convert(
							valueParameter,
							typeof(int)
						),
						Expression.Convert(
							otherValueParameter,
							typeof(int)
						)
					);

					if (binaryOperation == null)
						throw new InvalidOperationException("Binary operation selector cannot return null.");

					return
						Expression.Lambda<Func<TEnum, TEnum, TEnum>>(
							Expression.Convert(
								binaryOperation,
								typeof(TEnum)
							),
							valueParameter,
							otherValueParameter
						)
						.Compile();
				}

				/// <summary>
				///		Create a delegate that performs a binary operation on its arguments.
				/// </summary>
				/// <param name="binaryOperationSelector">
				///		A <see cref="Func{Expression,Expression,BinaryExpression}"/> that takes 2 <see cref="Int32"/> <see cref="ParameterExpression"/>s, and returns a <see cref="BinaryExpression"/> that operates on both of them.
				/// </param>
				/// <returns>
				///		A <see cref="Func{TEnum,TEnum,TEnum}"/> that takes 1 TEnum value and 1 <see cref="Int32"/> value, and returns the result of performing the binary operation on them.
				/// </returns>
				static Func<TEnum, int, TEnum> CreateBinaryOperationWithInt32(Func<Expression, Expression, BinaryExpression> binaryOperationSelector)
				{
					if (binaryOperationSelector == null)
						throw new ArgumentNullException(nameof(binaryOperationSelector));

					ParameterExpression valueParameter = Expression.Parameter(
						typeof(TEnum),
						"value"
					);

					ParameterExpression otherValueParameter = Expression.Parameter(
						typeof(int),
						"otherValueParameter"
					);

					BinaryExpression binaryOperation = binaryOperationSelector(
						Expression.Convert(
							valueParameter,
							typeof(int)
						),
						otherValueParameter
					);

					if (binaryOperation == null)
						throw new InvalidOperationException("Binary operation selector cannot return null.");

					return
						Expression.Lambda<Func<TEnum, int, TEnum>>(
							Expression.Convert(
								binaryOperation,
								typeof(TEnum)
							),
							valueParameter,
							otherValueParameter
						)
						.Compile();
				}
			}

			#endregion // Delegates

// ReSharper restore MemberHidesStaticFromOuterClass
		}

		#endregion // MetadataFor<TEnum>
	}
}