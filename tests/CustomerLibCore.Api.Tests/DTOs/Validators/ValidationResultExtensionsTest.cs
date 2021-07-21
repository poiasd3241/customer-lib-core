using System.Collections.Generic;
using CustomerLibCore.Api.DTOs.Validators;
using CustomerLibCore.Api.Exceptions;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Api.Tests.DTOs.Validators
{
	public class ValidationResultExtensionsTest
	{
		[Fact]
		public void ShouldThrowInvalidBodyExceptionOnInvalidResult()
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
			var exception = Assert.Throws<InvalidBodyException>(() =>
				invalidResult.WithInvalidBodyException());

			// Then
			Assert.Equal(errors, exception.Errors);
		}

		[Fact]
		public void ShouldDoNothingOnValidResult()
		{
			// Given
			var emptyErrors = new List<ValidationFailure>();

			var validResult = new ValidationResult(emptyErrors);

			// When
			validResult.WithInvalidBodyException();

			// Then
			Assert.True(validResult.IsValid);
		}
	}
}
