using CustomerLibCore.Data.Entities;
using Xunit;

namespace CustomerLibCore.Data.IntegrationTests.Entities
{
	public class CustomerEntityTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			CustomerEntity customer = new();

			Assert.Equal(0, customer.CustomerId);
			Assert.Null(customer.FirstName);
			Assert.Null(customer.LastName);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.Email);
			Assert.Null(customer.TotalPurchasesAmount);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			// Given
			var customerId = 1;
			var firstName = "firstName1";
			var lastName = "lastName1";
			var phoneNumber = "phoneNumber1";
			var email = "email1";
			var totalPurchasesAmount = 123m;

			var customer = new CustomerEntity();

			Assert.NotEqual(customerId, customer.CustomerId);
			Assert.NotEqual(firstName, customer.FirstName);
			Assert.NotEqual(lastName, customer.LastName);
			Assert.NotEqual(phoneNumber, customer.PhoneNumber);
			Assert.NotEqual(email, customer.Email);
			Assert.NotEqual(totalPurchasesAmount, customer.TotalPurchasesAmount);

			// When
			customer.CustomerId = customerId;
			customer.FirstName = firstName;
			customer.LastName = lastName;
			customer.PhoneNumber = phoneNumber;
			customer.Email = email;
			customer.TotalPurchasesAmount = totalPurchasesAmount;

			// Then
			Assert.Equal(customerId, customer.CustomerId);
			Assert.Equal(firstName, customer.FirstName);
			Assert.Equal(lastName, customer.LastName);
			Assert.Equal(phoneNumber, customer.PhoneNumber);
			Assert.Equal(email, customer.Email);
			Assert.Equal(totalPurchasesAmount, customer.TotalPurchasesAmount);
		}

		// TODO: Copy, Equals

	}
}
