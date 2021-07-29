using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace CustomerLibCore.TestHelpers.FluentValidation
{
	public static class ValidatorExtensions
	{
		public static IEnumerable<ValidationFailure> ValidateProperty<T>(
			this IValidator<T> validator, T instance, string propertyName)
		{
			return validator.Validate(instance, options =>
				options.IncludeProperties(propertyName)).Errors;
		}
	}
}
