using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark a code-analysis attribute as implying implicit use of its target element.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class MeansImplicitUseAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified code-analysis attribute as implying implicit use of its target element.
		/// </summary>
		[UsedImplicitly]
		public MeansImplicitUseAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		/// <summary>
		///		Mark the specified code-analysis attribute as implying implicit use of its target element.
		/// </summary>
		/// <param name="useKindFlags">
		///		The kind of use implied by the attribute.
		/// </param>
		/// <param name="targetFlags">
		///		The target of use implied by the attribute.
		/// </param>
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			UseKindFlags = useKindFlags;
			TargetFlags = targetFlags;
		}

		/// <summary>
		///		Mark the specified code-analysis attribute as implying implicit use of its target element.
		/// </summary>
		/// <param name="useKindFlags">
		///		The kind of use implied by the attribute.
		/// </param>
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		/// <summary>
		///		Mark the specified code-analysis attribute as implying implicit use of its target element.
		/// </summary>
		/// <param name="targetFlags">
		///		The target of use implied by the attribute.
		/// </param>
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		/// <summary>
		///		The kind of use implied by the attribute.
		/// </summary>
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags
		{
			get;
			private set;
		}

		/// <summary>
		///     The target of use implied by the attribute.
		/// </summary>
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags
		{
			get;
			private set;
		}
	}
}