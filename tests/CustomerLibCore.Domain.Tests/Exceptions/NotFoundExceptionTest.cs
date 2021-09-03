using CustomerLibCore.Domain.Exceptions;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Exceptions
{
	public class NotFoundExceptionTest
	{
		[Fact]
		public void ShouldCreateEntityNotFoundException()
		{
			var ex = new NotFoundException();

			Assert.NotNull(ex);
		}

		// TODO: test Serialization...
	}
}
