//using System;
//using System.Collections.Generic;
//using CustomerLibCore.Business.Entities;
//using CustomerLibCore.Business.Exceptions;
//using CustomerLibCore.Business.Validators;
//using CustomerLibCore.Data.Repositories;
//using CustomerLibCore.ServiceLayer.Services;
//using CustomerLibCore.ServiceLayer.Services.Implementations;
//using CustomerLibCore.TestHelpers;
//using Moq;
//using Xunit;
//using static CustomerLibCore.TestHelpers.FluentValidation.ValidationFailureExtensions;

//namespace CustomerLibCore.ServiceLayer.Tests.Services
//{
//	public class CustomerServiceTest
//	{
//		#region Constructors

//		[Fact]
//		public void ShouldCreateCustomerServiceDefault()
//		{
//			var customerService = new CustomerService();

//			Assert.NotNull(customerService);
//		}

//		[Fact]
//		public void ShouldCreateCustomerService()
//		{
//			var mockCustomerRepository = new StrictMock<ICustomerRepository>();

//			var mockAddressRepository = new StrictMock<IAddressRepository>();
//			var mockNoteRepository = new StrictMock<INoteRepository>();

//			var customerService = new CustomerService(mockCustomerRepository.Object,
//				mockAddressRepository.Object, mockNoteRepository.Object);

//			Assert.NotNull(customerService);
//		}

//		#endregion

//		#region Save

//		[Fact]
//		public void ShouldThrowOnSaveWhenProvidedInvalidCustomerIncludingAddressesAndNotes()
//		{
//			// Given
//			var invalidCustomer = new Customer();

//			var expectedErrors = new CustomerValidator().ValidateFull(invalidCustomer).Errors;

//			var service = new CustomerServiceFixture().CreateService();

//			// When
//			var actualErrors = Assert.Throws<InternalValidationException>(() =>
//				service.Save(invalidCustomer)).Errors;

//			// Then
//			AssertValidationFailuresEqualByPropertyNameAndErrorMessage(
//				expectedErrors, actualErrors, 3);
//			AssertContainPropertyNames(actualErrors, new[] {
//				nameof(Customer.LastName),
//				nameof(Customer.Addresses),
//				nameof(Customer.Notes) });
//		}

//		[Fact]
//		public void ShouldThrowOnSaveWhenEmailTaken()
//		{
//			// Given
//			var customer = CustomerServiceFixture.MockCustomer();
//			var email = customer.Email;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.IsEmailTaken(email)).Returns(true);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<EmailTakenException>(() => service.Save(customer));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.IsEmailTaken(email), Times.Once);
//		}

//		[Fact]
//		public void ShouldSave()
//		{
//			// Given
//			var createdCustomerId = 5;
//			var customer = CustomerServiceFixture.MockCustomer();

//			var email = customer.Email;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.IsEmailTaken(email)).Returns(false);
//			fixture.MockCustomerRepository.Setup(r => r.Create(customer))
//				.Returns(createdCustomerId);

//			var service = fixture.CreateService();

//			// When
//			service.Save(customer);

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.IsEmailTaken(email), Times.Once);
//			fixture.MockCustomerRepository.Verify(r => r.Create(customer), Times.Once);
//		}

//		#endregion

//		#region Get single

//		[Fact]
//		public void ShouldThrowOnGetWhenProvidedBadId()
//		{
//			// Given
//			var badCustomerId = 0;
//			bool includeAddresses = default;
//			bool includeNotes = default;

//			var service = new CustomerServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.Get(badCustomerId, includeAddresses, includeNotes));

//			// Then
//			Assert.Equal("customerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnGetWhenCustomerNotFound()
//		{
//			// Given
//			var customerId = 5;
//			bool includeAddresses = default;
//			bool includeNotes = default;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Read(customerId)).Returns(() => null);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() =>
//				service.Get(customerId, includeAddresses, includeNotes));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Read(customerId), Times.Once);
//		}

//		public class AddressesAndNotesData : TheoryData<List<Address>, List<Note>>
//		{
//			public AddressesAndNotesData()
//			{
//				Add(new() { new() }, new() { new() });
//				Add(new(), new());
//			}
//		}

//		[Theory]
//		[ClassData(typeof(AddressesAndNotesData))]
//		public void ShouldGetWithAddressesAndNotes(
//			List<Address> addresses, List<Note> notes)
//		{
//			// Given
//			var customerId = 5;
//			var repoCustomer = CustomerServiceFixture.MockCustomer();
//			repoCustomer.CustomerId = customerId;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Read(customerId)).Returns(repoCustomer);

