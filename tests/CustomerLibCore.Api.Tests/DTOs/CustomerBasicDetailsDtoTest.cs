using CustomerLibCore.Api.DTOs;
using Xunit;

namespace CustomerLibCore.Api.Tests.DTOs
{
	public class CustomerBasicDetailsDtoTest
	{
		[Fact]
		public void ShouldCreateCustomerBasicDetailsDto()
		{
			CustomerBasicDetailsDto customer = new();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.LastName);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.Email);
			Assert.Null(customer.TotalPurchasesAmount);
		}

		[Fact]
		public void ShouldSetCustomerBasicDetailsDtoProperties()
		{
			var text = "text";

			CustomerBasicDetailsDto customer = new();

			customer.FirstName = text;
			customer.LastName = text;
			customer.PhoneNumber = text;
			customer.Email = text;
			customer.TotalPurchasesAmount = text;

			Assert.Equal(text, customer.FirstName);
			Assert.Equal(text, customer.LastName);
			Assert.Equal(text, customer.PhoneNumber);
			Assert.Equal(text, customer.Email);
			Assert.Equal(text, customer.TotalPurchasesAmount);
		}
	}
}
