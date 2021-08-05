//using System;
//using System.Collections.Generic;
//using CustomerLibCore.Domain.Models;
//using CustomerLibCore.Domain.Enums;
//using CustomerLibCore.Domain.Exceptions;
//using CustomerLibCore.Domain.Validators;
//using CustomerLibCore.Data.Repositories;
//using CustomerLibCore.ServiceLayer.Services.Implementations;
//using CustomerLibCore.TestHelpers;
//using Moq;
//using Xunit;
//using static CustomerLibCore.TestHelpers.FluentValidation.ValidationFailureExtensions;

//namespace CustomerLibCore.ServiceLayer.Tests.Services
//{
//	public class AddressServiceTest
//	{
//		#region Constructors

//		[Fact]
//		public void ShouldCreateAddressServiceDefault()
//		{
//			var addressService = new AddressService();

//			Assert.NotNull(addressService);
//		}

//		[Fact]
//		public void ShouldCreateAddressService()
//		{
//			var mockCustomerRepo = new StrictMock<ICustomerRepository>();
//			var mockAddressRepo = new StrictMock<IAddressRepository>();

//			var addressService = new AddressService(mockCustomerRepo.Object, mockAddressRepo.Object);

//			Assert.NotNull(addressService);
//		}

//		#endregion

//		#region Save

//		[Fact]
//		public void ShouldThrowOnSaveWhenProvidedBadId()
//		{
//			// Given
//			var address = new Address() { CustomerId = 0 };
//			var service = new AddressServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.Save(address));

//			// Then
//			Assert.Equal("CustomerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnSaveWhenProvidedInvalidAddress()
//		{
//			// Given
//			var customerId = 5;

//			var invalidAddress = new Address() { CustomerId = customerId };
//			var expectedErrors = new AddressValidator().Validate(invalidAddress).Errors;

//			var service = new AddressServiceFixture().CreateService();

//			// When
//			var actualErrors = Assert.Throws<InternalValidationException>(() =>
//				service.Save(invalidAddress)).Errors;

//			// Then
//			AssertValidationFailuresEqualByPropertyNameAndErrorMessage(
//				expectedErrors, actualErrors, 6);
//			AssertContainPropertyNames(actualErrors, new[] {
//				nameof(Address.Line),
//				nameof(Address.Type),
//				nameof(Address.City),
//				nameof(Address.PostalCode),
//				nameof(Address.State),
//				nameof(Address.Country) });
//		}

//		[Fact]
//		public void ShouldThrowOnSaveWhenCustomerNotFound()
//		{
//			// Given
//			var customerId = 5;
//			var address = AddressServiceFixture.MockAddress();
//			address.CustomerId = customerId;

//			var fixture = new AddressServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.Save(address));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldSave()
//		{
//			// Given
//			var customerId = 5;
//			var address = AddressServiceFixture.MockAddress();
//			address.CustomerId = customerId;

//			var fixture = new AddressServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);
//			fixture.MockAddressRepository.Setup(r => r.Create(address)).Returns(8);

//			var service = fixture.CreateService();

//			// When
//			service.Save(address);

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockAddressRepository.Verify(r => r.Create(address), Times.Once);
//		}

//		#endregion

//		#region Get single

//		[Theory]
//		[InlineData(0, 1, "addressId")]
//		[InlineData(1, 0, "customerId")]
//		[InlineData(0, 0, "addressId")]
//		public void ShouldThrowOnGetAddressByIdForCustomerWhenProvidedBadIds(
//		int addressId, int customerId, string paramName)
//		{
//			// Given
//			var service = new AddressServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.GetForCustomer(addressId, customerId));

//			// Then
//			Assert.Equal(paramName, exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnGetAddressByIdForCustomerWhenResourceNotFound()
//		{
//			// Given
//			var addressId = 5;
//			var customerId = 7;

//			var fixture = new AddressServiceFixture();
//			fixture.MockAddressRepository.Setup(r => r.ReadForCustomer(addressId, customerId))
//				.Returns(() => null);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.GetForCustomer(addressId, customerId));

//			// Then
//			fixture.MockAddressRepository.Verify(r => r.ReadForCustomer(addressId, customerId),
//				Times.Once);
//		}

