using System;
using System.Linq.Expressions;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		Helper methods for Reflection.
	/// </summary>
	public static class ReflectionHelper
	{
		/// <summary>
		///		Get the name of a type member, in string.
		/// </summary>
		/// <typeparam name="T">
		///		The containing type.
		/// </typeparam>
		/// <param name="expression">
		///		The expression identifying the member to get the name from.
		/// </param>
		/// <returns>
		///		The member name.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		<paramref name="expression"/> does not represent a member-access expression.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "To allow the use of lambda syntax by the caller.")]
		public static string GetMemberName<T>(Expression<Func<T>> expression)
		{
			if (expression == null)
				throw new ArgumentNullException(nameof(expression));

			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression != null)
				return memberExpression.Member.Name;

			throw new InvalidOperationException("The supplied expression is not a member-access expression.");
		}
	}
}
