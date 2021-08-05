using CustomerLibCore.Data.IntegrationTests.Repositories.EF;
using CustomerLibCore.Data.Repositories.EF;
using Xunit;

namespace CustomerLibCore.Data.IntegrationTests
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class DatabaseHelperTest
	{
		[Fact]
		public void ShouldUnsafeRepopulateAddressTypes()
		{
			// Free up FK.
			var addressRepository = new AddressRepository(DbContextHelper.Context);
			addressRepository.DeleteAll();

			DatabaseHelper.UnsafeRepopulateAddressTypes();
		}

		[Fact]
		public void ShouldClear()
		{
			DatabaseHelper.Clear();
		}
	}
}
