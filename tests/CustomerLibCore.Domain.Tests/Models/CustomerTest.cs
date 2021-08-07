using System;
using System.Collections.Generic;
using CustomerLibCore.Domain.Models;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Entities
{
	public class CustomerTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			Customer customer = new();

			Assert.Equal(0, customer.CustomerId);
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
			// Given
			var customerId = 1;
			var firstName = "firstName1";
			var lastName = "lastName1";
			var phoneNumber = "phoneNumber1";
			var email = "email1";
			var totalPurchasesAmount = 123m;
			var addresses = new List<Address>();
			var notes = new List<Note>();

			var customer = new Customer();

			Assert.NotEqual(customerId, customer.CustomerId);
			Assert.NotEqual(firstName, customer.FirstName);
			Assert.NotEqual(lastName, customer.LastName);
			Assert.NotEqual(phoneNumber, customer.PhoneNumber);
			Assert.NotEqual(email, customer.Email);
			Assert.NotEqual(totalPurchasesAmount, customer.TotalPurchasesAmount);
			Assert.NotEqual(addresses, customer.Addresses);
			Assert.NotEqual(notes, customer.Notes);

			// When
			customer.CustomerId = customerId;
			customer.FirstName = firstName;
			customer.LastName = lastName;
			customer.PhoneNumber = phoneNumber;
			customer.Email = email;
			customer.TotalPurchasesAmount = totalPurchasesAmount;
			customer.Addresses = addresses;
			customer.Notes = notes;

			// Then
			Assert.Equal(customerId, customer.CustomerId);
			Assert.Equal(firstName, customer.FirstName);
			Assert.Equal(lastName, customer.LastName);
			Assert.Equal(phoneNumber, customer.PhoneNumber);
			Assert.Equal(email, customer.Email);
			Assert.Equal(totalPurchasesAmount, customer.TotalPurchasesAmount);
			Assert.Equal(addresses, customer.Addresses);
			Assert.Equal(notes, customer.Notes);
		}

		//#region Equals by value

		//[Fact]
		//public void ShouldThrowOnEqualsByValueByBadObjectType()
		//{
		//	// Given
		//	var customer1 = new Customer();
		//	var whatever = "whatever";

		//	// When
		//	var exception = Assert.Throws<ArgumentException>(() =>
		//		customer1.EqualsByValue(whatever));

		//	// Then
		//	Assert.Equal("Must use the same entity type for comparison", exception.Message);
		//}

		//[Fact]
		//public void ShouldConfirmEqualsByValue()
		//{
		//	// Given
		//	var customer1 = MockCustomer();
		//	var customer2 = MockCustomer();

		//	// When
		//	var equalsByValue = customer1.EqualsByValue(customer2);

		//	// Then
		//	Assert.True(equalsByValue);
		//}

		//[Fact]
		//public void ShouldRefuteEqualsByValueByNull()
		//{
		//	// Given
		//	var customer1 = MockCustomer();
		//	Customer customer2 = null;

		//	// When
		//	var equalsByValue = customer1.EqualsByValue(customer2);

		//	// Then
		//	Assert.False(equalsByValue);
		//}

		//[Fact]
		//public void ShouldRefuteEqualsByValueByCustomerId()
		//{
		//	// Given
		//	var customerId1 = 5;
		//	var customerId2 = 7;

		//	var customer1 = MockCustomer();
		//	var customer2 = MockCustomer();

		//	customer1.CustomerId = customerId1;
		//	customer2.CustomerId = customerId2;

		//	// When
		//	var equalsByValue = customer1.EqualsByValue(customer2);

		//	// Then
		//	Assert.False(equalsByValue);
		//}

		//[Fact]
		//public void ShouldRefuteEqualsByValueByAddresses()
		//{
		//	// Given
		//	var addresses1 = new List<Address>();
		//	var addresses2 = new List<Address>() { new() };

		//	var customer1 = MockCustomer();
		//	var customer2 = MockCustomer();

		//	customer1.Addresses = addresses1;
		//	customer2.Addresses = addresses2;

		//	// When
		//	var equalsByValue = customer1.EqualsByValue(customer2);

		//	// Then
		//	Assert.False(equalsByValue);
		//}

		//[Fact]
		//public void ShouldRefuteEqualsByValueByNotes()
		//{
		//	// Given
		//	var notes1 = new List<Note>();
		//	var notes2 = new List<Note>() { new() };

		//	var customer1 = MockCustomer();
		//	var customer2 = MockCustomer();

		//	customer1.Notes = notes1;
		//	customer2.Notes = notes2;

		//	// When
		//	var equalsByValue = customer1.EqualsByValue(customer2);

		//	// Then
		//	Assert.False(equalsByValue);
		//}

		//#endregion

		//private static Customer MockCustomer() => new()
		//{
		//	CustomerId = 8,
		//	FirstName = "one",
		//	LastName = "two",
		//	PhoneNumber = "+123",
		//	Email = "a@a.aa",
		//	TotalPurchasesAmount = 666,
		//	Addresses = new(),
		//	Notes = new() { null },
		//};
	}
}
