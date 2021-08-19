using System;
using System.Linq;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using CustomerLibCore.Data.Repositories.EF;
using CustomerLibCore.Data.Tests.Entities.Validators;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;
using static CustomerLibCore.TestHelpers.ValidatorTestData.Note;

namespace CustomerLibCore.Data.IntegrationTests.Repositories.EF
{
	[Collection(nameof(NotDbSafeResourceCollection))]
	public class CustomerRepositoryTest
	{
		#region Constructor

		[Fact]
		public void ShouldCreateCustomerRepository()
		{
			var context = DbContextHelper.Context;

			var repo = new CustomerRepository(context);

			Assert.NotNull(repo);
		}

		#endregion

		#region Exists

		[Theory]
		[InlineData(1, true)]
		[InlineData(2, true)]
		[InlineData(3, false)]
		public void ShouldCheckIfCustomerExistsById(int customerId, bool expectedExists)
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			CustomerRepositoryFixture.CreateMockCustomer(amount: 2);

			// When
			var exists = repo.Exists(customerId);

			// Then
			Assert.Equal(expectedExists, exists);
		}

		#endregion

		#region Create 

		[Fact]
		public void ShouldThrowOnCreateWhenProvidedInvalidObject()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			var (customer, details) = new CustomerEntityValidatorFixture().MockInvalidWithDetails();

			// When
			var errors = Assert.Throws<InternalValidationException>(() =>
				repo.Create(customer)).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		public class MockCustomerData : TheoryData<CustomerEntity>
		{
			public MockCustomerData()
			{
				Add(CustomerRepositoryFixture.MockCustomer());
				Add(CustomerRepositoryFixture.MockOptionalCustomer());
			}
		}

		[Theory]
		[ClassData(typeof(MockCustomerData))]
		public void ShouldCreateCustomerIncludingOptionalProperties(CustomerEntity customer)
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var createdId1 = repo.Create(customer);
			customer.CustomerId = 0;
			var createdId2 = repo.Create(customer);

