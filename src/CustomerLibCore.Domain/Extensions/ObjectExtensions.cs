using System;

namespace CustomerLibCore.Domain.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// If the <paramref name="obj"/> is <see langword="null"/>, 
		/// throws the <see cref="ArgumentNullException"/>; otherwise, does nothing.
		/// </summary>
		/// <param name="obj">The object to check.</param>
		/// <param name="paramName">The name of the <paramref name="obj"/> to include 
		/// in the exception details.</param>
		public static void PreventNull(this object obj, string paramName = null)
		{
			if (obj is null)
			{
				if (paramName is null)
				{
					throw new ArgumentNullException();
				}

				throw new ArgumentNullException(paramName);
			}
		}
	}
}