//		[Fact]
//		public void ShouldGetAddressByIdForCustomer()
//		{
//			// Given
//			var addressId = 5;
//			var customerId = 7;
//			var expectedAddress = AddressServiceFixture.MockAddress();

//			var fixture = new AddressServiceFixture();
//			fixture.MockAddressRepository.Setup(r => r.ReadForCustomer(addressId, customerId))
//				.Returns(expectedAddress);

//			var service = fixture.CreateService();

//			// When
//			var address = service.GetForCustomer(addressId, customerId);

//			// Then
//			Assert.Equal(expectedAddress, address);
//			fixture.MockAddressRepository.Verify(r => r.ReadForCustomer(addressId, customerId),
//				Times.Once);
//		}

//		#endregion

//		#region Find all

//		[Fact]
//		public void ShouldThrowOnFindAllForCustomerWhenProvidedBadId()
//		{
//			// Given
//			var service = new AddressServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() => service.FindAllForCustomer(0));

//			// Then
//			Assert.Equal("customerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnFindAllForCustomerWhenCustomerNotFound()
//		{
//			// Given
//			var customerId = 5;

//			var fixture = new AddressServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.FindAllForCustomer(customerId));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldFindAllForCustomer()
//		{
//			// Given
//			var customerId = 5;
//			var expectedAddresses = AddressServiceFixture.MockAddresses();

//			var fixture = new AddressServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);

//			fixture.MockAddressRepository.Setup(r => r.ReadManyForCustomer(customerId))
//				.Returns(expectedAddresses);

//			var service = fixture.CreateService();

//			// When
//			var addresses = service.FindAllForCustomer(customerId);

//			// Then
//			Assert.Equal(expectedAddresses, addresses);

//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockAddressRepository.Verify(r => r.ReadManyForCustomer(customerId),
//				Times.Once);
//		}

//		#endregion

//		#region Update

//		[Theory]
//		[InlineData(0, 1, "AddressId")]
//		[InlineData(1, 0, "CustomerId")]
//		public void ShouldThrowOnUpdateForCustomerWhenProvidedBadIds(
//			int addressId, int customerId, string paramName)
//		{
//			// Given
//			var address = new Address() { AddressId = addressId, CustomerId = customerId };

//			Assert.Equal(addressId, address.AddressId);
//			Assert.Equal(customerId, address.CustomerId);

//			var service = new AddressServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.UpdateForCustomer(address));

//			// Then
//			Assert.Equal(paramName, exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnUpdateForCustomerWhenProvidedInvalidAddress()
//		{
//			// Given
//			var addressId = 5;
//			var customerId = 7;
//			var invalidAddress = new Address() { AddressId = addressId, CustomerId = customerId };

//			var expectedErrors = new AddressValidator().Validate(invalidAddress).Errors;

//			var service = new AddressServiceFixture().CreateService();

//			// When
//			var actualErrors = Assert.Throws<InternalValidationException>(() =>
//				service.UpdateForCustomer(invalidAddress)).Errors;

//			// Then
//			AssertValidationFailuresEqualByPropertyNameAndErrorMessage(
//							expectedErrors, actualErrors, 6);
//			AssertContainPropertyNames(actualErrors, new[] {
//				nameof(Address.Line),
//				nameof(Address.Type),
//				nameof(Address.City),
//				nameof(Address.PostalCode),
//				nameof(Address.State),
//				nameof(Address.Country) });
//		}

//		[Fact]
//		public void ShouldThrowOnUpdateForCustomerWhenResourceNotFound()
//		{
//			// Given
//			var addressId = 5;
//			var customerId = 7;

//			var address = AddressServiceFixture.MockAddress();
//			address.AddressId = addressId;
//			address.CustomerId = customerId;

//			var fixture = new AddressServiceFixture();
//			fixture.MockAddressRepository.Setup(r => r.ExistsForCustomer(addressId, customerId))
//				.Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.UpdateForCustomer(address));

//			// Then
//			fixture.MockAddressRepository.Verify(r => r.ExistsForCustomer(addressId, customerId),
//				Times.Once);
//		}

//		[Fact]
//		public void ShouldUpdateForCustomer()
//		{
//			// Given
//			var addressId = 5;
//			var customerId = 7;

//			var address = AddressServiceFixture.MockAddress();
//			address.AddressId = addressId;
//			address.CustomerId = customerId;

