using CustomerLibCore.Business.Exceptions;
using Xunit;

namespace CustomerLibCore.Business.Tests.Exceptions
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
