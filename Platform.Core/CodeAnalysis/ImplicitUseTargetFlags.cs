using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Flags describing the target of implicit use.
	/// </summary>
	/// <remarks>
	///		Used by <see cref="MeansImplicitUseAttribute" /> or <see cref="UsedImplicitlyAttribute" />.
	/// </remarks>
	[Flags]
	public enum ImplicitUseTargetFlags
	{
		/// <summary>
		///		The target itself is considered used.
		/// </summary>
		Itself = 1,

		/// <summary>
		///     Members of the target are considered used.
		/// </summary>
		Members = 2,

		/// <summary>
		///     The target and its members are considered used.
		/// </summary>
		WithMembers = Itself | Members,

		/// <summary>
		///		The default implicit use target flags.
		/// </summary>
		Default = Itself,
	}
}