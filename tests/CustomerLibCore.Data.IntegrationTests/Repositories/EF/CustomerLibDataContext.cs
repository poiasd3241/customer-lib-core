using CustomerLibCore.Data.Repositories.EF;
using CustomerLibCore.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CustomerLibCore.Data.IntegrationTests.Repositories.EF
{
	public class CustomerLibDataContextTest
	{
		[Fact]
		public void ShouldCreateDataContext()
		{
			var options = new StrictMock<DbContextOptions<CustomerLibDataContext>>();

			var context = new CustomerLibDataContext(options.Object);

			Assert.Equal(QueryTrackingBehavior.NoTracking,
				context.ChangeTracker.QueryTrackingBehavior);

			Assert.NotNull(context.Customers);
			Assert.NotNull(context.Addresses);
			Assert.NotNull(context.Notes);
		}
	}
}
