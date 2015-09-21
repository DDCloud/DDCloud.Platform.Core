using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark an element as being used implicitly (e.g. via reflection, or in an external library).
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class UsedImplicitlyAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified element as being used implicitly (e.g. via reflection, or in an external library).
		/// </summary>
		[UsedImplicitly]
		public UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		/// <summary>
		///		Mark the specified element as being used implicitly (e.g. via reflection, or in an external library).
		/// </summary>
		/// <param name="useKindFlags">
		///		The kind of implicit use.
		/// </param>
		/// <param name="targetFlags">
		///		The target of implicit use.
		/// </param>
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			UseKindFlags = useKindFlags;
			TargetFlags = targetFlags;
		}

		/// <summary>
		///		Mark the specified element as being used implicitly (e.g. via reflection, or in an external library).
		/// </summary>
		/// <param name="useKindFlags">
		///		The kind of implicit use.
		/// </param>
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		/// <summary>
		///		Mark the specified element as being used implicitly (e.g. via reflection, or in an external library).
		/// </summary>
		/// <param name="targetFlags">
		///		The target of implicit use.
		/// </param>
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		/// <summary>
		///		The kind of implicit use.
		/// </summary>
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags
		{
			get;
			private set;
		}

		/// <summary>
		///    The target of implicit use.
		/// </summary>
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags
		{
			get;
			private set;
		}
	}
}