//			fixture.MockAddressRepository.Setup(s => s.ReadManyForCustomer(customerId))
//				.Returns(addresses);
//			fixture.MockNoteRepository.Setup(s => s.ReadManyForCustomer(customerId)).Returns(notes);


//			var expectedCustomer = CustomerServiceFixture.MockCustomer();
//			expectedCustomer.CustomerId = customerId;
//			expectedCustomer.Addresses = addresses;
//			expectedCustomer.Notes = notes;

//			var service = fixture.CreateService();

//			// When
//			var actualCustomer = service.Get(customerId, true, true);

//			// Then
//			Assert.True(expectedCustomer.EqualsByValue(actualCustomer));

//			fixture.MockCustomerRepository.Verify(r => r.Read(customerId), Times.Once);
//			fixture.MockAddressRepository.Verify(r => r.ReadManyForCustomer(customerId), Times.Once);
//			fixture.MockNoteRepository.Verify(r => r.ReadManyForCustomer(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldGetWithoutAddressesAndNotes()
//		{
//			// Given
//			var customerId = 5;
//			var customer = CustomerServiceFixture.MockRepoCustomer();
//			customer.CustomerId = customerId;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Read(customerId)).Returns(customer);

//			var service = fixture.CreateService();

//			// When
//			var actualCustomer = service.Get(customerId, false, false);

//			// Then
//			Assert.Equal(customer, actualCustomer);

//			fixture.MockCustomerRepository.Verify(r => r.Read(customerId), Times.Once);
//		}

//		#endregion

//		//#region Find all

//		//[Fact]
//		//public void ShouldFildAllWhenEmpty()
//		//{
//		//	// Given
//		//	var expectedCustomers = new List<Customer>();

//		//	bool includeAddresses = default;
//		//	bool includeNotes = default;

//		//	var fixture = new CustomerServiceFixture();
//		//	fixture.MockCustomerRepository.Setup(r => r.ReadMany()).Returns(expectedCustomers);

//		//	var service = fixture.CreateService();

//		//	// When
//		//	var customers = service.FindAll(includeAddresses, includeNotes);

//		//	// Then
//		//	Assert.Equal(expectedCustomers, customers);
//		//	fixture.MockCustomerRepository.Verify(r => r.ReadMany(), Times.Once);
//		//}

//		//[Theory]
//		//[ClassData(typeof(AddressesAndNotesData))]
//		//public void ShouldFindAllsWithAddressesAndNotes(
//		//	List<Address> addresses, List<Note> notes)
//		//{
//		//	// Given
//		//	var customerId1 = 5;
//		//	var customerId2 = 7;

//		//	var repoCustomers = new List<Customer>()
//		//	{
//		//		new() { CustomerId = customerId1 },
//		//		new() { CustomerId = customerId2 }
//		//	};

//		//	var fixture = new CustomerServiceFixture();
//		//	fixture.MockCustomerRepository.Setup(r => r.ReadMany()).Returns(repoCustomers);

//		//	var addresses1 = addresses;
//		//	var addresses2 = new List<Address>(addresses);
//		//	Assert.NotSame(addresses1, addresses2);

//		//	fixture.MockAddressRepository.Setup(r => r.ReadManyForCustomer(customerId1))
//		//		.Returns(addresses1);
//		//	fixture.MockAddressRepository.Setup(r => r.ReadManyForCustomer(customerId2))
//		//		.Returns(addresses2);

//		//	var notes1 = notes;
//		//	var notes2 = new List<Note>(notes);
//		//	Assert.NotSame(notes1, notes2);

//		//	fixture.MockNoteRepository.Setup(r => r.ReadManyForCustomer(customerId1))
//		//		.Returns(notes1);
//		//	fixture.MockNoteRepository.Setup(r => r.ReadManyForCustomer(customerId2))
//		//		.Returns(notes2);

//		//	var expectedCustomers = new List<Customer>(repoCustomers);

//		//	expectedCustomers[0].Addresses = addresses1;
//		//	expectedCustomers[1].Addresses = addresses2;

//		//	expectedCustomers[0].Notes = notes1;
//		//	expectedCustomers[1].Notes = notes2;

//		//	var service = fixture.CreateService();

//		//	// When
//		//	var actualCustomers = service.FindAll(true, true);

//		//	// Then
//		//	Assert.Equal(expectedCustomers, actualCustomers);

//		//	fixture.MockCustomerRepository.Verify(r => r.ReadMany(), Times.Once);

