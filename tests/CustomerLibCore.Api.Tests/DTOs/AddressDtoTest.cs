using CustomerLibCore.Api.DTOs;
using Xunit;

namespace CustomerLibCore.Api.Tests.DTOs
{
	public class AddressDtoTest
	{
		[Fact]
		public void ShouldCreateAddressDto()
		{
			AddressDto addressDto = new();

			Assert.Null(addressDto.Line);
			Assert.Null(addressDto.Line2);
			Assert.Null(addressDto.Type);
			Assert.Null(addressDto.City);
			Assert.Null(addressDto.PostalCode);
			Assert.Null(addressDto.State);
			Assert.Null(addressDto.Country);
		}

		[Fact]
		public void ShouldSetAddressDtoProperties()
		{
			var text = "text";

			AddressDto addressDto = new();

			addressDto.Line = text;
			addressDto.Line2 = text;
			addressDto.Type = text;
			addressDto.City = text;
			addressDto.PostalCode = text;
			addressDto.State = text;
			addressDto.Country = text;

			Assert.Equal(text, addressDto.Line);
			Assert.Equal(text, addressDto.Line2);
			Assert.Equal(text, addressDto.Type);
			Assert.Equal(text, addressDto.City);
			Assert.Equal(text, addressDto.PostalCode);
			Assert.Equal(text, addressDto.State);
			Assert.Equal(text, addressDto.Country);
		}
	}
}
