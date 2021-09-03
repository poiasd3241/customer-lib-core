using System.Collections.Generic;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.Domain.FluentValidation;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Domain.Tests.FluentValidation
{
	public class ValidationResultExtensionsTest
	{
		[Fact]
		public void ShouldThrowInternalValidationExceptionOnInvalidResult()
		{
			// Given
			var propertyName1 = "name1";
			var propertyName2 = "name2";

			var errorMessage1 = "msg1";
			var errorMessage2 = "msg2";

			var errors = new List<ValidationFailure>() { new(propertyName1,errorMessage1),
				new(propertyName2,errorMessage2)};

			var invalidResult = new ValidationResult(errors);

			// When
			var ex = Assert.Throws<InternalValidationException>(() =>
				invalidResult.WithInternalValidationException());

			// Then
			Assert.Equal(errors, ex.Errors);
		}

		[Fact]
		public void ShouldDoNothingOnValidResult()
		{
			// Given
			var emptyErrors = new List<ValidationFailure>();

			var validResult = new ValidationResult(emptyErrors);

			// When
			validResult.WithInternalValidationException();

			// Then
			Assert.True(validResult.IsValid);
		}
	}
}
