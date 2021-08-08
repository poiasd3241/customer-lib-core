//using System;
//using CustomerLibCore.Domain.Models;
//using CustomerLibCore.Domain.Enums;
//using CustomerLibCore.Data.Repositories.EF;
//using CustomerLibCore.TestHelpers;
//using Xunit;
//using static CustomerLibCore.Data.IntegrationTests.Repositories.EF.CustomerRepositoryTest;

//namespace CustomerLibCore.Data.IntegrationTests.Repositories.EF
//{
//	[Collection(nameof(NotDbSafeResourceCollection))]
//	public class AddressRepositoryTest
//	{
//		#region Constructor

//		[Fact]
//		public void ShouldCreateAddressRepository()
//		{
//			var context = new StrictMock<CustomerLibDataContext>();

//			var repo = new AddressRepository(context.Object);

//			Assert.NotNull(repo);
//		}

//		#endregion

//		#region Exists

//		[Theory]
//		[InlineData(2, true)]
//		[InlineData(3, false)]
//		public void ShouldCheckIfNoteExistsById(int addressId, bool expectedExists)
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();
//			AddressRepositoryFixture.CreateMockAddress(amount: 2);

//			// When
//			var exists = repo.Exists(addressId);

//			// Then
//			Assert.Equal(expectedExists, exists);
//		}

//		[Theory]
//		[InlineData(2, 1, true)]
//		[InlineData(2, 55, false)]
//		[InlineData(3, 1, false)]
//		[InlineData(3, 55, false)]
//		public void ShouldCheckIfNoteExistsForCustomerId(
//			int addressId, int customerId, bool expectedExists)
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();
//			AddressRepositoryFixture.CreateMockAddress(amount: 2);

//			// When
//			var exists = repo.ExistsForCustomer(addressId, customerId);

//			// Then
//			Assert.Equal(expectedExists, exists);
//		}

//		#endregion

//		#region Create

//		[Fact]
//		public void ShouldCreateAddress()
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();

//			var address = AddressRepositoryFixture.MockAddress();
//			address.CustomerId = 1;

//			// When
//			var createdId = repo.Create(address);

//			// Then
//			Assert.Equal(1, createdId);
//		}

//		#endregion

//		#region Read by Id

//		[Fact]
//		public void ShouldReadAddressNotFound()
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();

//			// When
//			var readAddress = repo.Read(1);

//			// Then
//			Assert.Null(readAddress);
//		}

//		public class CreateMockAddressData : TheoryData<Func<Address>>
//		{
//			public CreateMockAddressData()
//			{
//				Add(() => AddressRepositoryFixture.CreateMockAddress());
//				Add(() => AddressRepositoryFixture.CreateMockOptionalAddress());
//			}
//		}

//		[Theory]
//		[ClassData(typeof(CreateMockAddressData))]
//		public void ShouldReadAddressIncludingNullOptionalFields(Func<Address> createMockAddress)
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();
//			var address = createMockAddress();

//			// When
//			var readAddress = repo.Read(1);

//			// Then
//			Assert.Equal(1, readAddress.AddressId);
//			Assert.Equal(address.CustomerId, readAddress.CustomerId);
//			Assert.Equal(address.Line, readAddress.Line);
//			Assert.Equal(address.Line2, readAddress.Line2);
//			Assert.Equal(address.Type, readAddress.Type);
//			Assert.Equal(address.City, readAddress.City);
//			Assert.Equal(address.PostalCode, readAddress.PostalCode);
//			Assert.Equal(address.State, readAddress.State);
//			Assert.Equal(address.Country, readAddress.Country);
//		}

//		#endregion

//		#region Read by customer

//		[Fact]
//		public void ShouldReadSingleForCustomerNotFound()
//		{
//			// Given
//			var addressId = 5;
//			var customerId = 7;

//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();

//			// When
//			var readAddress = repo.ReadForCustomer(addressId, customerId);

//			// Then
//			Assert.Null(readAddress);
//		}

//		[Fact]
//		public void ShouldReadSingleForCustomer()
//		{
//			// Given
//			var addressId1 = 1;
//			var addressId2 = 2;
//			var customerId1 = 1;
//			var customerId2 = 2;

//			var city1 = "one";
//			var city2 = "two";