//		//	fixture.MockAddressRepository.Verify(r => r.ReadManyForCustomer(customerId1), Times.Once);
//		//	fixture.MockAddressRepository.Verify(r => r.ReadManyForCustomer(customerId2), Times.Once);

//		//	fixture.MockNoteRepository.Verify(r => r.ReadManyForCustomer(customerId1), Times.Once);
//		//	fixture.MockNoteRepository.Verify(r => r.ReadManyForCustomer(customerId2), Times.Once);
//		//}

//		//[Fact]
//		//public void ShouldFindAllWithoutAddressesAndNotes()
//		//{
//		//	// Given
//		//	var repoCustomers = new List<Customer>() { new() };

//		//	var fixture = new CustomerServiceFixture();
//		//	fixture.MockCustomerRepository.Setup(r => r.ReadMany()).Returns(repoCustomers);

//		//	var service = fixture.CreateService();

//		//	// When
//		//	var customers = service.FindAll(false, false);

//		//	// Then
//		//	Assert.Equal(repoCustomers, customers);
//		//	fixture.MockCustomerRepository.Verify(r => r.ReadMany(), Times.Once);
//		//}

//		//#endregion

//		#region Get count

//		[Theory]
//		[InlineData(0)]
//		[InlineData(5)]
//		public void ShouldGetTotalCustomersCount(int expectedCount)
//		{
//			// Given
//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.GetCount()).Returns(expectedCount);

//			var service = fixture.CreateService();

//			// When
//			var count = service.GetCount();

//			// Then
//			Assert.Equal(expectedCount, count);
//			fixture.MockCustomerRepository.Verify(r => r.GetCount(), Times.Once);
//		}

//		#endregion

//		#region Get page

//		[Theory]
//		[InlineData(0, 1, "page")]
//		[InlineData(1, 0, "pageSize")]
//		public void ShouldThrowOnGetPageWhenProvidedBadArguments(
//		int page, int pageSize, string paramName)
//		{
//			var service = new CustomerServiceFixture().CreateService();

//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.GetPage(page, pageSize, false, false));

//			Assert.Equal(paramName, exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnGetPageWhenRequestedPageIsInvalidForItems()
//		{
//			// Given
//			var page = 2;
//			var pageSize = 10;

//			bool includeAddresses = default;
//			bool includeNotes = default;

//			var expectedCustomers = new List<Customer>();

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.ReadPage(page, pageSize))
//				.Returns(expectedCustomers);

//			var service = fixture.CreateService();

//			// When
//			var exception = Assert.Throws<PagedRequestInvalidException>(() =>
//				service.GetPage(page, pageSize, includeAddresses, includeNotes));

//			// Then
//			Assert.Equal(page, exception.Page);
//			Assert.Equal(pageSize, exception.PageSize);

//			fixture.MockCustomerRepository.Verify(r => r.ReadPage(page, pageSize), Times.Once);
//		}

//		[Fact]
//		public void ShouldGetPageEmptyWhenNotFoundForFirstPage()
//		{
//			// Given
//			var page = 1;
//			var pageSize = 10;

//			bool includeAddresses = default;
//			bool includeNotes = default;

//			var expectedCustomers = new List<Customer>();

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.ReadPage(page, pageSize))
//				.Returns(expectedCustomers);

//			var service = fixture.CreateService();

//			// When
//			var result = service.GetPage(page, pageSize, includeAddresses, includeNotes);

//			// Then
//			Assert.Equal(expectedCustomers, result.Items);
//			Assert.Equal(page, result.Page);
//			Assert.Equal(pageSize, result.PageSize);
//			Assert.Equal(0, result.LastPage);

//			fixture.MockCustomerRepository.Verify(r => r.ReadPage(page, pageSize), Times.Once);
//		}

//		[Fact]
//		public void ShouldGetPageNotEmptyWithoutAddressesAndNotes()
//		{
//			// Given
//			var page = 5;
//			var pageSize = 7;
//			var totalCount = 30;

//			var expectedCustomers = new List<Customer>() { new(), new() };

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.ReadPage(page, pageSize))
//				.Returns(expectedCustomers);
//			fixture.MockCustomerRepository.Setup(r => r.GetCount()).Returns(totalCount);

//			var service = fixture.CreateService();

//			// When
//			var result = service.GetPage(page, pageSize, false, false);

//			// Then
//			Assert.Equal(expectedCustomers, result.Items);
//			Assert.Equal(page, result.Page);
//			Assert.Equal(pageSize, result.PageSize);
//			Assert.Equal(totalCount, result.LastPage);

//			fixture.MockCustomerRepository.Verify(r => r.ReadPage(page, pageSize), Times.Once);
//			fixture.MockCustomerRepository.Verify(r => r.GetCount(), Times.Once);
//		}

