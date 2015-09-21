using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	/// <summary>
	///     Mark an element as permitting <c>null</c> values (so a check for <c>null</c> is necessary before using it).
	/// </summary>
	/// <example>
	///     <code>
	///			[CanBeNull]
	///			public object Test()
	///			{
	///			  return null;
	///			}
	/// 
	///			public void UseTest()
	///			{
	///				object val = Test(); 
	///				string str = val.ToString(); // Warning: Possible 'System.NullReferenceException' 
	///			}
	///		</code>
	/// </example>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class CanBeNullAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified element as permitting <c>null</c> values (so a check for <c>null</c> is necessary before using it).
		/// </summary>
		public CanBeNullAttribute()
		{
		}
	}
}