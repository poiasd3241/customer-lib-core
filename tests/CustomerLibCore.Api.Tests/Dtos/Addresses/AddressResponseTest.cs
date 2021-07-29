using CustomerLibCore.Api.Dtos.Addresses;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Addresses
{
	public class AddressResponseTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var address = new AddressResponse();

			Assert.Null(address.Self);

			Assert.Null(address.Line);
			Assert.Null(address.Line2);
			Assert.Null(address.Type);
			Assert.Null(address.City);
			Assert.Null(address.PostalCode);
			Assert.Null(address.State);
			Assert.Null(address.Country);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			// Given
			var self = "self1";

			var line = "line1";
			var line2 = "line21";
			var type = "type1";
			var city = "city1";
			var postalCode = "postalCode1";
			var state = "state1";
			var country = "country1";

			var address = new AddressResponse();

			Assert.NotEqual(self, address.Self);

			Assert.NotEqual(line, address.Line);
			Assert.NotEqual(line2, address.Line2);
			Assert.NotEqual(type, address.Type);
			Assert.NotEqual(city, address.City);
			Assert.NotEqual(postalCode, address.PostalCode);
			Assert.NotEqual(state, address.State);
			Assert.NotEqual(country, address.Country);

			// When
			address.Self = self;

			address.Line = line;
			address.Line2 = line2;
			address.Type = type;
			address.City = city;
			address.PostalCode = postalCode;
			address.State = state;
			address.Country = country;

			// Then
			Assert.Equal(self, address.Self);

			Assert.Equal(line2, address.Line2);
			Assert.Equal(type, address.Type);
			Assert.Equal(city, address.City);
			Assert.Equal(postalCode, address.PostalCode);
			Assert.Equal(state, address.State);
			Assert.Equal(country, address.Country);
		}
	}
}
