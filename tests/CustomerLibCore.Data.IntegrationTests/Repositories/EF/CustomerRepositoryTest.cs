using System;
using System.Linq;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Repositories.EF;
using Xunit;

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
		public void ShouldCreateCustomer()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();
			var customer = CustomerRepositoryFixture.MockCustomer();

			// When
			var createdId = repo.Create(customer);

			// Then
			Assert.Equal(1, createdId);
		}

		#endregion

		#region Read by Id

		[Fact]
		public void ShouldReadCustomerNotFound()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var readCustomer = repo.Read(1);

			// Then
			Assert.Null(readCustomer);
		}

		public class CreateMockCustomerData : TheoryData<Func<CustomerEntity>>
		{
			public CreateMockCustomerData()
			{
				Add(() => CustomerRepositoryFixture.CreateMockCustomer());
				Add(() => CustomerRepositoryFixture.CreateMockOptionalCustomer());
			}
		}

		[Theory]
		[ClassData(typeof(CreateMockCustomerData))]
		public void ShouldReadCustomerIncludingNullOptionalFields(
			Func<CustomerEntity> createMockCustomer)
		{
			// Given
			var repo = new CustomerRepository(DbContextHelper.Context);
			var customer = createMockCustomer();

			// When
			var readCustomer = repo.Read(1);

			// Then
			Assert.Equal(1, readCustomer.CustomerId);
			Assert.Equal(customer.FirstName, readCustomer.FirstName);
			Assert.Equal(customer.LastName, readCustomer.LastName);
			Assert.Equal(customer.PhoneNumber, readCustomer.PhoneNumber);
			Assert.Equal(customer.Email, readCustomer.Email);
			Assert.Equal(customer.TotalPurchasesAmount, readCustomer.TotalPurchasesAmount);
		}

		#endregion

		#region Read many

		[Fact]
		public void ShouldReadManyCustomers()
		{
			// Given
			var repo = new CustomerRepository(DbContextHelper.Context);
			var customer = CustomerRepositoryFixture.CreateMockCustomer(amount: 2);

			// When
			var readCustomers = repo.ReadMany();

			// Then
			Assert.Equal(2, readCustomers.Count);

			var a1 = repo.ReadMany();
			var a2 = repo.ReadMany();
			var a3 = repo.ReadMany();
			var a4 = repo.ReadMany();

			foreach (var readCustomer in readCustomers)
			{
				Assert.Equal(customer.FirstName, readCustomer.FirstName);
				Assert.Equal(customer.LastName, readCustomer.LastName);
				Assert.Equal(customer.PhoneNumber, readCustomer.PhoneNumber);
				Assert.Equal(customer.Email, readCustomer.Email);
				Assert.Equal(customer.TotalPurchasesAmount, readCustomer.TotalPurchasesAmount);
			}
		}

		[Fact]
		public void ShouldReadManyCustomersNotFound()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var readCustomers = repo.ReadMany();

			// Then
			Assert.Empty(readCustomers);
		}

		#endregion

		#region Get count

		[Fact]
		public void ShouldGetTotalCustomerCount()
		{
			// Given
			var repo = new CustomerRepository(DbContextHelper.Context);
			CustomerRepositoryFixture.CreateMockCustomer(amount: 2);

			// When
			var count = repo.GetCount();

			// Then
			Assert.Equal(2, count);
		}

		[Fact]
		public void ShouldGetTotalCustomerCountWhenEmpty()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var count = repo.GetCount();

			// Then
			Assert.Equal(0, count);
		}

		#endregion

		#region Read page

		[Fact]
		public void ShouldReadPageOfCustomersFromSinglePage()
		{
			// Given
			var repo = new CustomerRepository(DbContextHelper.Context);
			CustomerRepositoryFixture.CreateMockCustomer(amount: 5);

			// When
			var readCustomers = repo.ReadPage(1, 10);

			// Then
			Assert.Equal(5, readCustomers.Count);
		}

		[Fact]
		public void ShouldReadPageOfCustomersFromMultiplePages()
		{
			// Given
			var repo = new CustomerRepository(DbContextHelper.Context);
			CustomerRepositoryFixture.CreateMockCustomer(amount: 5);

			// When
			var readCustomers = repo.ReadPage(2, 3);

			// Then
			Assert.Equal(2, readCustomers.Count);
			var readCustomersList = readCustomers.ToArray();

			Assert.Equal(4, readCustomersList[0].CustomerId);
			Assert.Equal(5, readCustomersList[1].CustomerId);
		}

		[Fact]
		public void ShouldReadPageOfCustomersNotFoundEmptyCollection()
		{
			// Given
			var repo = CustomerRepositoryFixture.CreateEmptyRepository();

			// When
			var readCustomers = repo.ReadPage(2, 3);

			// Then
			Assert.Empty(readCustomers);
		}

		#endregion

		#region Update

		[Fact]
		public void ShouldUpdateCustomer()
		{
			// Given
			var repo = new CustomerRepository(DbContextHelper.Context);
			var customer = CustomerRepositoryFixture.CreateMockCustomer();

			var createdCustomer = repo.Read(1);
			createdCustomer.FirstName = "New FN";

			// When
			repo.Update(createdCustomer);

			// Then
			var updatedCustomer = repo.Read(1);

			Assert.Equal("New FN", updatedCustomer.FirstName);
			Assert.Equal(customer.LastName, updatedCustomer.LastName);
			Assert.Equal(customer.PhoneNumber, updatedCustomer.PhoneNumber);
			Assert.Equal(customer.Email, updatedCustomer.Email);
			Assert.Equal(customer.TotalPurchasesAmount, updatedCustomer.TotalPurchasesAmount);
		}

		#endregion

		#region Delete

		[Fact]
		public void ShouldDeleteCustomer()
		{
			// Given
			var repo = new CustomerRepository(DbContextHelper.Context);
			CustomerRepositoryFixture.CreateMockCustomer();

			var createdCustomer = repo.Read(1);
			Assert.NotNull(createdCustomer);

			// When
			repo.Delete(1);

			// Then
			var deletedCustomer = repo.Read(1);
			Assert.Null(deletedCustomer);
		}

		[Fact]
		public void ShouldDeleteAllCustomers()
		{
			// Given
			var repo = new CustomerRepository(DbContextHelper.Context);
			CustomerRepositoryFixture.CreateMockCustomer(amount: 2);

			var createdCustomers = repo.ReadMany();
			Assert.Equal(2, createdCustomers.Count);

			// When
			repo.DeleteAll();

			// Then
			var deletedCustomers = repo.ReadMany();
			Assert.Empty(deletedCustomers);
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
			var repo = new CustomerRepository(DbContextHelper.Context);
			CustomerRepositoryFixture.CreateMockCustomer("taken@asd.com");

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
			var repo = new CustomerRepository(DbContextHelper.Context);
			CustomerRepositoryFixture.CreateMockCustomer("taken@asd.com");

			// When
			var (isTaken, takenById) = repo.IsEmailTakenWithCustomerId(email);

			// Then
			Assert.Equal(isTakenExpected, isTaken);
			Assert.Equal(isTaken ? 1 : 0, takenById);
		}

		#endregion


		public class CustomerRepositoryFixture
		{
			/// <summary>
			/// Creates the empty repository.
			/// </summary>
			/// <returns>The empty customer repository.</returns>
			public static CustomerRepository CreateEmptyRepository()
			{
				DatabaseHelper.Clear();
				return new CustomerRepository(DbContextHelper.Context);
			}

			/// <summary>
			/// Creates the specified amount of mocked customers 
			/// with repo-relevant valid properties, optional properties not <see langword="null"/>.
			/// </summary>
			/// <param name="amount">The amount of customers to create.</param>
			/// <returns>The mocked customer with repo-relevant valid properties, 
			/// optional properties not <see langword="null"/>.</returns>
			public static CustomerEntity CreateMockCustomer(
				string email = "john@doe.com", int amount = 1)
			{
				var repo = new CustomerRepository(DbContextHelper.Context);

				var customer = MockCustomer(email);

				for (int i = 0; i < amount; i++)
				{
					repo.Create(customer);
					//repo.Create(MockCustomer(email));
				}

				//return MockCustomer(email);
				return customer;
			}

			/// <summary>
			/// Creates the mocked customer with  repo-relevant valid properties, 
			/// optional properties <see langword="null"/>.
			/// </summary>
			/// <returns>The mocked customer with repo-relevant valid properties,
			/// optional properties <see langword="null"/>.</returns>
			public static CustomerEntity CreateMockOptionalCustomer()
			{
				var repo = new CustomerRepository(DbContextHelper.Context);

				var customer = MockOptionalCustomer();
				repo.Create(customer);

				return customer;
			}

			/// <returns>The mocked customer with valid properties,
			/// optional properties not <see langword="null"/>.</returns>
			public static CustomerEntity MockCustomer(string email = "john@doe.com") => new()
			{
				FirstName = "John",
				LastName = "Doe",
				PhoneNumber = "+12345",
				Email = email,
				TotalPurchasesAmount = 123,
			};

			/// <returns>The mocked customer with valid properties,
			/// optional properties <see langword="null"/>.</returns>
			public static CustomerEntity MockOptionalCustomer() => new()
			{
				FirstName = null,
				LastName = "Doe",
				PhoneNumber = null,
				Email = null,
				TotalPurchasesAmount = null,
			};
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
				var result = new .Validate(customer);

				// Then
				Assert.True(result.IsValid);
			}

			[Fact]
			public void ShouldValidateCustomerWithOptionalPropertiesNull()
			{
				// Given
				var customer = new CustomerCreateRequestValidatorFixture().MockValidOptional();

				Assert.Null(customer.FirstName);
				Assert.Null(customer.Email);
				Assert.Null(customer.PhoneNumber);
				Assert.Null(customer.TotalPurchasesAmount);

				// When
				var result = _validator.Validate(customer);

				// Then
				Assert.True(result.IsValid);
			}
			[Fact]
			public void ShouldValidateCustomerWithOptionalPropertiesNotNull()
			{
				// Given
				var repo = new CustomerRepository(DbContextHelper.Context);
				CustomerRepositoryFixture.CreateMockCustomer();

				var createdCustomer = repo.Read(1);
				Assert.NotNull(createdCustomer);

				// When
				repo.Delete(1);

				// Then
				var deletedCustomer = repo.Read(1);
				Assert.Null(deletedCustomer);
			}
		}
	}
}
