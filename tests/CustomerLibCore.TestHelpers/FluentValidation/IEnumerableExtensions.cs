using System.Collections.Generic;
using System.Linq;

namespace CustomerLibCore.TestHelpers.FluentValidation
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<(string propertyName, string errorMessage)> AppendDetail(
			this IEnumerable<(string propertyName, string errorMessage)> details,
			string parentPropertyName, (string propertyName, string errorMessage) newDetail) =>
				details.Append(
					($"{parentPropertyName}.{newDetail.propertyName}", newDetail.errorMessage));
	}
}
