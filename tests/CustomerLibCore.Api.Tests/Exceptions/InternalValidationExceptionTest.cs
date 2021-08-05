using System.Collections.Generic;
using CustomerLibCore.Domain.Exceptions;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Exceptions
{
	public class InternalValidationExceptionTest
	{
		[Fact]
		public void ShouldCreateEntityValidationException()
		{
			var propertyName1 = "name1";
			var propertyName2 = "name2";

			var errorMessage1 = "msg1";
			var errorMessage2 = "msg2";

			var errors = new List<ValidationFailure>() { new(propertyName1,errorMessage1),
				new(propertyName2,errorMessage2)};

			var ex = new InternalValidationException(errors);

			Assert.Equal(errors, ex.Errors);
		}

		// TODO: test Serialization...
	}
}
