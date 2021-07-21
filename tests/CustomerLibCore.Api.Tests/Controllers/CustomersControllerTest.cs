using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.Core.Resource;
using CustomerLibCore.Api.Controllers;
using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Api.Exceptions;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.Business.Enums;
using CustomerLibCore.Business.Exceptions;
using CustomerLibCore.ServiceLayer.Services;
using CustomerLibCore.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerLibCore.Api.Tests.Controllers
{
	public class CustomersControllerTest
	{
		#region Constructors

		[Fact]
		public void ShouldCreateCustomersController()
		{
			var mockCustomerService = new StrictMock<ICustomerService>();
			var mockMapper = new StrictMock<IMapper>();

			var controller = new CustomersController(mockCustomerService.Object, mockMapper.Object);

			Assert.NotNull(controller);
		}

		#endregion

		#region Find all by customer ID

		[Fact]
		public void ShouldFindAllEmtpy()
		{
			// Given
			var mapper = CreateMapper();
			bool includeAddresses = default;
			bool includeNotes = default;

			var customers = new List<Customer>();
			var customersDto = mapper.Map<IEnumerable<CustomerDto>>(customers);

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s => s.FindAll(includeAddresses, includeNotes))
				.Returns(customers);
			fixture.MockMapper.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers))
				.Returns(customersDto);

			var controller = fixture.CreateController();

			// When
			var result = controller.FindAll(includeAddresses, includeNotes).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var items = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(value);
			Assert.Empty(items);

			fixture.MockCustomerService.Verify(s => s.FindAll(includeAddresses, includeNotes),
				Times.Once);
			fixture.MockMapper.Verify(m => m.Map<IEnumerable<CustomerDto>>(customers), Times.Once);
		}

		[Fact]
		public void ShouldFindAllWithAddressesAndNotes()
		{
			// Given
			var mapper = CreateMapper();
			bool includeAddresses = true;
			bool includeNotes = true;

			var customerId1 = 5;
			var customerId2 = 7;

			var customers = new List<Customer>() { MockCustomer(customerId1),
				MockCustomer(customerId2) };

			var addresses1 = new List<Address>() { MockAddress(1, customerId1) };
			var addresses2 = new List<Address>() { MockAddress(2, customerId2),
				MockAddress(3, customerId2)};

			var notes1 = new List<Note>() { MockNote(4, customerId1) };
			var notes2 = new List<Note>() { MockNote(5, customerId2), MockNote(6, customerId2) };

			customers[0].Addresses = addresses1;
			customers[1].Addresses = addresses2;

			customers[0].Notes = notes1;
			customers[1].Notes = notes2;

			var customersDto = mapper.Map<IEnumerable<CustomerDto>>(customers);

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s => s.FindAll(includeAddresses, includeNotes))
				.Returns(customers);
			fixture.MockMapper.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers))
				.Returns(customersDto);

			var controller = fixture.CreateController();

			// When
			var result = controller.FindAll(includeAddresses, includeNotes).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var items = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(value);
			Assert.Equal(customersDto, items);

			fixture.MockCustomerService.Verify(s => s.FindAll(includeAddresses, includeNotes),
				Times.Once);
			fixture.MockMapper.Verify(m => m.Map<IEnumerable<CustomerDto>>(customers), Times.Once);
		}

		[Fact]
		public void ShouldFindAllWithoutAddressesAndNotes()
		{
			// Given
			var mapper = CreateMapper();
			bool includeAddresses = false;
			bool includeNotes = false;

			var customers = new List<Customer>() { MockCustomer(5), MockCustomer(7) };

			var customersDto = mapper.Map<IEnumerable<CustomerDto>>(customers);

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s => s.FindAll(includeAddresses, includeNotes))
				.Returns(customers);
			fixture.MockMapper.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers))
				.Returns(customersDto);

			var controller = fixture.CreateController();

			// When
			var result = controller.FindAll(includeAddresses, includeNotes).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var items = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(value);
			Assert.Equal(customersDto, items);

			fixture.MockCustomerService.Verify(s => s.FindAll(includeAddresses, includeNotes),
				Times.Once);
			fixture.MockMapper.Verify(m => m.Map<IEnumerable<CustomerDto>>(customers), Times.Once);
		}

		#endregion

		#region Get single

		[Fact]
		public void ShouldThrowOnGetSingleWhenProvidedBadId()
		{
			// Given
			var customerId = 0;
			bool includeAddresses = default;
			bool includeNotes = default;

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<RouteArgumentException>(() =>
				controller.Get(customerId, includeAddresses, includeNotes));

			// Then
			Assert.Equal("customerId", exception.ParamName);
			Assert.Equal("ID cannot be less than 1", exception.Message);
		}

		[Fact]
		public void ShouldGetSingleWithAddressesAndNotes()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;
			bool includeAddresses = true;
			bool includeNotes = true;

			var customer = MockCustomer(customerId);

			var addresses = new List<Address>() { MockAddress(1, customerId) };
			var notes = new List<Note>() { MockNote(4, customerId) };

			customer.Addresses = addresses;
			customer.Notes = notes;

			var customerDto = mapper.Map<CustomerDto>(customer);

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s =>
				s.Get(customerId, includeAddresses, includeNotes)).Returns(customer);
			fixture.MockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

			var controller = fixture.CreateController();

			// When
			var result = controller.Get(customerId, includeAddresses, includeNotes).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var item = Assert.IsAssignableFrom<CustomerDto>(value);
			Assert.Equal(customerDto, item);

			fixture.MockCustomerService.Verify(s =>
				s.Get(customerId, includeAddresses, includeNotes), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<CustomerDto>(customer), Times.Once);
		}

		[Fact]
		public void ShouldGetSingleWithoutAddressesAndNotes()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;
			bool includeAddresses = false;
			bool includeNotes = false;

			var customer = MockCustomer(customerId);

			var customerDto = mapper.Map<CustomerDto>(customer);

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s =>
				s.Get(customerId, includeAddresses, includeNotes)).Returns(customer);
			fixture.MockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

			var controller = fixture.CreateController();

			// When
			var result = controller.Get(customerId, includeAddresses, includeNotes).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var item = Assert.IsAssignableFrom<CustomerDto>(value);
			Assert.Equal(customerDto, item);

			fixture.MockCustomerService.Verify(s =>
				s.Get(customerId, includeAddresses, includeNotes), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<CustomerDto>(customer), Times.Once);
		}

		#endregion

		#region Save

		[Fact]
		public void ShouldThrowOnSaveWhenProvidedInvalidBody()
		{
			// Given
			var mapper = CreateMapper();

			var badCustomerDto = MockCustomerDto();
			badCustomerDto.PhoneNumber = "123456";

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var errors = Assert.Throws<InvalidBodyException>(() =>
				controller.Save(badCustomerDto)).Errors;

			//Then
			Assert.Equal(3, errors.Count());

			Assert.Equal(nameof(CustomerDto.PhoneNumber), errors.ElementAt(0).PropertyName);
			Assert.Equal(nameof(CustomerDto.Addresses), errors.ElementAt(1).PropertyName);
			Assert.Equal(nameof(CustomerDto.Notes), errors.ElementAt(2).PropertyName);
		}

		[Fact]
		public void ShouldThrowOnSaveWhenEmailTaken()
		{
			// Given
			var mapper = CreateMapper();

			var customerDto = MockCustomerDtoWithAddressesAndNotes();
			var customer = mapper.Map<Customer>(customerDto);

			var fixture = new CustomersControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Customer>(customerDto)).Returns(customer);
			fixture.MockCustomerService.Setup(s => s.Save(customer)).Throws<EmailTakenException>();

			var controller = fixture.CreateController();

			// When
			var ex = Assert.Throws<ConflictWithExistingException>(() =>
				controller.Save(customerDto));

			//Then
			Assert.Equal("Email is already taken", ex.ConflictMessage);
			Assert.Equal(nameof(CustomerDto.Email), ex.IncomingPropertyName);
			Assert.Equal(customerDto.Email, ex.IncomingPropertyValue);

			fixture.MockMapper.Verify(m => m.Map<Customer>(customerDto), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Save(customer), Times.Once);
		}

		[Fact]
		public void ShouldSave()
		{
			// Given
			var mapper = CreateMapper();

			var customerDto = MockCustomerDto();

			var addressesDto = new List<AddressDto>() { MockAddressDto() };
			var notesDto = new List<NoteDto>() { MockNoteDto() };

			customerDto.Addresses = addressesDto;
			customerDto.Notes = notesDto;

			var customer = mapper.Map<Customer>(customerDto);

			var fixture = new CustomersControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Customer>(customerDto)).Returns(customer);
			fixture.MockCustomerService.Setup(s => s.Save(customer));

			var controller = fixture.CreateController();

			// When
			var result = controller.Save(customerDto);

			//Then
			var okResult = Assert.IsType<OkResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			fixture.MockMapper.Verify(m => m.Map<Customer>(customerDto), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Save(customer), Times.Once);
		}

		#endregion

		#region Update

		[Fact]
		public void ShouldThrowOnUpdateWhenProvidedBadId()
		{
			// Given
			var customerId = 0;
			var customerBasicDetailsDto = new CustomerBasicDetailsDto();

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<RouteArgumentException>(() =>
				controller.Update(customerId, customerBasicDetailsDto));

			// Then
			Assert.Equal("customerId", exception.ParamName);
			Assert.Equal("ID cannot be less than 1", exception.Message);
		}

		[Fact]
		public void ShouldThrowOnUpdateWhenProvidedInvalidBody()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;

			var badCustomerBasicDetailsDto = MockCustomerBasicDetailsDto();
			badCustomerBasicDetailsDto.LastName = null;

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<InvalidBodyException>(() =>
				controller.Update(customerId, badCustomerBasicDetailsDto));

			//Then
			var error = Assert.Single(exception.Errors);

			Assert.Equal(nameof(CustomerDto.LastName), error.PropertyName);
		}

		[Fact]
		public void ShouldThrowOnUpdateWhenEmailTaken()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;

			var badCustomerBasicDetailsDto = MockCustomerBasicDetailsDto();
			var customer = mapper.Map<Customer>(badCustomerBasicDetailsDto);

			var fixture = new CustomersControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Customer>(badCustomerBasicDetailsDto))
				.Returns(customer);
			fixture.MockCustomerService.Setup(s =>
				s.Update(customer)).Throws<EmailTakenException>();

			var controller = fixture.CreateController();

			// When
			var ex = Assert.Throws<ConflictWithExistingException>(() =>
				controller.Update(customerId, badCustomerBasicDetailsDto));

			//Then
			Assert.Equal("Email is already taken", ex.ConflictMessage);
			Assert.Equal(nameof(CustomerBasicDetailsDto.Email), ex.IncomingPropertyName);
			Assert.Equal(badCustomerBasicDetailsDto.Email, ex.IncomingPropertyValue);

			fixture.MockMapper.Verify(m => m.Map<Customer>(badCustomerBasicDetailsDto), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Update(It.Is<Customer>(n =>
				n.CustomerId == customerId)), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Update(customer), Times.Once);
		}

		[Fact]
		public void ShouldUpdate()
		{
			// Given
			var mapper = CreateMapper();
			var customerId = 5;

			var badCustomerBasicDetailsDto = MockCustomerBasicDetailsDto();
			var customer = mapper.Map<Customer>(badCustomerBasicDetailsDto);

			var fixture = new CustomersControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Customer>(badCustomerBasicDetailsDto))
				.Returns(customer);
			fixture.MockCustomerService.Setup(s => s.Update(customer));

			var controller = fixture.CreateController();

			// When
			var result = controller.Update(customerId, badCustomerBasicDetailsDto);

			//Then
			var okResult = Assert.IsType<OkResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			fixture.MockCustomerService.Verify(s => s.Update(customer), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Update(It.Is<Customer>(n =>
				n.CustomerId == customerId)), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<Customer>(badCustomerBasicDetailsDto), Times.Once);
		}

		#endregion

		#region Delete

		[Fact]
		public void ShouldThrowOnDeleteWhenProvidedBadId()
		{
			// Given
			var customerId = 0;

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var exception = Assert.Throws<RouteArgumentException>(() =>
				controller.Delete(customerId));

			// Then
			Assert.Equal("customerId", exception.ParamName);
			Assert.Equal("ID cannot be less than 1", exception.Message);
		}

		[Fact]
		public void ShouldDelete()
		{
			// Given
			var customerId = 5;

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s => s.Delete(customerId));

			var controller = fixture.CreateController();

			// When
			var result = controller.Delete(customerId);

			//Then
			var okResult = Assert.IsType<OkResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			fixture.MockCustomerService.Verify(s => s.Delete(customerId), Times.Once);
		}

		#endregion

		#region Fixture, object mock helpers

		public class CustomersControllerFixture
		{
			public StrictMock<ICustomerService> MockCustomerService { get; set; }
			public StrictMock<IMapper> MockMapper { get; set; }

			public CustomersControllerFixture()
			{
				MockCustomerService = new();
				MockMapper = new();
			}

			public CustomersController CreateController() =>
				new(MockCustomerService.Object, MockMapper.Object);
		}

		public static IMapper CreateMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new AutoMapperApiProfile());
			});

			return config.CreateMapper();
		}

		public static Address MockAddress(int addressId, int customerId) => new()
		{
			AddressId = addressId,
			CustomerId = customerId,
			Line = "line one",
			Line2 = "line two",
			Type = AddressType.Shipping,
			City = "city x",
			PostalCode = "3241",
			State = "state x",
			Country = "Canada"
		};

		public static AddressDto MockAddressDto() => new()
		{
			Line = "line one",
			Line2 = "line two",
			Type = "Shipping",
			City = "city x",
			PostalCode = "3241",
			State = "state x",
			Country = "Canada"
		};

		public static Note MockNote(int noteId, int customerId) => new()
		{
			NoteId = noteId,
			CustomerId = customerId,
			Content = "MockNote content"
		};

		public static NoteDto MockNoteDto() => new()
		{
			Content = "MockNoteDto content"
		};

		public static Customer MockCustomer(int customerId) => new()
		{
			CustomerId = customerId,
			FirstName = "One",
			LastName = "Two",
			PhoneNumber = "+123",
			Email = "a@a.aa",
			TotalPurchasesAmount = 666,
			Addresses = null,
			Notes = null
		};

		public static CustomerDto MockCustomerDto() => new()
		{
			FirstName = "One",
			LastName = "Two",
			PhoneNumber = "+123",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666",
			Addresses = null,
			Notes = null
		};

		public static CustomerDto MockCustomerDtoWithAddressesAndNotes() => new()
		{
			FirstName = "One",
			LastName = "Two",
			PhoneNumber = "+123",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666",
			Addresses = new() { MockAddressDto() },
			Notes = new() { MockNoteDto() }
		};

		public static CustomerBasicDetailsDto MockCustomerBasicDetailsDto() => new()
		{
			FirstName = "One",
			LastName = "Two",
			PhoneNumber = "+123",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666"
		};

		#endregion
	}
}
