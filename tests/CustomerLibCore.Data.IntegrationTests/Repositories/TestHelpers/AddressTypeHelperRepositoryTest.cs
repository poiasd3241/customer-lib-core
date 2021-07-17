using CustomerLibCore.Data.Repositories.EF;
using CustomerLibCore.Data.IntegrationTests.Repositories.EF;
using Xunit;

namespace CustomerLibCore.Data.IntegrationTests.Repositories.TestHelpers
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class AddressTypeHelperRepositoryTest
	{
		[Fact]
		public void ShouldUnsafeRepopulateAddressTypes()
		{
			// Free up FK.
			var addressRepository = new AddressRepository(
				DbContextOptionsHelper.CustomerLibDbContextOptions);
			addressRepository.DeleteAll();

			AddressTypeHelperRepository.UnsafeRepopulateAddressTypes();
		}
	}
}
