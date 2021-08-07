using CustomerLibCore.Domain.Exceptions;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Exceptions
{
	public class PagedRequestInvalidExceptionTest
	{
		[Fact]
		public void ShouldCreatePagedRequestInvalidException()
		{
			var page = 5;
			var pageSize = 7;

			var ex = new PagedRequestInvalidException(page, pageSize);

			Assert.Equal(page, ex.Page);
			Assert.Equal(pageSize, ex.PageSize);
		}

		// TODO: test Serialization...
	}
}
