using CustomerLibCore.Domain.Exceptions;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Exceptions
{
	public class EmailTakenExceptionTest
	{
		[Fact]
		public void ShouldCreateEmailTakenException()
		{
			var ex = new EmailTakenException();

			Assert.NotNull(ex);
		}

		// TODO: test Serialization...
	}
}
