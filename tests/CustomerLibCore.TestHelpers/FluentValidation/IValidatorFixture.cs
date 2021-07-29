using System.Collections.Generic;

namespace CustomerLibCore.TestHelpers.FluentValidation
{
	public interface IValidatorFixture<T>
	{
		T MockValid();

		T MockInvalid();

		(T invalidObject, IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails();
	}
}
