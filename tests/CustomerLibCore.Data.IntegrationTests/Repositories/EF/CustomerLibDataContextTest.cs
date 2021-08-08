using CustomerLibCore.Data.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CustomerLibCore.Data.IntegrationTests.Repositories.EF
{
	public class CustomerLibDataContextTest
	{
		[Fact]
		public void ShouldCreateDataContext()
		{
			var context = new CustomerLibDataContext(DbContextHelper.Options);

			Assert.Equal(QueryTrackingBehavior.NoTracking,
				context.ChangeTracker.QueryTrackingBehavior);

			Assert.NotNull(context.Customers);
			Assert.NotNull(context.Addresses);
			Assert.NotNull(context.Notes);
		}
	}
}
