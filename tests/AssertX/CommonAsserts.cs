using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace AssertX
{
	public partial class AssertX
	{
		public static void NotDefault<T>(T obj) where T : class
		{
			if (obj == default)
			{
				throw new AssertXException($"{obj} is not default");
			}
		}

		public static void NotDefault<T>(IEnumerable<T> items) where T : class
		{
			var position = 0;

			foreach (var item in items)
			{
				if (item == default)
				{
					throw new AssertXException($"{obj} is not default");
				}

				position++;
			}
		}
	}
}
