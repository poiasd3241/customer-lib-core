using CustomerLibCore.Api.Dtos.Addresses;
using System.Collections.Generic;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Addresses
{
	public class AddressListResponseTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var addresses = new AddressListResponse();

			Assert.Null(addresses.Self);
			Assert.Null(addresses.Items);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			var self = "self1";
			var items = new List<AddressResponse>();

			var addresses = new AddressListResponse();

			Assert.NotEqual(self, addresses.Self);
			Assert.NotEqual(items, addresses.Items);

			// When
			addresses.Self = self;
			addresses.Items = items;

			Assert.Equal(self, addresses.Self);
			Assert.Equal(items, addresses.Items);
		}
	}
}
