using CustomerLibCore.Business.Exceptions;
using Xunit;

namespace CustomerLibCore.Api.Tests.Exceptions
{
	public class ConflictWithExistingExceptionTest
	{
		[Fact]
		public void ShouldCreateConflictWithExistingException()
		{
			var incomingPropertyName = "MyProp";
			var incomingPropertyValue = "veryBad";
			var conflictMessage = "This property already exists";

			var ex = new ConflictWithExistingException(
				incomingPropertyName, incomingPropertyValue, conflictMessage);

			Assert.Equal(incomingPropertyName, ex.IncomingPropertyName);
			Assert.Equal(incomingPropertyValue, ex.IncomingPropertyValue);
			Assert.Equal(conflictMessage, ex.ConflictMessage);
		}

		[Fact]
		public void ShouldCreateConflictWithExistingExceptionEmailTaken()
		{
			var incomingPropertyName = "MyProp";
			var incomingPropertyValue = "veryBad";

			var ex = ConflictWithExistingException.EmailTaken(
				incomingPropertyName, incomingPropertyValue);

			Assert.Equal(incomingPropertyName, ex.IncomingPropertyName);
			Assert.Equal(incomingPropertyValue, ex.IncomingPropertyValue);
			Assert.Equal("Email is already taken", ex.ConflictMessage);
		}

		// TODO: test Serialization...
	}
}
