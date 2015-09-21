namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		Options for <see cref="IndentedStringWriter.BeginIndentScope"/>.
	/// </summary>
	public sealed class ScopedIndentOptions
	{
		/// <summary>
		///		Scoped indent options that represent an indent scope with no prefix, no suffix, and no new-lines automatically inserted.
		/// </summary>
		public static readonly ScopedIndentOptions None = new ScopedIndentOptions();

		/// <summary>
		///		Scoped indent options that simply begin a new line after opening the scope, and begin a new line before closing it.
		/// </summary>
		public static readonly ScopedIndentOptions NewLinesBeforeAndAfter = new ScopedIndentOptions(scopePrefixNewLines: NewLines.After, scopePostfixNewLines: NewLines.Before);

		/// <summary>
		///		The text (if any) to write before opening the scope.
		/// </summary>
		readonly string		_scopePrefix;

		/// <summary>
		///		The new-line settings for the scope prefix.
		/// </summary>
		/// <remarks>
		///		If no prefix is defined, a value other than <see cref="NewLines.None"/> will still result in new lines being written.
		/// </remarks>
		readonly NewLines	_scopePrefixNewLines;

		/// <summary>
		///		The suffix text (if any) to write before closing the scope.
		/// </summary>
		readonly string		_scopePostfix;

		/// <summary>
		///		The new-line settings for the scope suffix.
		/// </summary>
		/// <remarks>
		///		If no suffix is defined, a value other than <see cref="NewLines.None"/> will still result in new lines being written.
		/// </remarks>
		readonly NewLines	_scopePostfixNewLines;

		/// <summary>
		///		Create new scoped indent options.
		/// </summary>
		/// <param name="scopePrefix">
		///		The text (if any) to write before opening the scope.
		/// </param>
		/// <param name="scopePrefixNewLines">
		///		The new-line settings for the scope prefix.
		/// </param>
		/// <param name="scopePostfix">
		///		The text (if any) to write before closing the scope.
		/// </param>
		/// <param name="scopePostfixNewLines">
		///		The new-line settings for the scope suffix.
		/// </param>
		ScopedIndentOptions(string scopePrefix = null, NewLines scopePrefixNewLines = NewLines.None, string scopePostfix = null, NewLines scopePostfixNewLines = NewLines.None)
		{
			_scopePrefix = scopePrefix;
			_scopePrefixNewLines = scopePrefixNewLines;
			_scopePostfix = scopePostfix;
			_scopePostfixNewLines = scopePostfixNewLines;
		}

		/// <summary>
		///		The text (if any) to write before opening the scope.
		/// </summary>
		public string ScopePrefix
		{
			get
			{
				return _scopePrefix;
			}
		}

		/// <summary>
		///		The new-line settings for the scope prefix.
		/// </summary>
		/// <remarks>
		///		If no prefix is defined, a value other than <see cref="NewLines.None"/> will still result in new lines being written.
		/// </remarks>
		public NewLines ScopePrefixNewLines
		{
			get
			{
				return _scopePrefixNewLines;
			}
		}

		/// <summary>
		///		The suffix text (if any) to write before closing the scope.
		/// </summary>
		public string ScopePostfix
		{
			get
			{
				return _scopePostfix;
			}
		}

		/// <summary>
		///		The new-line settings for the scope prefix.
		/// </summary>
		/// <remarks>
		///		If no prefix is defined, a value other than <see cref="NewLines.None"/> will still result in new lines being written.
		/// </remarks>
		public NewLines ScopePostfixNewLines
		{
			get
			{
				return _scopePostfixNewLines;
			}
		}

		/// <summary>
		///		Create <see cref="ScopedIndentOptions"/> that represent an indented block delimited with the specified prefix and suffix (with a new line before each one).
		/// </summary>
		/// <param name="prefix">
		///		The block prefix (written before the block is opened).
		/// </param>
		/// <param name="suffix">
		///		The block suffix (written after the block is closed).
		/// </param>
		/// <param name="prefixNewLines">
		///		Optional new-line settings for the scope prefix.
		/// 
		///		Defaults to <see cref="NewLines.Before"/>.
		/// </param>
		/// <param name="suffixNewlines">
		///		Optional new-line settings for the scope suffix.
		/// 
		///		Defaults to <see cref="NewLines.Before"/>.
		/// </param>
		/// <returns>
		///		The scoped indent options.
		/// </returns>
		public static ScopedIndentOptions DelimitedBlock(string prefix, string suffix, NewLines prefixNewLines = NewLines.Before, NewLines suffixNewlines = NewLines.Before)
		{
			return new ScopedIndentOptions(
				prefix,
				prefixNewLines,
				suffix,
				suffixNewlines
			);
		}
	}
}
