using CustomerLibCore.Data.IntegrationTests.Repositories.EF;
using CustomerLibCore.Data.Repositories.EF;
using Xunit;

namespace CustomerLibCore.Data.IntegrationTests
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class DatabaseHelperTest
	{
		[Fact]
		public void ShouldClear()
		{
			DatabaseHelper.Clear();

			var customerRepo = new CustomerRepository(DbContextHelper.Context);

			Assert.Equal(0, customerRepo.GetCount());

			// Addresses and Notes cannot exist without customers.
		}
	}
}