//			var expectedAddress1 = AddressRepositoryFixture.MockAddress(customerId1);
//			expectedAddress1.City = city1;

//			var expectedAddress2 = AddressRepositoryFixture.MockAddress(customerId2);
//			expectedAddress2.City = city2;

//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer(2);
//			expectedAddress1.AddressId = repo.Create(expectedAddress1);
//			expectedAddress2.AddressId = repo.Create(expectedAddress2);

//			// When
//			var readAddress1 = repo.ReadForCustomer(addressId1, customerId1);
//			var readAddress2 = repo.ReadForCustomer(addressId2, customerId2);

//			// Then
//			Assert.Equal(addressId1, readAddress1.AddressId);
//			Assert.Equal(addressId2, readAddress2.AddressId);

//			Assert.True(expectedAddress1.EqualsByValue(readAddress1));
//			Assert.True(expectedAddress2.EqualsByValue(readAddress2));
//		}

//		[Fact]
//		public void ShouldReadManyForCustomer()
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();
//			var address = AddressRepositoryFixture.CreateMockAddress(2);

//			// When
//			var readAddresses = repo.ReadManyForCustomer(address.CustomerId);

//			// Then
//			Assert.Equal(2, readAddresses.Count);

//			foreach (var readAddress in readAddresses)
//			{
//				Assert.Equal(address.CustomerId, readAddress.CustomerId);
//				Assert.Equal(address.Line, readAddress.Line);
//				Assert.Equal(address.Line2, readAddress.Line2);
//				Assert.Equal(address.Type, readAddress.Type);
//				Assert.Equal(address.City, readAddress.City);
//				Assert.Equal(address.PostalCode, readAddress.PostalCode);
//				Assert.Equal(address.State, readAddress.State);
//				Assert.Equal(address.Country, readAddress.Country);
//			}
//		}

//		[Theory]
//		[InlineData(1)]
//		[InlineData(2)]
//		public void ShouldReadManyForCustomerBothNotFoundAndEmpty(int customerId)
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();

//			// When
//			var readAddresses = repo.ReadManyForCustomer(customerId);

//			// Then
//			Assert.Empty(readAddresses);
//		}

//		#endregion

//		#region Update

//		[Fact]
//		public void ShouldUpdateAddressWithCustomerChange()
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer(2);
//			AddressRepositoryFixture.CreateMockAddress();
//			var addressId = 1;

//			var newCustomerId = 2;
//			var newLine = "New line!";

//			var readAddress = repo.Read(addressId);

//			var address = readAddress.Copy();
//			var beforeUpdate = readAddress.Copy();

//			address.CustomerId = newCustomerId;
//			address.Line = newLine;

//			// When
//			repo.Update(address);

//			// Then
//			var afterUpdate = repo.Read(addressId);

//			// CustomerId - updated
//			var customerIdBeforeUpdate = beforeUpdate.CustomerId;
//			Assert.Equal(1, customerIdBeforeUpdate);
//			Assert.NotEqual(customerIdBeforeUpdate, newCustomerId);

//			Assert.Equal(newCustomerId, afterUpdate.CustomerId);

//			// Line - updated
//			var addressLineBeforeUpdate = beforeUpdate.Line;
//			Assert.Equal("one", addressLineBeforeUpdate);
//			Assert.NotEqual(addressLineBeforeUpdate, newLine);

//			Assert.Equal(newLine, afterUpdate.Line);

//			// Other properties untouched
//			Assert.Equal(addressId, beforeUpdate.AddressId);
//			Assert.Equal(beforeUpdate.AddressId, afterUpdate.AddressId);

//			Assert.Equal(beforeUpdate.Line2, afterUpdate.Line2);
//			Assert.Equal(beforeUpdate.Type, afterUpdate.Type);
//			Assert.Equal(beforeUpdate.City, afterUpdate.City);
//			Assert.Equal(beforeUpdate.PostalCode, afterUpdate.PostalCode);
//			Assert.Equal(beforeUpdate.State, afterUpdate.State);
//			Assert.Equal(beforeUpdate.Country, afterUpdate.Country);
//		}

//		[Fact]
//		public void ShouldUpdateAddressForCustomerWhenCustomerIdNotModified()
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();
//			AddressRepositoryFixture.CreateMockAddress();
//			var addressId = 1;

