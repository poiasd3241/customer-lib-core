using CustomerLibCore.Domain.Exceptions;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Exceptions
{
	public class NotFoundExceptionTest
	{
		[Fact]
		public void ShouldCreateEntityNotFoundException()
		{
			var exception = new NotFoundException();

			Assert.NotNull(exception);
		}

		// TODO: test Serialization...
	}
}
