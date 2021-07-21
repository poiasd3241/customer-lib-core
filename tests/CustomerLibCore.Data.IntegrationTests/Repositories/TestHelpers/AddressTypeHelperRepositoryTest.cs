using CustomerLibCore.Data.IntegrationTests.Repositories.EF;
using CustomerLibCore.Data.Repositories.EF;
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
			var addressRepository = new AddressRepository(DbContextHelper.Context);
			addressRepository.DeleteAll();

			AddressTypeHelperRepository.UnsafeRepopulateAddressTypes();
		}
	}
}