//			var newLine = "New line!";

//			var readAddress = repo.Read(addressId);

//			var address = readAddress.Copy();
//			var beforeUpdate = readAddress.Copy();

//			address.Line = newLine;

//			// When
//			repo.UpdateForCustomer(address);

//			// Then
//			var afterUpdate = repo.Read(addressId);

//			// CustomerId - untouched
//			var customerIdBeforeUpdate = beforeUpdate.CustomerId;
//			Assert.Equal(1, customerIdBeforeUpdate);
//			Assert.Equal(customerIdBeforeUpdate, afterUpdate.CustomerId);

//			// Line - updated
//			var addressLineBeforeUpdate = beforeUpdate.Line;
//			Assert.Equal("one", addressLineBeforeUpdate);
//			Assert.NotEqual(addressLineBeforeUpdate, newLine);

//			Assert.Equal(newLine, afterUpdate.Line);

//			// Other properties untouched
//			Assert.Equal(addressId, beforeUpdate.AddressId);
//			Assert.Equal(beforeUpdate.AddressId, afterUpdate.AddressId);

//			Assert.Equal(beforeUpdate.Line2, afterUpdate.Line2);
//			Assert.Equal(beforeUpdate.Type, afterUpdate.Type);
//			Assert.Equal(beforeUpdate.City, afterUpdate.City);
//			Assert.Equal(beforeUpdate.PostalCode, afterUpdate.PostalCode);
//			Assert.Equal(beforeUpdate.State, afterUpdate.State);
//			Assert.Equal(beforeUpdate.Country, afterUpdate.Country);
//		}

//		[Fact]
//		public void ShouldNotUpdateAddressForCustomerWhenTryingToChangeCustomerId()
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer(2);
//			AddressRepositoryFixture.CreateMockAddress();
//			var addressId = 1;

//			var newCustomerIdTry = 2;
//			var newLine = "New line!";

//			var readAddress = repo.Read(addressId);

//			var address = readAddress.Copy();
//			var beforeUpdate = readAddress.Copy();

//			address.CustomerId = newCustomerIdTry;
//			address.Line = newLine;

//			// When
//			repo.UpdateForCustomer(address);

//			// Then
//			var afterUpdate = repo.Read(addressId);

//			// CustomerId - untouched
//			var customerIdBeforeUpdate = beforeUpdate.CustomerId;
//			Assert.Equal(1, customerIdBeforeUpdate);
//			Assert.NotEqual(customerIdBeforeUpdate, newCustomerIdTry);

//			Assert.Equal(customerIdBeforeUpdate, afterUpdate.CustomerId);

//			// Line - untouched
//			var addressLineBeforeUpdate = beforeUpdate.Line;
//			Assert.Equal("one", addressLineBeforeUpdate);
//			Assert.NotEqual(addressLineBeforeUpdate, newLine);

//			Assert.Equal(addressLineBeforeUpdate, afterUpdate.Line);

//			// All properties untouched
//			Assert.Equal(addressId, beforeUpdate.AddressId);

//			Assert.True(afterUpdate.EqualsByValue(beforeUpdate));
//		}

//		#endregion

//		#region Delete

//		[Fact]
//		public void ShouldDeleteAddress()
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();
//			AddressRepositoryFixture.CreateMockAddress(2);

//			var createdAddress1 = repo.Read(1);
//			var createdAddress2 = repo.Read(2);
//			Assert.NotNull(createdAddress1);
//			Assert.NotNull(createdAddress2);

//			// When
//			repo.Delete(1);

//			// Then
//			var deletedAddress = repo.Read(1);
//			var untouchedAddress = repo.Read(2);

//			Assert.Null(deletedAddress);

//			Assert.NotNull(untouchedAddress);
//			Assert.True(untouchedAddress.EqualsByValue(createdAddress2));
//		}

//		[Fact]
//		public void ShouldDeleteManyAddressesForCustomerId()
//		{
//			// Given
//			var customerId1 = 1;
//			var customerId2 = 2;

//			var addressRepo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer(2);

//			AddressRepositoryFixture.CreateMockAddress(amount: 2, customerId1);
//			AddressRepositoryFixture.CreateMockAddress(amount: 3, customerId2);

