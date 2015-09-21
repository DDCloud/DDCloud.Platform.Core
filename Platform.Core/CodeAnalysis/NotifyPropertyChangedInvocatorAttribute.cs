using System;

namespace DDCloud.Platform.Core.CodeAnalysis
{
	using System.ComponentModel;

	/// <summary>
	///     Mark a method as being contained in a type that implements <see cref="INotifyPropertyChanged" />, and that the method is used to raise its <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
	/// </summary>
	/// <remarks>
	///     The method should be non-static and conform to one of the supported signatures:
	///     <list>
	///         <item>
	///             <c>NotifyChanged(string)</c>
	///         </item>
	///         <item>
	///             <c>NotifyChanged(params string[])</c>
	///         </item>
	///         <item>
	///             <c>NotifyChanged{T}(Expression{Func{T}})</c>
	///         </item>
	///         <item>
	///             <c>NotifyChanged{T,U}(Expression{Func{T,U}})</c>
	///         </item>
	///         <item>
	///             <c>SetProperty{T}(ref T, T, string)</c>
	///         </item>
	///     </list>
	/// </remarks>
	/// <example>
	///     <code>
	///			public class Foo : INotifyPropertyChanged
	///			{
	///				public event PropertyChangedEventHandler PropertyChanged;
	/// 
	///				[NotifyPropertyChangedInvocator]
	///				protected virtual void NotifyChanged(string propertyName)
	///				{
	///				}
	/// 
	///				private string _name;
	///				public string Name
	///				{
	///					get
	///					{
	///						return _name; 
	///					}
	///					set
	///					{
	///						_name = value;
	///						NotifyChanged("LastName"); // Warning
	///					}
	///				}
	///			}
	///		</code>
	///     
	///		Examples of generated notifications:
	///     <list>
	///         <item>
	///             <c>NotifyChanged("Property")</c>
	///         </item>
	///         <item>
	///             <c>NotifyChanged(() => Property)</c>
	///         </item>
	///         <item>
	///             <c>NotifyChanged((VM x) => x.Property)</c>
	///         </item>
	///         <item>
	///             <c>SetProperty(ref myField, value, "Property")</c>
	///         </item>
	///     </list>
	/// </example>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class NotifyPropertyChangedInvocatorAttribute
		: Attribute
	{
		/// <summary>
		///		Mark the specified method as being contained in a type that implements <see cref="INotifyPropertyChanged" />, and that the method is used to raise its <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
		/// </summary>
		public NotifyPropertyChangedInvocatorAttribute()
		{
		}

		/// <summary>
		///		Mark the specified method as being contained in a type that implements <see cref="INotifyPropertyChanged" />, and that the method is used to raise its <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
		/// </summary>
		/// <param name="parameterName">
		///		The name of the method parameter that refers to the property name.
		/// </param>
		public NotifyPropertyChangedInvocatorAttribute(string parameterName)
		{
			if (String.IsNullOrWhiteSpace(parameterName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'parameterName'.", nameof(parameterName));

			ParameterName = parameterName;
		}

		/// <summary>
		///		The name of the method parameter that refers to the property name.
		/// </summary>
		[UsedImplicitly]
		public string ParameterName
		{
			get;
			private set;
		}
	}
}