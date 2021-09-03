using AutoMapper;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.TestHelpers.ModelsAssert;
using Xunit;

namespace CustomerLibCore.Data.Tests.Entities
{
	public class AutoMapperDataProfileTest
	{
		private IMapper _mapper;

		public IMapper Mapper
		{
			get
			{
				if (_mapper is null)
				{
					var config = new MapperConfiguration(cfg =>
					{
						cfg.AddProfile(new AutoMapperDataProfile());
					});

					_mapper = config.CreateMapper();
				}

				return _mapper;
			}
		}

		#region Constructor

		[Fact]
		public void ShouldCreateAutoMapperDataProfile()
		{
			var profile = new AutoMapperDataProfile();

			Assert.NotNull(profile);
		}

		#endregion

		#region Address <-> AddressEntity

		[Fact]
		public void ShouldMapFromAddressToAddressEntity()
		{
			// Given
			var address = MockAddress();

			// When
			var entity = Mapper.Map<AddressEntity>(address);

			// Then
			Assert.Equal(address.AddressId, entity.AddressId);
			Assert.Equal(address.AddressId, entity.AddressId);
			Assert.Equal(address.Line, entity.Line);
			Assert.Equal(address.Line2, entity.Line2);
			Assert.Equal(address.Type, entity.Type);
			Assert.Equal(address.City, entity.City);
			Assert.Equal(address.PostalCode, entity.PostalCode);
			Assert.Equal(address.State, entity.State);
			Assert.Equal(address.Country, entity.Country);
		}

		[Fact]
		public void ShouldMapFromAddressEntityToAddress()
		{
			// Given
			var addressEntity = MockAddressEntity();

			// When
			var address = Mapper.Map<Address>(addressEntity);

			// Then
			Assert.Equal(addressEntity.AddressId, address.AddressId);
			Assert.Equal(addressEntity.AddressId, address.AddressId);
			Assert.Equal(addressEntity.Line, address.Line);
			Assert.Equal(addressEntity.Line2, address.Line2);
			Assert.Equal(addressEntity.Type, address.Type);
			Assert.Equal(addressEntity.City, address.City);
			Assert.Equal(addressEntity.PostalCode, address.PostalCode);
			Assert.Equal(addressEntity.State, address.State);
			Assert.Equal(addressEntity.Country, address.Country);
		}

		#endregion

		#region Note <-> NoteEntity

		[Fact]
		public void ShouldMapFromNoteToNoteEntity()
		{
			// Given
			var note = MockNote();

			// When
			var entity = Mapper.Map<NoteEntity>(note);

			// Then
			Assert.Equal(note.NoteId, entity.NoteId);
			Assert.Equal(note.NoteId, entity.NoteId);
			Assert.Equal(note.Content, entity.Content);
		}

		[Fact]
		public void ShouldMapFromNoteEntityToNote()
		{
			// Given
			var noteEntity = MockNoteEntity();

			// When
			var note = Mapper.Map<Note>(noteEntity);

			// Then
			Assert.Equal(noteEntity.NoteId, note.NoteId);
			Assert.Equal(noteEntity.NoteId, note.NoteId);
			Assert.Equal(noteEntity.Content, note.Content);
		}

		#endregion

		#region Customer <-> CustomerEntity

		[Fact]
		public void ShouldMapFromCustomerToCustomerEntity()
		{
			// Given
			var customer = MockCustomer();

			// When
			var entity = Mapper.Map<CustomerEntity>(customer);

			// Then
			Assert.Equal(customer.CustomerId, entity.CustomerId);
			Assert.Equal(customer.CustomerId, entity.CustomerId);
			Assert.Equal(customer.FirstName, entity.FirstName);
			Assert.Equal(customer.LastName, entity.LastName);
			Assert.Equal(customer.PhoneNumber, entity.PhoneNumber);
			Assert.Equal(customer.Email, entity.Email);
			Assert.Equal(customer.TotalPurchasesAmount, entity.TotalPurchasesAmount);
		}

		[Fact]
		public void ShouldMapFromCustomerEntityToCustomer()
		{
			// Given
			var customerEntity = MockCustomerEntity();

			// When
			var customer = Mapper.Map<Customer>(customerEntity);

			// Then
			Assert.Equal(customerEntity.CustomerId, customer.CustomerId);
			Assert.Equal(customerEntity.CustomerId, customer.CustomerId);
			Assert.Equal(customerEntity.FirstName, customer.FirstName);
			Assert.Equal(customerEntity.LastName, customer.LastName);
			Assert.Equal(customerEntity.PhoneNumber, customer.PhoneNumber);
			Assert.Equal(customerEntity.Email, customer.Email);
			Assert.Equal(customerEntity.TotalPurchasesAmount, customer.TotalPurchasesAmount);

			Assert.Null(customer.Addresses);
			Assert.Null(customer.Notes);
		}

		#endregion

		#region Mocks - Domain models

		public static Customer MockCustomer() => new()
		{
			CustomerId = 2,
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
			AddressId = 3,
			CustomerId = 2,
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
			NoteId = 4,
			CustomerId = 2,
			Content = "Content1"
		};

		#endregion

		#region Mocks - Data entities

		public static CustomerEntity MockCustomerEntity() => new()
		{
			CustomerId = 2,
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@b.c",
			TotalPurchasesAmount = 666,
		};

		public static AddressEntity MockAddressEntity() => new()
		{
			AddressId = 3,
			CustomerId = 2,
			Line = "Line1",
			Line2 = "Line21",
			Type = AddressType.Shipping,
			City = "City1",
			PostalCode = "123456",
			State = "State1",
			Country = "United States"
		};

		public static NoteEntity MockNoteEntity() => new()
		{
			NoteId = 4,
			CustomerId = 2,
			Content = "Content1"
		};

		#endregion

		#region Mocks tests

		[Fact]
		public void ShouldMockMeaningfulData()
		{
			var assertDomainModels = new AssertDomainModels();
			assertDomainModels.MeaningfulWithIdsAndNested(MockCustomer());
			assertDomainModels.MeaningfulWithIds(MockAddress());
			assertDomainModels.MeaningfulWithIds(MockNote());

			var assertDataEntities = new AssertDataEntities();
			assertDataEntities.MeaningfulWithIds(MockCustomerEntity());
			assertDataEntities.MeaningfulWithIds(MockAddressEntity());
			assertDataEntities.MeaningfulWithIds(MockNoteEntity());
		}

		#endregion
	}
}
