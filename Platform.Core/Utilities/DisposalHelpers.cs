﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DDCloud.Platform.Core.Utilities
{
	/// <summary>
	///		Helper methods for <see cref="IDisposable"/>.
	/// </summary>
	public static class DisposalHelpers
	{
		/// <summary>
		///		Create an aggregate <see cref="IDisposable"/> that disposes of the specified <see cref="IDisposable"/>s when it is disposed.
		/// </summary>
		/// <param name="disposables">
		///		The <see cref="IDisposable"/>s to aggregate.
		/// </param>
		/// <returns>
		///		An aggregate <see cref="IDisposable"/> representing the supplied disposables.
		/// </returns>
		/// <exception cref="AggregateException">
		///		One or more aggregated disposables throw exceptions during disposal.
		/// </exception>
		public static IDisposable ToAggregateDisposable(this IEnumerable<IDisposable> disposables)
		{
			if (disposables == null)
				throw new ArgumentNullException(nameof(disposables));

			return new AggregateDisposable(disposables);
		}
		
		/// <summary>
		///		Implements disposal of multiple <see cref="IDisposable"/>s.
		/// </summary>
		struct AggregateDisposable
			: IDisposable
		{
			/// <summary>
			///		The disposables to dispose of.
			/// </summary>
			readonly IReadOnlyList<IDisposable> _disposables;

			/// <summary>
			///		Create a new aggregate disposable.
			/// </summary>
			/// <param name="disposables">
			///		A sequence of <see cref="IDisposable"/>s to aggregate.
			/// </param>
			public AggregateDisposable(IEnumerable<IDisposable> disposables)
			{
				if (disposables == null)
					throw new ArgumentNullException(nameof(disposables));

				_disposables = disposables.ToArray();
			}

			/// <summary>
			///		Dispose the disposables.
			/// </summary>
			public void Dispose()
			{
				List<Exception> disposalExceptions = new List<Exception>();
				foreach (IDisposable disposable in _disposables) // AF: What about exception in enumerator?
				{
					try
					{
						disposable.Dispose();
					}
					catch (Exception eDisposal)
					{
						disposalExceptions.Add(eDisposal);
					}
				}

				if (disposalExceptions.Count > 0)
					throw new AggregateException("One or more exceptions were encountered during object disposal.", disposalExceptions);
			}
		}
	}
}
