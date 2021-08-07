using System;
using System.Collections.Generic;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Enums;
using Xunit;
using CustomerLibCore.Api.Dtos.Addresses.Request;

namespace CustomerLibCore.Domain.Tests.Entities
{
	public class AddressTest
	{
		[Fact]
		public void ShouldCreateObject()
		{
			var address = new Address();

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

			var address = new Address();

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

		//#region Equals by value

		//[Fact]
		//public void ShouldThrowOnEqualsByValueByBadObjectType()
		//{
		//	// Given
		//	var address1 = new Address();
		//	var whatever = "whatever";

		//	// When
		//	var exception = Assert.Throws<ArgumentException>(() =>
		//		address1.EqualsByValue(whatever));

		//	// Then
		//	Assert.Equal("Must use the same entity type for comparison", exception.Message);
		//}

		//[Fact]
		//public void ShouldConfirmEqualsByValue()
		//{
		//	// Given
		//	var address1 = MockAddress();
		//	var address2 = MockAddress();

		//	// When
		//	var equalsByValue = address1.EqualsByValue(address2);

		//	// Then
		//	Assert.True(equalsByValue);
		//}

		//[Fact]
		//public void ShouldRefuteEqualsByValueByNull()
		//{
		//	// Given
		//	var address1 = MockAddress();
		//	Address address2 = null;

		//	// When
		//	var equalsByValue = address1.EqualsByValue(address2);

		//	// Then
		//	Assert.False(equalsByValue);
		//}

		//[Fact]
		//public void ShouldRefuteEqualsByValueByAddressId()
		//{
		//	// Given
		//	var addressId1 = 5;
		//	var addressId2 = 7;

		//	var address1 = MockAddress();
		//	var address2 = MockAddress();

		//	address1.AddressId = addressId1;
		//	address2.AddressId = addressId2;

		//	// When
		//	var equalsByValue = address1.EqualsByValue(address2);

		//	// Then
		//	Assert.False(equalsByValue);
		//}

		//#endregion

		//#region Lists equal by value

		//private class NullAndNotNullListsData : TheoryData<List<Address>, List<Address>>
		//{
		//	public NullAndNotNullListsData()
		//	{
		//		Add(null, new());
		//		Add(new(), null);
		//	}
		//}

		//[Theory]
		//[ClassData(typeof(NullAndNotNullListsData))]
		//public void ShouldRefuteListsEqualByValueByOneListNull(
		//	List<Address> list1, List<Address> list2)
		//{
		//	// When
		//	var equalByValue = Address.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.False(equalByValue);
		//}

		//[Fact]
		//public void ShouldRefuteListsEqualByValueByCountMismatch()
		//{
		//	// Given
		//	var list1 = new List<Address>();
		//	var list2 = new List<Address>() { new() };

		//	// When
		//	var equalByValue = Address.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.False(equalByValue);
		//}

		//[Fact]
		//public void ShouldConfirmListsEqualByValueByBothNull()
		//{
		//	// Given
		//	List<Address> list1 = null;
		//	List<Address> list2 = null;

		//	// When
		//	var equalByValue = Address.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.True(equalByValue);
		//}

		//private class NotNullEqualListsData : TheoryData<List<Address>, List<Address>>
		//{
		//	public NotNullEqualListsData()
		//	{
		//		Add(new(), new());

		//		Add(new() { null }, new() { null });
		//		Add(new() { MockAddress() }, new() { MockAddress() });
		//	}
		//}

		//[Theory]
		//[ClassData(typeof(NotNullEqualListsData))]
		//public void ShouldConfirmListsEqualNotNull(List<Address> list1, List<Address> list2)
		//{
		//	// When
		//	var equalByValue = Address.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.True(equalByValue);
		//}

		//private class NotNullNotEqualListsData : TheoryData<List<Address>, List<Address>>
		//{
		//	public NotNullNotEqualListsData()
		//	{
		//		Add(new() { null }, new() { MockAddress() });
		//		Add(new() { MockAddress() }, new() { null });

		//		var addressId1 = 5;
		//		var addressId2 = 7;

		//		var address1 = MockAddress();
		//		var address2 = MockAddress();

		//		address1.AddressId = addressId1;
		//		address2.AddressId = addressId2;

		//		Add(new() { address1 }, new() { address2 });
		//	}
		//}

		//[Theory]
		//[ClassData(typeof(NotNullNotEqualListsData))]
		//public void ShouldRefuteListsEqualNotNull(List<Address> list1, List<Address> list2)
		//{
		//	// When
		//	var equalByValue = Address.ListsEqualByValues(list1, list2);

		//	// Then
		//	Assert.False(equalByValue);
		//}

		//#endregion

		//[Fact]
		//public void ShouldCopyAddress()
		//{
		//	var address = MockAddress();

		//	var copy = address.Copy();

		//	Assert.NotEqual(address, copy);

		//	Assert.True(address.EqualsByValue(copy));
		//}

		//private static Address MockAddress() => new()
		//{
		//	AddressId = 5,
		//	CustomerId = 8,
		//	Line = "one",
		//	Line2 = "two",
		//	Type = AddressType.Shipping,
		//	City = "Seattle",
		//	PostalCode = "123456",
		//	State = "WA",
		//	Country = "United States"
		//};
	}
}