//		[Theory]
//		[ClassData(typeof(AddressesAndNotesData))]
//		public void ShouldGetPageNotEmptyWithAddressesAndNotes
//			(List<Address> addresses, List<Note> notes)
//		{
//			// Given
//			var page = 5;
//			var pageSize = 7;
//			var totalCount = 30;

//			var customerId1 = 5;
//			var customerId2 = 7;

//			var repoCustomers = new List<Customer>()
//			{
//				new() { CustomerId = customerId1 },
//				new() { CustomerId = customerId2 }
//			};

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.ReadPage(page, pageSize))
//				.Returns(repoCustomers);
//			fixture.MockCustomerRepository.Setup(r => r.GetCount()).Returns(totalCount);

//			var addresses1 = addresses;
//			var addresses2 = new List<Address>(addresses);
//			Assert.NotSame(addresses1, addresses2);

//			fixture.MockAddressRepository.Setup(r => r.ReadManyForCustomer(customerId1))
//				.Returns(addresses1);
//			fixture.MockAddressRepository.Setup(r => r.ReadManyForCustomer(customerId2))
//				.Returns(addresses2);

//			var notes1 = notes;
//			var notes2 = new List<Note>(notes);
//			Assert.NotSame(notes1, notes2);

//			fixture.MockNoteRepository.Setup(r => r.ReadManyForCustomer(customerId1))
//				.Returns(notes1);
//			fixture.MockNoteRepository.Setup(r => r.ReadManyForCustomer(customerId2))
//				.Returns(notes2);

//			var expectedCustomers = new List<Customer>(repoCustomers);

//			expectedCustomers[0].Addresses = addresses1;
//			expectedCustomers[1].Addresses = addresses2;

//			expectedCustomers[0].Notes = notes1;
//			expectedCustomers[1].Notes = notes2;

//			var service = fixture.CreateService();

//			// When
//			var result = service.GetPage(page, pageSize, true, true);

//			// Then
//			Assert.Equal(expectedCustomers, result.Items);
//			Assert.Equal(page, result.Page);
//			Assert.Equal(pageSize, result.PageSize);
//			Assert.Equal(totalCount, result.LastPage);

//			fixture.MockCustomerRepository.Verify(r => r.ReadPage(page, pageSize), Times.Once);
//			fixture.MockCustomerRepository.Verify(r => r.GetCount(), Times.Once);

//			fixture.MockAddressRepository.Verify(r => r.ReadManyForCustomer(customerId1), Times.Once);
//			fixture.MockAddressRepository.Verify(r => r.ReadManyForCustomer(customerId2), Times.Once);

//			fixture.MockNoteRepository.Verify(r => r.ReadManyForCustomer(customerId1), Times.Once);
//			fixture.MockNoteRepository.Verify(r => r.ReadManyForCustomer(customerId2), Times.Once);
//		}

//		#endregion

//		#region Update

//		[Fact]
//		public void ShouldThrowOnUpdateWhenProvidedBadId()
//		{
//			// Given
//			var badCustomerId = 0;

//			var service = new CustomerServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() =>
//				service.Update(new() { CustomerId = badCustomerId }));

//			// Then
//			Assert.Equal("CustomerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnUpdateWhenProvidedInvalidCustomerExcludingAddressesAndNotes()
//		{
//			// Given
//			var customerId = 5;
//			var invalidCustomer = new Customer() { CustomerId = customerId };

//			var expectedErrors = new CustomerValidator()
//				.ValidateWithoutAddressesAndNotes(invalidCustomer).Errors;

//			var service = new CustomerServiceFixture().CreateService();

//			// When
//			var actualErrors = Assert.Throws<InternalValidationException>(() =>
//				service.Update(invalidCustomer)).Errors;

//			// Then
//			AssertValidationFailuresEqualByPropertyNameAndErrorMessage(
//				expectedErrors, actualErrors, 1);
//			AssertContainPropertyNames(actualErrors,
//				new[] { nameof(Customer.LastName) });

//		}

//		[Fact]
//		public void ShouldThrowOnUpdateWhenCustomerNotFound()
//		{
//			// Given
//			var customerId = 5;

//			var customer = CustomerServiceFixture.MockCustomer();
//			customer.CustomerId = customerId;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.Update(customer));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldThrowOnUpdateByEmailTakenByOtherCustomer()
//		{
//			// Given
//			var customerId = 5;
//			var takenById = 666;