			// Then
			Assert.Equal(1, createdId1);
			Assert.Equal(2, createdId2);
		}

		#endregion

		#region Read by Id

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void ShouldReadCustomerNullWhenEmptyRepo(int customerId)
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var readCustomer = repo.Read(customerId);

			// Then
			Assert.Null(readCustomer);
		}

		[Theory]
		[InlineData(1, false)]
		[InlineData(2, false)]
		[InlineData(3, true)]
		[InlineData(4, true)]
		public void ShouldReadCustomerNullWhenNotFound(int customerId, bool isNull)
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(amount: 2);

			// When
			var readCustomer = repo.Read(customerId);

			// Then
			Assert.Equal(readCustomer is null, isNull);
		}

		public class CreateMockCustomerData : TheoryData<Action, CustomerEntity>
		{
			public CreateMockCustomerData()
			{
				Add(() => CustomerRepositoryFixture.CreateMockCustomer(),
					CustomerRepositoryFixture.MockCustomer());
				Add(() => CustomerRepositoryFixture.CreateMockOptionalCustomer(),
					CustomerRepositoryFixture.MockOptionalCustomer());
			}
		}

		[Theory]
		[ClassData(typeof(CreateMockCustomerData))]
		public void ShouldReadCustomerIncludingNullOptionalFields(
			Action createCustomer, CustomerEntity customer)
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			createCustomer();

			// When
			var readCustomer = repo.Read(1);

			// Then
			Assert.Equal(1, readCustomer.CustomerId);
			Assert.True(readCustomer.EqualsByValueExcludingId(customer));
		}

		#endregion

		#region Read many

		[Fact]
		public void ShouldReadManyCustomersEmptyWhenEmptyRepo()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var readCustomers = repo.ReadMany();

			// Then
			Assert.Empty(readCustomers);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void ShouldReadManyCustomersIncludingSingle(int amount)
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(amount: amount);

			var customer = CustomerRepositoryFixture.MockCustomer();

			// When
			var readCustomers = repo.ReadMany();

			// Then
			Assert.Equal(amount, readCustomers.Count);

			foreach (var readCustomer in readCustomers)
			{
				Assert.True(readCustomer.EqualsByValueExcludingId(customer));
			}
		}

		#endregion

		#region Get count

		[Fact]
		public void ShouldGetTotalCustomersCountWhenEmpty()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var count = repo.GetCount();

			// Then
			Assert.Equal(0, count);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void ShouldGetTotalCustomersCount(int amount)
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(amount: amount);

			// When
			var count = repo.GetCount();

			// Then
			Assert.Equal(amount, count);
		}


		#endregion

		#region Read page

		[Fact]
		public void ShouldReadPageOfCustomersEmptyWhenEmptyRepo()
		{
			// Given
			var page = 1;
			var pageSize = 3;

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var pageCustomers = repo.ReadPage(page, pageSize);

			// Then
			Assert.Equal(0, repo.GetCount());
			Assert.Empty(pageCustomers);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void ShouldReadPageOfCustomersEmptyWhenNotFound(int amount)
		{
			// Given
			var page = 2;
			var pageSize = 3;

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(amount: amount);

			// When
			var pageCustomers = repo.ReadPage(page, pageSize);

			// Then
			Assert.Empty(pageCustomers);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void ShouldReadPageOfCustomersFromSinglePage(int amount)
		{
			// Given
			var page = 1;
			var pageSize = 3;

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(amount: amount);

			// When
			var pageCustomers = repo.ReadPage(page, pageSize);

			// Then
			Assert.Equal(amount, repo.GetCount());
			Assert.Equal(amount, pageCustomers.Count);
		}

		[Theory]
		[InlineData(4, 1, new[] { 4 })]
		[InlineData(5, 2, new[] { 4, 5 })]
		[InlineData(6, 3, new[] { 4, 5, 6 })]
		[InlineData(7, 3, new[] { 4, 5, 6 })]
		public void ShouldReadPageOfCustomersFromMultiplePages(
			int amount, int secondPageItemsAmount, int[] secondPageItemsIds)
		{
			// Given
			var page = 2;
			var pageSize = 3;

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(amount: amount);

			// When
			var pageCustomers = repo.ReadPage(page, pageSize);

			// Then
			Assert.Equal(amount, repo.GetCount());
			Assert.Equal(secondPageItemsAmount, pageCustomers.Count);

			var actualIds = pageCustomers.Select(x => x.CustomerId);

			Assert.Equal(secondPageItemsIds, actualIds);
		}

		#endregion

		#region Update

		[Fact]
		public void ShouldThrowOnUpdateWhenProvidedInvalidObject()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			var (customer, details) = new CustomerEntityValidatorFixture().MockInvalidWithDetails();

			// When
			var errors = Assert.Throws<InternalValidationException>(() =>
				repo.Update(customer)).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(123)]
		public void ShouldNotUpdateExistingCustomersWhenNotFound(int customerId)
		{
			// Given
			var customerId1 = 1;
			var customerId2 = 2;

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			var firstName1 = "FirstName1";
			var firstName2 = "FirstName2";
			var newFirstName = "NewFirstName";

			var customer1 = CustomerRepositoryFixture.MockCustomer();
			customer1.FirstName = firstName1;

			var customer2 = CustomerRepositoryFixture.MockCustomer();
			customer2.FirstName = firstName1;

			repo.Create(customer1);
			repo.Create(customer2);

			var createdCustomer1 = repo.Read(customerId1);
			var createdCustomer2 = repo.Read(customerId2);

			Assert.Equal(firstName1, createdCustomer1.FirstName);
			Assert.Equal(firstName2, createdCustomer2.FirstName);

			Assert.NotEqual(firstName1, firstName2);
			Assert.NotEqual(newFirstName, firstName1);
			Assert.NotEqual(newFirstName, firstName2);

			var nonExistingCustomer = CustomerRepositoryFixture.MockCustomer();
			nonExistingCustomer.CustomerId = customerId;
			nonExistingCustomer.FirstName = newFirstName;

			Assert.False(repo.Exists(customerId));

			// When
			repo.Update(nonExistingCustomer);

			// Then
			Assert.False(repo.Exists(customerId));

			var untouchedCustomer1 = repo.Read(customerId1);
			var untouchedCustomer2 = repo.Read(customerId2);

			Assert.NotEqual(untouchedCustomer1.FirstName, untouchedCustomer2.FirstName);
			Assert.NotEqual(newFirstName, untouchedCustomer1.FirstName);
			Assert.NotEqual(newFirstName, untouchedCustomer2.FirstName);

			Assert.True(untouchedCustomer1.EqualsByValue(createdCustomer1));
			Assert.True(untouchedCustomer2.EqualsByValue(createdCustomer2));
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(2, 1)]
		public void ShouldUpdateCustomer(int customerId, int untouchedCustomerId)
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			var firstName1 = "FirstName1";
			var firstName2 = "FirstName2";
			var newFirstName = "NewFirstName";

			var customer1 = CustomerRepositoryFixture.MockCustomer();
			customer1.FirstName = firstName1;

			var customer2 = CustomerRepositoryFixture.MockCustomer();
			customer2.FirstName = firstName1;

			repo.Create(customer1);
			repo.Create(customer2);

			var createdCustomerToBeUpdated = repo.Read(customerId);
			var createdCustomerToStayUntouched = repo.Read(untouchedCustomerId);

			Assert.Equal(firstName1, createdCustomerToBeUpdated.FirstName);
			Assert.Equal(firstName2, createdCustomerToStayUntouched.FirstName);

			Assert.NotEqual(firstName1, firstName2);
			Assert.NotEqual(newFirstName, firstName1);
			Assert.NotEqual(newFirstName, firstName2);

			var customer = CustomerRepositoryFixture.MockCustomer();
			customer.CustomerId = customerId;
			customer.FirstName = newFirstName;

			// When
			repo.Update(customer);

			// Then
			var updatedCustomer = repo.Read(customerId);

			Assert.Equal(newFirstName, updatedCustomer.FirstName);
			Assert.Equal(customer.LastName, updatedCustomer.LastName);
			Assert.Equal(customer.PhoneNumber, updatedCustomer.PhoneNumber);
			Assert.Equal(customer.Email, updatedCustomer.Email);
			Assert.Equal(customer.TotalPurchasesAmount, updatedCustomer.TotalPurchasesAmount);

			var untouchedCustomer = repo.Read(untouchedCustomerId);

			Assert.NotEqual(newFirstName, untouchedCustomer.FirstName);
			Assert.True(untouchedCustomer.EqualsByValue(createdCustomerToStayUntouched));
		}

		#endregion

		#region Delete

		[Fact]
		public void ShouldNotDeleteWhenNotFound()
		{
			// Given
			var customerId = 2;

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer();

			Assert.Equal(1, repo.GetCount());

			// When
			repo.Delete(customerId);

			// Then
			Assert.Equal(1, repo.GetCount());
		}

		[Fact]
		public void ShouldDeleteSingleCustomer()
		{
			// Given
			var customerId = 1;

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer();

			Assert.Equal(1, repo.GetCount());

			// When
			repo.Delete(customerId);

			// Then
			Assert.Equal(0, repo.GetCount());
		}

		[Fact]
		public void ShouldDeleteCustomerWhenManyExist()
		{
			// Given
			var deletedCustomerId = 1;
			var untouchedCustomerId = 2;
			var amount = 2;

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(amount: amount);

			Assert.Equal(2, repo.GetCount());

			// When
			repo.Delete(deletedCustomerId);

			// Then
			Assert.NotNull(repo.Read(untouchedCustomerId));
			Assert.Equal(1, repo.GetCount());
		}

		#endregion

		#region Delete all

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void ShouldDeleteAllCustomers(int amount)
		{
			// Given

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(amount: amount);

			Assert.Equal(amount, repo.GetCount());

			// When
			repo.DeleteAll();

			// Then
			Assert.Equal(0, repo.GetCount());
		}

		#endregion

		#region Email taken checks

		class EmailTakenData : TheoryData<string, bool>
		{
			public EmailTakenData()
			{
				Add("taken@asd.com", true);
				Add("free@asd.com", false);
			}
		}

		[Theory]
		[ClassData(typeof(EmailTakenData))]
		public void ShouldCheckForEmailTaken(string email, bool isTakenExpected)
		{
			// Given
			var takenEmail = "taken@asd.com";
			var anotherCustomerEmail = "second@asd.com";
			Assert.NotEqual(anotherCustomerEmail, email);

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(takenEmail);
			CustomerRepositoryFixture.CreateMockCustomer(anotherCustomerEmail);

			// When
			var isTaken = repo.IsEmailTaken(email);

			// Then
			Assert.Equal(isTakenExpected, isTaken);
		}

		[Theory]
		[ClassData(typeof(EmailTakenData))]
		public void ShouldCheckForEmailTakenWithCustomerId(string email, bool isTakenExpected)
		{
			// Given
			var takenEmail = "taken@asd.com";
			var anotherCustomerEmail = "second@asd.com";
			Assert.NotEqual(anotherCustomerEmail, email);

			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			CustomerRepositoryFixture.CreateMockCustomer(anotherCustomerEmail);
			CustomerRepositoryFixture.CreateMockCustomer(takenEmail);

			// When
			var (isTaken, takenById) = repo.IsEmailTakenWithCustomerId(email);

			// Then
			Assert.Equal(isTakenExpected, isTaken);
			Assert.Equal(isTaken ? 2 : 0, takenById);
		}

		#endregion

		public class CustomerRepositoryFixture
		{
			private static readonly CustomerEntityValidatorFixture _validatorFixture = new();

			/// <summary>
			/// Clears the database, then creates the customer repository.
			/// </summary>
			/// <returns>The customer repository.</returns>
			public static CustomerRepository CreateEmptyRepository()
			{
				DatabaseHelper.Clear();
				return new(DbContextHelper.Context);
			}

			/// <summary>
			/// Creates the specified amount of mocked customers with valid properties, 
			/// optional properties not <see langword="null"/>.
			/// </summary>
			/// <param name="email">The customer email.</param>
			/// <param name="amount">The amount of customers to create.</param>
			public static void CreateMockCustomer(
				string email = "a@b.c", int amount = 1)
			{
				var repo = new CustomerRepository(DbContextHelper.Context);

				var customer = MockCustomer(email);

				for (int i = 0; i < amount; i++)
				{
					customer.CustomerId = 0;
					repo.Create(customer);
				}
			}

			/// <summary>
			/// Creates the mocked customer with valid properties, 
			/// optional properties <see langword="null"/>.
			/// </summary>
			public static void CreateMockOptionalCustomer() =>
				new CustomerRepository(DbContextHelper.Context)
					.Create(MockOptionalCustomer());

			/// <returns>The mocked customer with valid properties,
			/// optional properties not <see langword="null"/>.</returns>
			public static CustomerEntity MockCustomer(string email = "a@b.c")
			{
				var customer = _validatorFixture.MockValid();
				customer.Email = email;

				return customer;
			}

			/// <returns>The mocked customer with valid properties,
			/// optional properties <see langword="null"/>.</returns>
			public static CustomerEntity MockOptionalCustomer() =>
				_validatorFixture.MockValidOptional();
		}

		public class CustomerRepositoryFixtureTest
		{
			[Fact]
			public void ShouldValidateCustomerWithOptionalPropertiesNotNull()
			{
				// Given
				var customer = CustomerRepositoryFixture.MockCustomer();

				Assert.NotNull(customer.FirstName);
				Assert.NotNull(customer.Email);
				Assert.NotNull(customer.PhoneNumber);
				Assert.NotNull(customer.TotalPurchasesAmount);

				// When
				var result = new CustomerEntityValidator().Validate(customer);

				// Then
				Assert.True(result.IsValid);
			}

			[Fact]
			public void ShouldValidateCustomerWithOptionalPropertiesNull()
			{
				// Given
				var customer = CustomerRepositoryFixture.MockOptionalCustomer();

				Assert.Null(customer.FirstName);
				Assert.Null(customer.Email);
				Assert.Null(customer.PhoneNumber);
				Assert.Null(customer.TotalPurchasesAmount);

				// When
				var result = new CustomerEntityValidator().Validate(customer);

				// Then
				Assert.True(result.IsValid);
			}
		}
	}
}
