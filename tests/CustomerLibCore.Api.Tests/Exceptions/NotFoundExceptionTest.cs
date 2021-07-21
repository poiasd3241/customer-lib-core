using CustomerLibCore.Business.Exceptions;
using Xunit;

namespace CustomerLibCore.Business.Tests.Exceptions
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
