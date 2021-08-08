using CustomerLibCore.Data.Entities;
using CustomerLibCore.Domain.Enums;
using Xunit;

namespace CustomerLibCore.Data.Tests.Entities
{
	public class AddressEntityTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var address = new AddressEntity();

			Assert.Equal(0, address.AddressId);
			Assert.Equal(0, address.CustomerId);
			Assert.Null(address.Line);
			Assert.Null(address.Line2);
			Assert.Equal(0, (int)address.Type);
			Assert.Null(address.City);
			Assert.Null(address.PostalCode);
			Assert.Null(address.State);
			Assert.Null(address.Country);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			// Given
			var addressId = 1;
			var customerId = 2;
			var line = "line1";
			var line2 = "line21";
			var type = AddressType.Billing;
			var city = "city1";
			var postalCode = "postalCode1";
			var state = "state1";
			var country = "country1";

			var address = new AddressEntity();

			Assert.NotEqual(addressId, address.AddressId);
			Assert.NotEqual(customerId, address.CustomerId);
			Assert.NotEqual(line, address.Line);
			Assert.NotEqual(line2, address.Line2);
			Assert.NotEqual(type, address.Type);
			Assert.NotEqual(city, address.City);
			Assert.NotEqual(postalCode, address.PostalCode);
			Assert.NotEqual(state, address.State);
			Assert.NotEqual(country, address.Country);

			// When
			address.AddressId = addressId;
			address.CustomerId = customerId;
			address.Line = line;
			address.Line2 = line2;
			address.Type = type;
			address.City = city;
			address.PostalCode = postalCode;
			address.State = state;
			address.Country = country;

			// Then
			Assert.Equal(addressId, address.AddressId);
			Assert.Equal(customerId, address.CustomerId);
			Assert.Equal(line, address.Line);
			Assert.Equal(line2, address.Line2);
			Assert.Equal(type, address.Type);
			Assert.Equal(city, address.City);
			Assert.Equal(postalCode, address.PostalCode);
			Assert.Equal(state, address.State);
			Assert.Equal(country, address.Country);
		}

		// TODO: Copy, Equals
	}
}
