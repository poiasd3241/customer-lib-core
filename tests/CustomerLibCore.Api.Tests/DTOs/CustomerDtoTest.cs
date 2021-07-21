using System.Collections.Generic;
using CustomerLibCore.Api.DTOs;
using Xunit;

namespace CustomerLibCore.Api.Tests.DTOs
{
	public class CustomerDtoTest
	{
		[Fact]
		public void ShouldCreateCustomerDto()
		{
			CustomerDto customer = new();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.LastName);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.Email);
			Assert.Null(customer.TotalPurchasesAmount);
			Assert.Null(customer.Addresses);
			Assert.Null(customer.Notes);
		}

		[Fact]
		public void ShouldSetCustomerDtoProperties()
		{
			var text = "text";
			var addresses = new List<AddressDto>() { new() };
			var notes = new List<NoteDto>() { new() };

			CustomerDto customer = new();

			customer.FirstName = text;
			customer.LastName = text;
			customer.PhoneNumber = text;
			customer.Email = text;
			customer.TotalPurchasesAmount = text;
			customer.Addresses = addresses;
			customer.Notes = notes;

			Assert.Equal(text, customer.FirstName);
			Assert.Equal(text, customer.LastName);
			Assert.Equal(text, customer.PhoneNumber);
			Assert.Equal(text, customer.Email);
			Assert.Equal(text, customer.TotalPurchasesAmount);
			Assert.Equal(addresses, customer.Addresses);
			Assert.Equal(notes, customer.Notes);
		}
	}
}