//			var customer = CustomerServiceFixture.MockCustomer();
//			customer.CustomerId = customerId;
//			var email = customer.Email;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);
//			fixture.MockCustomerRepository.Setup(r => r.IsEmailTakenWithCustomerId(email))
//				.Returns((true, takenById));

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<EmailTakenException>(() => service.Update(customer));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockCustomerRepository.Verify(r => r.IsEmailTakenWithCustomerId(email),
//				Times.Once);
//		}

//		[Fact]
//		public void ShouldUpdateCustomer()
//		{
//			// Given
//			var customerId = 5;

//			var customer = CustomerServiceFixture.MockCustomer();
//			customer.CustomerId = customerId;
//			var email = customer.Email;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);
//			fixture.MockCustomerRepository.Setup(r => r.IsEmailTakenWithCustomerId(email))
//				.Returns((false, 0));
//			fixture.MockCustomerRepository.Setup(r => r.Update(customer));

//			var service = fixture.CreateService();

//			// When
//			service.Update(customer);

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockCustomerRepository.Verify(r => r.IsEmailTakenWithCustomerId(email),
//				Times.Once);
//			fixture.MockCustomerRepository.Verify(r => r.Update(customer), Times.Once);
//		}

//		#endregion

//		#region Delete

//		[Fact]
//		public void ShouldThrowOnDeleteWhenProvidedBadId()
//		{
//			// Given
//			var badCustomerId = 0;

//			var service = new CustomerServiceFixture().CreateService();

//			// When
//			var exception = Assert.Throws<ArgumentException>(() => service.Delete(badCustomerId));

//			// Then
//			Assert.Equal("customerId", exception.ParamName);
//		}

//		[Fact]
//		public void ShouldThrowOnDeleteWhenCustomerNotFound()
//		{
//			// Given
//			var customerId = 5;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(false);

//			var service = fixture.CreateService();

//			// When
//			Assert.Throws<NotFoundException>(() => service.Delete(customerId));

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//		}

//		[Fact]
//		public void ShouldDeleteIncludingAddressesAndNotes()
//		{
//			// Given
//			var customerId = 5;

//			var fixture = new CustomerServiceFixture();
//			fixture.MockCustomerRepository.Setup(r => r.Exists(customerId)).Returns(true);
//			fixture.MockCustomerRepository.Setup(r => r.Delete(customerId));

//			fixture.MockAddressRepository.Setup(r => r.DeleteManyForCustomer(customerId));
//			fixture.MockNoteRepository.Setup(r => r.DeleteManyForCustomer(customerId));

//			var service = fixture.CreateService();

//			// When
//			service.Delete(customerId);

//			// Then
//			fixture.MockCustomerRepository.Verify(r => r.Exists(customerId), Times.Once);
//			fixture.MockCustomerRepository.Verify(r => r.Delete(customerId), Times.Once);

//			fixture.MockAddressRepository.Verify(r => r.DeleteManyForCustomer(customerId),
//				Times.Once);
//			fixture.MockNoteRepository.Verify(r => r.DeleteManyForCustomer(customerId), Times.Once);
//		}

//		#endregion
//	}

//	public class CustomerServiceFixture
//	{
//		public StrictMock<ICustomerRepository> MockCustomerRepository { get; set; }
//		public StrictMock<IAddressRepository> MockAddressRepository { get; set; }
//		public StrictMock<INoteRepository> MockNoteRepository { get; set; }
//		public CustomerServiceFixture()
//		{
//			MockCustomerRepository = new();
//			MockAddressRepository = new();
//			MockNoteRepository = new();
//		}

//		public CustomerService CreateService() => new(MockCustomerRepository.Object,
//			MockAddressRepository.Object, MockNoteRepository.Object);

//		/// <returns>The mocked customer with repo-relevant properties
//		/// with valid addresses and notes. Optional properties not null.</returns>
//		public static Customer MockCustomer() => new()
//		{
//			FirstName = "a",
//			LastName = "a",
//			Addresses = new() { AddressServiceFixture.MockAddress() },
//			PhoneNumber = "+123",
//			Email = "a@a.aa",
//			Notes = new() { NoteServiceFixture.MockNote() },
//			TotalPurchasesAmount = 123,
//		};

//		/// <returns>The mocked customer with repo-relevant properties 
//		/// with null addresses and notes. Optional properties not null.</returns>
//		public static Customer MockRepoCustomer() => new()
//		{
//			FirstName = "a",
//			LastName = "a",
//			Addresses = null,
//			PhoneNumber = "+123",
//			Email = "a@a.aa",
//			Notes = null,
//			TotalPurchasesAmount = 123,
//		};
//	}
//}
