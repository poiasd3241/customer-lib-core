using Moq;
using Xunit;

namespace CustomerLibCore.TestHelpers
{
	public class StrictMockTest
	{
		private class Whatever { }

		[Fact]
		public void ShouldCreateMockWithStrictBehavior()
		{
			var mock = new StrictMock<Whatever>();

			AssertX.Equal(MockBehavior.Strict, mock.Behavior);
		}
	}
}