//			var readAddresses1 = addressRepo.ReadManyForCustomer(customerId1);
//			var readAddresses2 = addressRepo.ReadManyForCustomer(customerId2);

//			Assert.Equal(2, readAddresses1.Count);
//			Assert.Equal(3, readAddresses2.Count);

//			// When
//			addressRepo.DeleteManyForCustomer(customerId1);

//			// Then
//			var deletedAddresses = addressRepo.ReadManyForCustomer(customerId1);
//			var untouchedAddresses = addressRepo.ReadManyForCustomer(customerId2);

//			Assert.Empty(deletedAddresses);
//			Assert.Equal(3, untouchedAddresses.Count);
//		}

//		[Fact]
//		public void ShouldDeleteAllAddresses()
//		{
//			// Given
//			var repo = AddressRepositoryFixture.CreateEmptyRepositoryWithCustomer();
//			AddressRepositoryFixture.CreateMockAddress(2);

//			var createdAddresses = repo.ReadManyForCustomer(1);
//			Assert.Equal(2, createdAddresses.Count);

//			// When
//			repo.DeleteAll();

//			// Then
//			var deletedAddresses = repo.ReadManyForCustomer(1);
//			Assert.Empty(deletedAddresses);
//		}

//		#endregion

//		public class AddressRepositoryFixture
//		{
//			/// <summary>
//			/// Creates the empty repository, containing the specified amount of customers
//			/// (<see cref="Customer.CustomerId"/> = 1 for the first customer and
//			/// +1 for every next customer) and no addresses.
//			/// <br/>
//			/// Also repopulates the <see cref="AddressType"/> table.
//			/// </summary>
//			/// <returns>The empty address repository.</returns>
//			public static AddressRepository CreateEmptyRepositoryWithCustomer(
//				int customersAmount = 1)
//			{
//				DatabaseHelper.Clear();
//				DatabaseHelper.UnsafeRepopulateAddressTypes();

//				CustomerRepositoryFixture.CreateMockCustomer(amount: customersAmount);

//				return new(DbContextHelper.Context);
//			}

//			/// <summary>
//			/// Creates the specified amount of mocked addresses  with repo-relevant 
//			/// valid properties, optional properties not <see langword="null"/>, 
//			/// for the specified customer.
//			/// </summary>
//			/// <param name="amount">The amount of addresses to create.</param>
//			/// <param name="customerId">The Id of the customer to create addresses for.</param>
//			/// <returns>The mocked address with repo-relevant valid properties.</returns>
//			public static Address CreateMockAddress(int amount = 1, int customerId = 1)
//			{
//				var repo = new AddressRepository(DbContextHelper.Context);

//				var address = MockAddress(customerId);

//				for (int i = 0; i < amount; i++)
//				{
//					repo.Create(address);
//				}

//				return address;
//			}

//			/// <summary>
//			/// Creates the mocked address with repo-relevant valid properties,
//			/// optional properties <see langword="null"/>, <see cref="Address.CustomerId"/> = 1.
//			/// </summary>
//			/// <returns>The mocked address with repo-relevant valid properties, 
//			/// optional properties <see langword="null"/>, <see cref="Address.CustomerId"/> = 1.
//			/// </returns>
//			public static Address CreateMockOptionalAddress()
//			{
//				var repo = new AddressRepository(DbContextHelper.Context);

//				var address = MockOptionalAddress();

//				repo.Create(address);

//				return address;
//			}

//			/// <returns>The mocked address with repo-relevant valid properties,
//			/// optional properties not <see langword="null"/>.</returns>
//			public static Address MockAddress(int customerId = 1) => new()
//			{
//				CustomerId = customerId,
//				Line = "one",
//				Line2 = "two",
//				Type = AddressType.Shipping,
//				City = "Seattle",
//				PostalCode = "123456",
//				State = "WA",
//				Country = "United States"
//			};

//			/// <returns>The mocked address with repo-relevant valid properties,
//			/// optional properties <see langword="null"/>, <see cref="Address.CustomerId"/> = 1.
//			/// </returns>
//			public static Address MockOptionalAddress()
//			{
//				var mockAddress = MockAddress(1);
//				mockAddress.Line2 = null;
//				return mockAddress;
//			}
//		}
//	}
//}
