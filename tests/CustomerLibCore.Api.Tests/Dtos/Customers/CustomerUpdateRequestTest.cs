﻿using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Api.Dtos.Customers.Request;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Customers
{
	public class CustomerUpdateRequestTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var customer = new CustomerUpdateRequest();

			Assert.Null(customer.FirstName);
			Assert.Null(customer.LastName);
			Assert.Null(customer.PhoneNumber);
			Assert.Null(customer.Email);
			Assert.Null(customer.TotalPurchasesAmount);
		}

		[Fact]
		public void ShouldSetProperties()
		{
			var firstName = "firstName1";
			var lastName = "lastName1";
			var phoneNumber = "phoneNumber1";
			var email = "email1";
			var totalPurchasesAmount = "totalPurchasesAmount1";

			var customer = new CustomerUpdateRequest();

			Assert.NotEqual(firstName, customer.FirstName);
			Assert.NotEqual(lastName, customer.LastName);
			Assert.NotEqual(phoneNumber, customer.PhoneNumber);
			Assert.NotEqual(email, customer.Email);
			Assert.NotEqual(totalPurchasesAmount, customer.TotalPurchasesAmount);

			// When
			customer.FirstName = firstName;
			customer.LastName = lastName;
			customer.PhoneNumber = phoneNumber;
			customer.Email = email;
			customer.TotalPurchasesAmount = totalPurchasesAmount;

			Assert.Equal(firstName, customer.FirstName);
			Assert.Equal(lastName, customer.LastName);
			Assert.Equal(phoneNumber, customer.PhoneNumber);
			Assert.Equal(email, customer.Email);
			Assert.Equal(totalPurchasesAmount, customer.TotalPurchasesAmount);
		}
	}
}