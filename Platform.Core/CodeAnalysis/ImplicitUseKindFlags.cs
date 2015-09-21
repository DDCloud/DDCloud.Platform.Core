using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///		Flags describing types of implicit use.
	/// </summary>
	[Flags]
	public enum ImplicitUseKindFlags
	{
		/// <summary>
		///     Only the entity marked with the attribute is considered used.
		/// </summary>
		Access = 1,

		/// <summary>
		///     Indicates implicit assignment to a member.
		/// </summary>
		Assign = 2,

		/// <summary>
		///     Indicates implicit instantiation of a type with a fixed constructor signature.
		/// </summary>
		/// <remarks>
		///		Any unused constructor parameters won't be reported as such.
		/// </remarks>
		InstantiatedWithFixedConstructorSignature = 4,

		/// <summary>
		///     Indicates implicit instantiation of a type.
		/// </summary>
		InstantiatedNoFixedConstructorSignature = 8,

		/// <summary>
		///		The default implicit usage flags.
		/// </summary>
		Default = Access | Assign | InstantiatedWithFixedConstructorSignature
	}
}