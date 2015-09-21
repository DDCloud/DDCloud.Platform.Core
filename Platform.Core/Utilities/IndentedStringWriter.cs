using System;
using System.Text;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		Writes to a <see cref="StringBuilder"/>, with indent support.
	/// </summary>
	public class IndentedStringWriter
	{
		/// <summary>
		///		The <see cref="StringBuilder"/> to which input strings are written.
		/// </summary>
		readonly StringBuilder	_buffer;

		/// <summary>
		///		The indent string that is prefixed to each line.
		/// </summary>
		readonly string			_indentPrefix;

		/// <summary>
		///		The current indent level.
		/// </summary>
		int						_currentIndentLevel;

		/// <summary>
		///		Is the writer at the start of a line?
		/// </summary>
		bool					_atStartOfLine = true;

		/// <summary>
		///		Create a new indented string writer.
		/// </summary>
		/// <param name="indentCharacter">
		///		The indent character.
		/// </param>
		/// <param name="indent">
		///		The number of indent characters to prefix for each indent level.
		/// </param>
		public IndentedStringWriter(char indentCharacter = '\t', int indent = 1)
			: this(new StringBuilder(), indentCharacter, indent)
		{
		}

		/// <summary>
		///		Create a new indented string writer that writes to the specified <see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="buffer">
		///		The <see cref="StringBuilder"/> buffer to which input lines are written.
		/// </param>
		/// <param name="indentCharacter">
		///		The indent character.
		/// </param>
		/// <param name="indent">
		///		The number of indent characters to prefix for each indent level.
		/// </param>
		public IndentedStringWriter(StringBuilder buffer, char indentCharacter = '\t', int indent = 1)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));

			if (indent < 0)
				throw new ArgumentOutOfRangeException(nameof(indent));

			_buffer = buffer;
			_indentPrefix = new String(indentCharacter, indent);
		}

		/// <summary>
		///		The current indent level.
		/// </summary>
		public int CurrentIndentLevel
		{
			get
			{
				return _currentIndentLevel;
			}
		}

		/// <summary>
		///		Is the writer at the start of a line?
		/// </summary>
		public bool AtStartOfLine
		{
			get
			{
				return _atStartOfLine;
			}
		}

		/// <summary>
		///		Ensure that the writer is at the start of a new line.
		/// </summary>
		/// <returns>
		///		<c>true</c>, if the writer was not previous at the start of a line, and has therefore started a new one; <c>false</c>, if the writer was already at the start of a line.
		/// </returns>
		public bool EnsureStartOfLine()
		{
			if (_atStartOfLine)
				return false;

			WriteLine();

			return true;
		}

		/// <summary>
		///		Increment the current indent level.
		/// </summary>
		public void Indent()
		{
			_currentIndentLevel++;
		}

		/// <summary>
		///		Decrement the current indent level.
		/// </summary>
		public void Unindent()
		{
			_currentIndentLevel--;
			if (_currentIndentLevel < 0)
				_currentIndentLevel = 0;
		}

		/// <summary>
		///		Reset the current indent level to 0.
		/// </summary>
		public void ResetIndent()
		{
			_currentIndentLevel = 0;
		}

		/// <summary>
		///		Append the specified string to the underlying <see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="str">
		///		The string to append.
		/// </param>
		public void Write(string str)
		{
			if (str == null)
				throw new ArgumentNullException(nameof(str));

			if (_atStartOfLine)
				AppendIndent();

			_buffer.Append(str);
		}

		/// <summary>
		///		Format the specified string, and append it to the underlying <see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="format">
		///		The format string to append.
		/// </param>
		/// <param name="formatArguments">
		///		Arguments to be inserted into the format string.
		/// </param>
		public void Write(string format, params object[] formatArguments)
		{
			if (format == null)
				throw new ArgumentNullException(nameof(format));

			if (_atStartOfLine)
				AppendIndent();

			_buffer.AppendFormat(format, formatArguments);
		}

		/// <summary>
		///		Append the specified string to the underlying <see cref="StringBuilder"/>, followed by (a) newline character(s) appropriate for the current environment.
		/// </summary>
		/// <param name="str">
		///		The string to append.
		/// </param>
		public void WriteLine(string str)
		{
			if (str == null)
				throw new ArgumentNullException(nameof(str));

			if (_atStartOfLine)
				AppendIndent();

			_buffer.AppendLine(str);
			_atStartOfLine = true;
		}

		/// <summary>
		///		Format the specified string, and append it to the underlying <see cref="StringBuilder"/>, followed by (a) newline character(s) appropriate for the current environment.
		/// </summary>
		/// <param name="format">
		///		The format string to append.
		/// </param>
		/// <param name="formatArguments">
		///		Arguments to be inserted into the format string.
		/// </param>
		public void WriteLine(string format, params object[] formatArguments)
		{
			if (format == null)
				throw new ArgumentNullException(nameof(format));

			if (_atStartOfLine)
				AppendIndent();

			_buffer.AppendFormat(format, formatArguments);
			_buffer.AppendLine();
			_atStartOfLine = true;
		}

		/// <summary>
		///		Append a new line to the underlying <see cref="StringBuilder"/>.
		/// </summary>
		public void WriteLine()
		{
			_buffer.AppendLine();
			_atStartOfLine = true;
		}

		/// <summary>
		///		Begin an indent scope.
		/// </summary>
		/// <param name="options">
		///		Options for the scoped indent.
		/// </param>
		/// <returns>
		///		An <see cref="IDisposable"/> implementation which, when disposed, closes the indent scope.
		/// </returns>
		/// <remarks>
		///		Indent scopes are a simple way of making code that produces indented text easier to read.
		///		
		///		Use of C#'s <c>using</c> statement has a negligible performance impact if no exceptions are thrown, so feel free to use it wherever it makes sense.
		/// </remarks>
		public IDisposable BeginIndentScope(ScopedIndentOptions options = null)
		{
			return new IndentScope(this, options ?? ScopedIndentOptions.None);
		}

		/// <summary>
		///		Convert the contents of the underlying <see cref="StringBuilder"/> to a <see cref="String"/>.
		/// </summary>
		/// <returns>
		///		The contents of the underlying <see cref="StringBuilder"/>, as a <see cref="String"/>.
		/// </returns>
		public override string ToString()
		{
			return _buffer.ToString();
		}

		/// <summary>
		///		Write the indent prefix string for the current indent level to the underlying <see cref="StringBuilder"/>.
		/// </summary>
		void AppendIndent()
		{
			for (int indent = 0; indent < _currentIndentLevel; indent++)
				_buffer.Append(_indentPrefix);

			_atStartOfLine = false;
		}

		#region IndentScope

		/// <summary>
		///		A helper for indent scopes which, when disposed, closes the indent scope.
		/// </summary>
		struct IndentScope
			: IDisposable
		{
			/// <summary>
			///		Options for the scoped indent.
			/// </summary>
			readonly ScopedIndentOptions	_options;

			/// <summary>
			///		The indented string writer to which the indent scope applies.
			/// </summary>
			IndentedStringWriter			_writer;

			/// <summary>
			///		Create a new indent scope.
			/// </summary>
			/// <param name="writer">
			///		The indented string writer to which the indent scope applies.
			/// </param>
			/// <param name="options">
			///		Options for the scoped indent.
			/// </param>
			public IndentScope(IndentedStringWriter writer, ScopedIndentOptions options)
				: this()
			{
				if (writer == null)
					throw new ArgumentNullException(nameof(writer));

				if (options == null)
					throw new ArgumentNullException(nameof(options));

				_writer = writer;
				_options = options;

				if (!String.IsNullOrWhiteSpace(_options.ScopePrefix))
				{
					if (_options.ScopePrefixNewLines.HasFlag(NewLines.Before))
						_writer.WriteLine(_options.ScopePrefix);
					else
						_writer.Write(_options.ScopePrefix);
				}
				else if (_options.ScopePrefixNewLines.HasFlag(NewLines.Before))
					_writer.WriteLine();

				_writer.Indent();

				if (_options.ScopePrefixNewLines.HasFlag(NewLines.After))
					_writer.WriteLine();
			}

			/// <summary>
			///		Close the indent scope.
			/// </summary>
			public void Dispose()
			{
				if (_writer == null)
					return;

				if (_options.ScopePostfixNewLines.HasFlag(NewLines.Before))
					_writer.WriteLine();

				_writer.Unindent();

				if (!String.IsNullOrWhiteSpace(_options.ScopePostfix))
				{
					if (_options.ScopePostfixNewLines.HasFlag(NewLines.After))
						_writer.WriteLine(_options.ScopePostfix);
					else
						_writer.Write(_options.ScopePostfix);
				}
				else if (_options.ScopePostfixNewLines.HasFlag(NewLines.After))
					_writer.WriteLine();

				_writer = null;
			}
		}

		#endregion // IndentScope
	}
}
