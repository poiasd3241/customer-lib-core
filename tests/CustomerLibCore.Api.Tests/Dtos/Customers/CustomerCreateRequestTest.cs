using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Api.Dtos.Notes.Request;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Customers
{
	public class CustomerCreateRequestTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var customer = new CustomerCreateRequest();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.LastName);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.Email);
			Assert.Null(customer.TotalPurchasesAmount);
			Assert.Null(customer.Addresses);
			Assert.Null(customer.Notes);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			var firstName = "firstName1";
			var lastName = "lastName1";
			var phoneNumber = "phoneNumber1";
			var email = "email1";
			var totalPurchasesAmount = "totalPurchasesAmount1";
			var addresses = new List<AddressRequest>();
			var notes = new List<NoteRequest>();

			var customer = new CustomerCreateRequest();

			Assert.NotEqual(firstName, customer.FirstName);
			Assert.NotEqual(lastName, customer.LastName);
			Assert.NotEqual(phoneNumber, customer.PhoneNumber);
			Assert.NotEqual(email, customer.Email);
			Assert.NotEqual(totalPurchasesAmount, customer.TotalPurchasesAmount);
			Assert.NotEqual(addresses, customer.Addresses);
			Assert.NotEqual(notes, customer.Notes);

			// When
			customer.FirstName = firstName;
			customer.LastName = lastName;
			customer.PhoneNumber = phoneNumber;
			customer.Email = email;
			customer.TotalPurchasesAmount = totalPurchasesAmount;
			customer.Addresses = addresses;
			customer.Notes = notes;

			Assert.Equal(firstName, customer.FirstName);
			Assert.Equal(lastName, customer.LastName);
			Assert.Equal(phoneNumber, customer.PhoneNumber);
			Assert.Equal(email, customer.Email);
			Assert.Equal(totalPurchasesAmount, customer.TotalPurchasesAmount);
			Assert.Equal(addresses, customer.Addresses);
			Assert.Equal(notes, customer.Notes);
		}
	}
}
