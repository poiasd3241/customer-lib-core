using System;
using CustomerLibCore.Business.Exceptions;
using Xunit;

namespace CustomerLibCore.Business.Tests.Exceptions
{
	public class EntityValidationExceptionTest
	{
		[Fact]
		public void ShouldCreateEntityValidationException()
		{
			var message = "oops";
			var inner = new Exception();

			var defaultConstructor = new EntityValidationException();
			var messageConstructor = new EntityValidationException(message);

			var messageWithInnerConstructor = new EntityValidationException(message, inner);

			Assert.NotNull(defaultConstructor);

			Assert.Equal(message, messageConstructor.Message);

			Assert.Equal(message, messageWithInnerConstructor.Message);
			Assert.Equal(inner, messageWithInnerConstructor.InnerException);
		}

		// TODO: test Serialization...
	}
}