//			var fixture = new AddressServiceFixture();
//			fixture.MockAddressRepository.Setup(r => r.ExistsForCustomer(addressId, customerId))
//				.Returns(true);
//			fixture.MockAddressRepository.Setup(r => r.Update(address));

//			var service = fixture.CreateService();

//			// When
//			service.UpdateForCustomer(address);

//			// Then
//			fixture.MockAddressRepository.Verify(r => r.ExistsForCustomer(addressId, customerId),
//				Times.Once);
//			fixture.MockAddressRepository.Verify(r => r.Update(address), Times.Once);
//		}

//		#endregion

//		#region Delete single

//		[Theory]
//		[InlineData(0, 1, "addressId")]
//		[InlineData(1, 0, "customerId")]
//		[InlineData(0, 0, "addressId")]
//		public void ShouldThrowOnDeleteForCustomerWhenProvidedBadIds(
//			int addressId, int customerId, string paramName)
//		{
//			// Given
//			var service = new AddressServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.DeleteForCustomer(addressId, customerId));

//			// Then
//			Assert.Equal(paramName, exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnDeleteForCustomerWhenResourceNotFound()
//		{
//			// Given
//			var addressId = 5;
//			var customerId = 7;

//			var fixture = new AddressServiceFixture();
//			fixture.MockAddressRepository.Setup(r => r.ExistsForCustomer(addressId, customerId))
//				.Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() =>
//				service.DeleteForCustomer(addressId, customerId));


//			// Then
//			fixture.MockAddressRepository.Verify(r => r.ExistsForCustomer(addressId, customerId),
//				Times.Once);
//		}

//		[Fact]
//		public void ShouldDeleteForCustomer()
//		{
//			// Given
//			var addressId = 5;
//			var customerId = 7;

//			var fixture = new AddressServiceFixture();
//			fixture.MockAddressRepository.Setup(r => r.ExistsForCustomer(addressId, customerId))
//				.Returns(true);
//			fixture.MockAddressRepository.Setup(r => r.Delete(addressId));

//			var service = fixture.CreateService();

//			// When
//			service.DeleteForCustomer(addressId, customerId);

//			// Then
//			fixture.MockAddressRepository.Verify(r => r.ExistsForCustomer(addressId, customerId),
//				Times.Once);
//			fixture.MockAddressRepository.Verify(r => r.Delete(addressId), Times.Once);
//		}

//		#endregion

//		#region Delete all

//		[Fact]
//		public void ShouldThrowOnDeleteAllForCustomerWhenProvidedBadId()
//		{
//			// Given
//			var service = new AddressServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() => service.DeleteAllForCustomer(0));

//			// Then
//			Assert.Equal("customerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnDeleteAllForCustomerWhenCustomerNotFound()
//		{
//			// Given
//			var customerId = 5;

//			var fixture = new AddressServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.DeleteAllForCustomer(customerId));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldDeleteAllForCustomer()
//		{
//			// Given
//			var customerId = 5;

//			var fixture = new AddressServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);

//			fixture.MockAddressRepository.Setup(r => r.DeleteManyForCustomer(customerId));

//			var service = fixture.CreateService();

//			// When
//			service.DeleteAllForCustomer(customerId);

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockAddressRepository.Verify(r => r.DeleteManyForCustomer(customerId),
//				Times.Once);
//		}

//		#endregion
//	}

//	public class AddressServiceFixture
//	{
//		/// <returns>The mocked address with repo-relevant valid properties,
//		/// optional properties not null.</returns>
//		public static Address MockAddress() => new()
//		{
//			Line = "line",
//			Line2 = "line2",
//			Type = AddressType.Shipping,
//			City = "city",
//			PostalCode = "code",
//			State = "state",
//			Country = "United States"
//		};

//		/// <returns>The list containing 2 mocked address with repo-relevant valid properties,
//		/// optional properties not null</returns>
//		public static List<Address> MockAddresses() => new() { MockAddress(), MockAddress() };

//		public StrictMock<ICustomerRepository> MockCustomerRepository { get; set; }
//		public StrictMock<IAddressRepository> MockAddressRepository { get; set; }

//		public AddressServiceFixture()
//		{
//			MockCustomerRepository = new();
//			MockAddressRepository = new();
//		}

//		public AddressService CreateService() =>
//			new(MockCustomerRepository.Object, MockAddressRepository.Object);
//	}
//}
