using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.Core.Resource;
using CustomerLibCore.Api.Controllers;
using CustomerLibCore.Api.Dtos;
using CustomerLibCore.Api.Exceptions;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.ServiceLayer.Services;
using CustomerLibCore.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.Api.Tests.Dtos.Validators.Customers;
using CustomerLibCore.Api.Dtos.Customers.Response;
using CustomerLibCore.TestHelpers.ModelsAssert;
using CustomerLibCore.TestHelpers.FluentValidation;
using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Notes.Request;

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

		#region Get page

		[Theory]
		[InlineData(-1, 0, "addresses")]
		[InlineData(2, 0, "addresses")]
		[InlineData(0, -1, "notes")]
		[InlineData(0, 2, "notes")]
		public void ShouldThrowOnGetPageWhenProvidedBadQueryArguments(
			int addresses, int notes, string paramName)
		{
			// Given
			var page = 1;
			var pageSize = 1;

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var ex = Assert.Throws<QueryArgumentException>(() =>
				controller.GetPage(page, pageSize, addresses, notes));

			// Then
			Assert.Equal(paramName, ex.ParamName);
			Assert.Equal(ErrorMessages.INT_FLAG, ex.Message);
		}

		[Fact]
		public void ShouldThrowOnGetPageOnInvalidResponse()
		{
			// Given
			var addresses = 0;
			var notes = 0;
			var includeAddresses = CheckUrlArgument.Flag(addresses, null);
			var includeNotes = CheckUrlArgument.Flag(notes, null);

			var customers = MockCustomerPagedResult();
			var page = customers.Page;
			var pageSize = customers.PageSize;

			var (response, details) = new CustomerPagedResponseValidatorFixture()
				.MockInvalidWithDetails();

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s =>
				s.GetPage(page, pageSize, includeAddresses, includeNotes)).Returns(customers);
			fixture.MockMapper.Setup(m => m.Map<CustomerPagedResponse>(customers))
				.Returns(response);

			var controller = fixture.CreateController();

			// When
			var errors = Assert.Throws<InternalValidationException>(() =>
				controller.GetPage(page, pageSize, addresses, notes)).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);

			fixture.MockCustomerService.Verify(s =>
				s.GetPage(page, pageSize, includeAddresses, includeNotes), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<CustomerPagedResponse>(customers), Times.Once);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 1)]
		[InlineData(1, 0)]
		[InlineData(1, 1)]
		public void ShouldGetPage(int addresses, int notes)
		{
			// Given
			var includeAddresses = CheckUrlArgument.Flag(addresses, null);
			var includeNotes = CheckUrlArgument.Flag(notes, null);

			var customers = MockCustomerPagedResult();
			var page = customers.Page;
			var pageSize = customers.PageSize;

			if (includeAddresses == false)
			{
				foreach (var customer in customers.Items)
				{
					customer.Addresses = null;
				}
			}

			if (includeNotes == false)
			{
				foreach (var customer in customers.Items)
				{
					customer.Notes = null;
				}
			}

			var response = Map<CustomerPagedResponse>(customers);

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s =>
				s.GetPage(page, pageSize, includeAddresses, includeNotes)).Returns(customers);
			fixture.MockMapper.Setup(m => m.Map<CustomerPagedResponse>(customers))
				.Returns(response);

			var controller = fixture.CreateController();

			// When
			var result = controller.GetPage(page, pageSize, addresses, notes).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var actualResponse = Assert.IsAssignableFrom<CustomerPagedResponse>(value);
			Assert.Equal(response, actualResponse);

			foreach (var item in actualResponse.Items)
			{
				Assert.Equal(includeAddresses, item.Addresses.Items is not null);
				Assert.Equal(includeNotes, item.Notes.Items is not null);
			}

			fixture.MockCustomerService.Verify(s =>
				s.GetPage(page, pageSize, includeAddresses, includeNotes), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<CustomerPagedResponse>(customers), Times.Once);
		}

		#endregion

		#region Get single

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		public void ShouldThrowOnGetSingleWhenProvidedBadRouteArguments(int customerId)
		{
			// Given
			var paramName = "customerId";
			var addresses = 0;
			var notes = 0;

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var ex = Assert.Throws<RouteArgumentException>(() =>
				controller.Get(customerId, addresses, notes));

			// Then
			Assert.Equal(paramName, ex.ParamName);
			Assert.Equal(ErrorMessages.ID, ex.Message);
		}

		[Theory]
		[InlineData(-1, 0, "addresses")]
		[InlineData(2, 0, "addresses")]
		[InlineData(0, -1, "notes")]
		[InlineData(0, 2, "notes")]
		public void ShouldThrowOnGetSingleWhenProvidedBadQueryArguments(
			int addresses, int notes, string paramName)
		{
			// Given
			var customerId = 1;

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var ex = Assert.Throws<QueryArgumentException>(() =>
				controller.Get(customerId, addresses, notes));

			// Then
			Assert.Equal(paramName, ex.ParamName);
			Assert.Equal(ErrorMessages.INT_FLAG, ex.Message);
		}

		[Fact]
		public void ShouldThrowOnGetSingleOnInvalidResponse()
		{
			// Given
			var addresses = 0;
			var notes = 0;
			var includeAddresses = CheckUrlArgument.Flag(addresses, null);
			var includeNotes = CheckUrlArgument.Flag(notes, null);

			var customer = MockCustomer();
			var customerId = customer.CustomerId;

			var (response, details) = new CustomerResponseValidatorFixture()
				.MockInvalidWithDetails();

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s =>
				s.Get(customerId, includeAddresses, includeNotes)).Returns(customer);
			fixture.MockMapper.Setup(m => m.Map<CustomerResponse>(customer)).Returns(response);

			var controller = fixture.CreateController();

			// When
			var errors = Assert.Throws<InternalValidationException>(() =>
				controller.Get(customerId, addresses, notes)).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);

			fixture.MockCustomerService.Verify(s =>
				s.Get(customerId, includeAddresses, includeNotes), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<CustomerResponse>(customer), Times.Once);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 1)]
		[InlineData(1, 0)]
		[InlineData(1, 1)]
		public void ShouldGetSingle(int addresses, int notes)
		{
			// Given
			var includeAddresses = CheckUrlArgument.Flag(addresses, null);
			var includeNotes = CheckUrlArgument.Flag(notes, null);

			var customer = MockCustomer();
			var customerId = customer.CustomerId;

			if (includeAddresses == false)
			{
				customer.Addresses = null;
			}

			if (includeNotes == false)
			{
				customer.Notes = null;
			}

			var response = Map<CustomerResponse>(customer);

			var fixture = new CustomersControllerFixture();
			fixture.MockCustomerService.Setup(s =>
				s.Get(customerId, includeAddresses, includeNotes)).Returns(customer);
			fixture.MockMapper.Setup(m => m.Map<CustomerResponse>(customer)).Returns(response);

			var controller = fixture.CreateController();

			// When
			var result = controller.Get(customerId, addresses, notes).Result;

			//Then
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			var value = okResult.Value;
			var item = Assert.IsAssignableFrom<CustomerResponse>(value);
			Assert.Equal(response, item);
			Assert.Equal(includeAddresses, item.Addresses.Items is not null);
			Assert.Equal(includeNotes, item.Notes.Items is not null);

			fixture.MockCustomerService.Verify(s =>
				s.Get(customerId, includeAddresses, includeNotes), Times.Once);
			fixture.MockMapper.Verify(m => m.Map<CustomerResponse>(customer), Times.Once);
		}

		#endregion

		#region Create

		[Fact]
		public void ShouldThrowOnCreateWhenProvidedInvalidBody()
		{
			// Given
			var (request, details) = new CustomerCreateRequestValidatorFixture()
				.MockInvalidWithDetails();

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var errors = Assert.Throws<InvalidBodyException>(() =>
				controller.Create(request)).Errors;

			//Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		[Fact]
		public void ShouldThrowOnCreateWhenEmailTaken()
		{
			// Given
			var request = MockCustomerCreateRequest();
			var customer = Map<Customer>(request);

			var fixture = new CustomersControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);
			fixture.MockCustomerService.Setup(s => s.Create(customer))
				.Throws<EmailTakenException>();

			var controller = fixture.CreateController();

			// When
			var ex = Assert.Throws<ConflictWithExistingException>(() => controller.Create(request));

			//Then
			Assert.Equal("email is already taken", ex.ConflictMessage);
			Assert.Equal(nameof(CustomerCreateRequest.Email), ex.IncomingPropertyName);
			Assert.Equal(request.Email, ex.IncomingPropertyValue);

			fixture.MockMapper.Verify(m => m.Map<Customer>(request), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Create(customer), Times.Once);
		}

		[Fact]
		public void ShouldCreate()
		{
			// Given
			var request = MockCustomerCreateRequest();
			var customer = Map<Customer>(request);

			var fixture = new CustomersControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);
			fixture.MockCustomerService.Setup(s => s.Create(customer));

			var controller = fixture.CreateController();

			// When
			var result = controller.Create(request);

			//Then
			var okResult = Assert.IsType<OkResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			fixture.MockMapper.Verify(m => m.Map<Customer>(request), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Create(customer), Times.Once);
		}

		#endregion

		#region Edit

		[Fact]
		public void ShouldThrowOnEditWhenProvidedBadRouteArguments()
		{
			// Given
			var customerId = 0;

			var request = MockCustomerEditRequest();

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var ex = Assert.Throws<RouteArgumentException>(() =>
				controller.Edit(customerId, request));

			// Then
			Assert.Equal("customerId", ex.ParamName);
			Assert.Equal("ID cannot be less than 1", ex.Message);
		}

		[Fact]
		public void ShouldThrowOnEditWhenProvidedInvalidBody()
		{
			var customerId = 1;

			var (request, details) = new CustomerEditRequestValidatorFixture()
				.MockInvalidWithDetails();

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var errors = Assert.Throws<InvalidBodyException>(() =>
				controller.Edit(customerId, request)).Errors;

			//Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		[Fact]
		public void ShouldThrowOnEditWhenEmailTaken()
		{
			// Given
			var customerId = 1;

			var request = MockCustomerEditRequest();
			var customer = Map<Customer>(request);

			var fixture = new CustomersControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);
			fixture.MockCustomerService.Setup(s => s.Edit(customer))
				.Throws<EmailTakenException>();

			var controller = fixture.CreateController();

			// When
			var ex = Assert.Throws<ConflictWithExistingException>(() =>
				controller.Edit(customerId, request));

			//Then
			Assert.Equal("email is already taken", ex.ConflictMessage);
			Assert.Equal(nameof(CustomerEditRequest.Email), ex.IncomingPropertyName);
			Assert.Equal(request.Email, ex.IncomingPropertyValue);

			fixture.MockMapper.Verify(m => m.Map<Customer>(request), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Edit(customer), Times.Once);
		}

		[Fact]
		public void ShouldEdit()
		{
			// Given
			var customerId = 1;

			var request = MockCustomerEditRequest();
			var customer = Map<Customer>(request);

			var fixture = new CustomersControllerFixture();
			fixture.MockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);
			fixture.MockCustomerService.Setup(s => s.Edit(customer));

			var controller = fixture.CreateController();

			// When
			var result = controller.Edit(customerId, request);

			//Then
			var okResult = Assert.IsType<OkResult>(result);
			Assert.Equal(200, okResult.StatusCode);

			fixture.MockMapper.Verify(m => m.Map<Customer>(request), Times.Once);
			fixture.MockCustomerService.Verify(s => s.Edit(customer), Times.Once);
		}

		#endregion

		#region Delete

		[Fact]
		public void ShouldThrowOnDeleteWhenProvidedBadRouteArguments()
		{
			// Given
			var customerId = 0;

			var controller = new CustomersControllerFixture().CreateController();

			// When
			var ex = Assert.Throws<RouteArgumentException>(() => controller.Delete(customerId));

			// Then
			Assert.Equal("customerId", ex.ParamName);
			Assert.Equal("ID cannot be less than 1", ex.Message);
		}

		[Fact]
		public void ShouldDelete()
		{
			// Given
			var customerId = 1;

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

		#region Fixture

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

		private static IMapper _mapper;

		private static TTo Map<TTo>(object source)
		{
			if (_mapper is null)
			{
				var config = new MapperConfiguration(cfg =>
				{
					cfg.AddProfile(new AutoMapperApiProfile());
				});

				_mapper = config.CreateMapper();
			}

			return _mapper.Map<TTo>(source);
		}

		#endregion

		#region Mocks - Domain models

		public static PagedResult<Customer> MockCustomerPagedResult() => new()
		{
			Page = 2,
			PageSize = 5,
			LastPage = 3,
			Items = new[] { MockCustomer() }
		};

		public static Customer MockCustomer() => new()
		{
			CustomerId = 1,
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@b.c",
			TotalPurchasesAmount = 666,
			Addresses = new[] { MockAddress() },
			Notes = new[] { MockNote() }
		};

		public static Address MockAddress() => new()
		{
			AddressId = 2,
			CustomerId = 1,
			Line = "Line1",
			Line2 = "Line21",
			Type = AddressType.Shipping,
			City = "City1",
			PostalCode = "123456",
			State = "State1",
			Country = "United States"
		};

		public static Note MockNote() => new()
		{
			NoteId = 3,
			CustomerId = 1,
			Content = "Content1"
		};

		#endregion

		#region Mocks - Api Dtos

		public static CustomerCreateRequest MockCustomerCreateRequest() => new()
		{
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@b.c",
			TotalPurchasesAmount = "666",
			Addresses = new[] { MockAddressRequest() },
			Notes = new[] { MockNoteRequest() },
		};

		public static CustomerEditRequest MockCustomerEditRequest() => new()
		{
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@b.c",
			TotalPurchasesAmount = "666"
		};

		public static AddressRequest MockAddressRequest() => new()
		{
			Line = "Line1",
			Line2 = "Line21",
			Type = "Shipping",
			City = "City1",
			PostalCode = "123456",
			State = "State1",
			Country = "United States"
		};

		public static NoteRequest MockNoteRequest() => new()
		{
			Content = "Content1"
		};

		#endregion

		#region Mocks tests

		[Fact]
		public void ShouldMockMeaningfulData()
		{
			// Domain
			var assertDomainModels = new AssertDomainModels();
			assertDomainModels.MeaningfulWithIdsAndNested(MockCustomer());
			assertDomainModels.MeaningfulWithIds(MockAddress());
			assertDomainModels.MeaningfulWithIds(MockNote());

			// PagedResult (domain Customer)
			MeaningfulPagedResultCustomer(MockCustomerPagedResult());

			// Api Dtos
			var assertApiAddressDtos = new AssertApiAddressDtos();
			assertApiAddressDtos.Meaningful(MockAddressRequest());

			var assertApiNoteDtos = new AssertApiNoteDtos();
			assertApiNoteDtos.Meaningful(MockNoteRequest());

			var assertApiCustomerDtos = new AssertApiCustomerDtos();
			assertApiCustomerDtos.Meaningful(MockCustomerCreateRequest());
			assertApiCustomerDtos.Meaningful(MockCustomerEditRequest());
		}



		#endregion
	}
}
