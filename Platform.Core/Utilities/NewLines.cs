using System;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		Options controlling how new lines are placed when writing to an indented scope.
	/// </summary>
	[Flags]
	public enum NewLines
	{
		/// <summary>
		///		No options.
		/// </summary>
		None = 0,

		/// <summary>
		///		Begin a new line before writing.
		/// </summary>
		Before		= 1,

		/// <summary>
		///		Begin a new line after writing.
		/// </summary>
		After		= 2,

		/// <summary>
		///		Begin a new line, write, and then begin another new line.
		/// </summary>
		Both		= Before | After
	}
}
