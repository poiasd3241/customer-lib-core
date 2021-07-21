using AutoMapper;
using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.Business.Enums;
using Xunit;

namespace CustomerLibCore.Api.Tests.DTOs
{
	public class AutoMapperApiProfileTest
	{
		#region Constructor

		[Fact]
		public void ShouldCreateAutoMapperApiProfile()
		{
			var profile = new AutoMapperApiProfile();

			Assert.NotNull(profile);
		}

		#endregion

		#region Note - NoteDto

		[Fact]
		public void ShouldMapFromNoteToNoteDto()
		{
			// Given
			var note = MockNote();

			// When
			var noteDto = CreateMapper().Map<NoteDto>(note);

			// Then
			Assert.Equal(note.Content, noteDto.Content);
		}

		[Fact]
		public void ShouldMapFromNoteDtoToNote()
		{
			// Given
			var noteDto = MockNoteDto();

			// When
			var note = CreateMapper().Map<Note>(noteDto);

			// Then
			Assert.Equal(0, note.NoteId);
			Assert.Equal(0, note.CustomerId);
			Assert.Equal(noteDto.Content, note.Content);
		}

		#endregion

		#region Address - AddressDto

		[Fact]
		public void ShouldMapFromAddressToAddressDto()
		{
			// Given
			var address = MockAddress();

			// When
			var addressDto = CreateMapper().Map<AddressDto>(address);

			// Then
			Assert.Equal(address.Line, addressDto.Line);
			Assert.Equal(address.Line2, addressDto.Line2);
			Assert.Equal(address.Type.ToString(), addressDto.Type);
			Assert.Equal(address.City, addressDto.City);
			Assert.Equal(address.PostalCode, addressDto.PostalCode);
			Assert.Equal(address.State, addressDto.State);
			Assert.Equal(address.Country, addressDto.Country);
		}

		[Fact]
		public void ShouldMapFromAddressDtoToAddress()
		{
			// Given
			var addressDto = MockAddressDto();

			// When
			var address = CreateMapper().Map<Address>(addressDto);

			// Then
			Assert.Equal(0, address.AddressId);
			Assert.Equal(0, address.CustomerId);
			Assert.Equal(addressDto.Line, address.Line);
			Assert.Equal(addressDto.Line2, address.Line2);
			Assert.Equal(AddressType.Shipping, address.Type);
			Assert.Equal(addressDto.City, address.City);
			Assert.Equal(addressDto.PostalCode, address.PostalCode);
			Assert.Equal(addressDto.State, address.State);
			Assert.Equal(addressDto.Country, address.Country);
		}

		[Theory]
		[InlineData("0")]
		[InlineData("555")]
		[InlineData("whatever")]
		public void ShouldMapFromAddressDtoToAddressTypeDefaultShipping(string type)
		{
			// Given
			var addressDto = MockAddressDto();
			addressDto.Type = type;

			// When
			var address = CreateMapper().Map<Address>(addressDto);

			// Then
			Assert.Equal(AddressType.Shipping, address.Type);
		}

		#endregion

		#region Customer - CustomerDto

		[Fact]
		public void ShouldMapFromCustomerToCustomerDto()
		{
			// Given
			var address = MockAddress();
			var note = MockNote();

			var customer = MockCustomer();
			customer.Addresses = new() { address };
			customer.Notes = new() { note };

			// When
			var customerDto = CreateMapper().Map<CustomerDto>(customer);

			// Then
			Assert.Equal(customer.FirstName, customerDto.FirstName);
			Assert.Equal(customer.LastName, customerDto.LastName);
			Assert.Equal(customer.PhoneNumber, customerDto.PhoneNumber);
			Assert.Equal(customer.Email, customerDto.Email);
			Assert.Equal(customer.TotalPurchasesAmount.ToString(),
				customerDto.TotalPurchasesAmount);

			var addressDto = Assert.Single(customerDto.Addresses);
			Assert.True(AddressAndDtoEqualByValues(address, addressDto));

			var noteDto = Assert.Single(customerDto.Notes);
			Assert.True(NoteAndDtoEqualByValues(note, noteDto));
		}

		[Fact]
		public void ShouldMapFromCustomerDtoToCustomer()
		{
			// Given
			var addressDto = MockAddressDto();
			var noteDto = MockNoteDto();

			var customerDto = MockCustomerDto();
			customerDto.Addresses = new() { addressDto };
			customerDto.Notes = new() { noteDto };

			// When
			var customer = CreateMapper().Map<Customer>(customerDto);

			// Then
			Assert.Equal(0, customer.CustomerId);
			Assert.Equal(customerDto.FirstName, customer.FirstName);
			Assert.Equal(customerDto.LastName, customer.LastName);
			Assert.Equal(customerDto.PhoneNumber, customer.PhoneNumber);
			Assert.Equal(customerDto.Email, customer.Email);
			Assert.Equal(customerDto.TotalPurchasesAmount,
				customer.TotalPurchasesAmount.ToString());

			var address = Assert.Single(customer.Addresses);
			Assert.True(AddressAndDtoEqualByValues(address, addressDto));

			var note = Assert.Single(customer.Notes);
			Assert.True(NoteAndDtoEqualByValues(note, noteDto));
		}

		[Fact]
		public void ShouldMapFromCustomerDtoToCustomerTotalPurchasesAmountNull()
		{
			// Given
			var customerDto = MockCustomerDto();
			customerDto.TotalPurchasesAmount = null;

			// When
			var customer = CreateMapper().Map<Customer>(customerDto);

			// Then
			Assert.Null(customer.TotalPurchasesAmount);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("1.1.1")]
		public void ShouldThrowOnMapFromCustomerDtoToCustomerTotalPurchasesAmount(
			string totalPurchasesAmount)
		{
			// Given
			var mapper = CreateMapper();

			var customerDto = MockCustomerDto();
			customerDto.TotalPurchasesAmount = totalPurchasesAmount;

			// When, Then
			Assert.Throws<AutoMapperMappingException>(() => mapper.Map<Customer>(customerDto));
		}

		#endregion

		#region Customer - CustomerBasicDetailsDto

		[Fact]
		public void ShouldMapFromCustomerToCustomerBasicDetailsDto()
		{
			// Given
			var customer = MockCustomer();

			// When
			var customerBasicDetailsDto = CreateMapper().Map<CustomerBasicDetailsDto>(customer);

			// Then
			Assert.Equal(customer.FirstName, customerBasicDetailsDto.FirstName);
			Assert.Equal(customer.LastName, customerBasicDetailsDto.LastName);
			Assert.Equal(customer.PhoneNumber, customerBasicDetailsDto.PhoneNumber);
			Assert.Equal(customer.Email, customerBasicDetailsDto.Email);
			Assert.Equal(customer.TotalPurchasesAmount.ToString(),
				customerBasicDetailsDto.TotalPurchasesAmount);
		}

		[Fact]
		public void ShouldMapFromCustomerBasicDetailsDtoToCustomer()
		{
			// Given
			var customerBasicDetailsDto = MockCustomerBasicDetailsDto();

			// When
			var customer = CreateMapper().Map<Customer>(customerBasicDetailsDto);

			// Then
			Assert.Equal(0, customer.CustomerId);
			Assert.Equal(customerBasicDetailsDto.FirstName, customer.FirstName);
			Assert.Equal(customerBasicDetailsDto.LastName, customer.LastName);
			Assert.Equal(customerBasicDetailsDto.PhoneNumber, customer.PhoneNumber);
			Assert.Equal(customerBasicDetailsDto.Email, customer.Email);
			Assert.Equal(customerBasicDetailsDto.TotalPurchasesAmount,
				customer.TotalPurchasesAmount.ToString());

			Assert.Null(customer.Addresses);
			Assert.Null(customer.Notes);
		}

		[Fact]
		public void ShouldMapFromCustomerBasicDetailsDtoToCustomerTotalPurchasesAmountNull()
		{
			// Given
			var customerBasicDetailsDto = MockCustomerBasicDetailsDto();
			customerBasicDetailsDto.TotalPurchasesAmount = null;

			// When
			var customer = CreateMapper().Map<Customer>(customerBasicDetailsDto);

			// Then
			Assert.Null(customer.TotalPurchasesAmount);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("1.1.1")]
		public void ShouldThrowOnMapFromCustomerBasicDetailsDtoToCustomerTotalPurchasesAmount(
			string totalPurchasesAmount)
		{
			// Given
			var mapper = CreateMapper();

			var customerBasicDetailsDto = MockCustomerBasicDetailsDto();
			customerBasicDetailsDto.TotalPurchasesAmount = totalPurchasesAmount;

			// When, Then
			Assert.Throws<AutoMapperMappingException>(() =>
				mapper.Map<Customer>(customerBasicDetailsDto));
		}

		#endregion

		public static IMapper CreateMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new AutoMapperApiProfile());
			});

			return config.CreateMapper();
		}

		public static bool NoteAndDtoEqualByValues(Note note, NoteDto noteDto)
		{
			return note.Content == noteDto.Content;
		}

		public static bool AddressAndDtoEqualByValues(Address address, AddressDto addressDto)
		{
			return
				address.Line == addressDto.Line &&
				address.Line2 == addressDto.Line2 &&
				address.Type.ToString() == addressDto.Type &&
				address.City == addressDto.City &&
				address.PostalCode == addressDto.PostalCode &&
				address.State == addressDto.State &&
				address.Country == addressDto.Country;
		}

		public static Address MockAddress() => new()
		{
			AddressId = 2,
			CustomerId = 3,
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

		public static Note MockNote() => new()
		{
			NoteId = 4,
			CustomerId = 5,
			Content = "MockNote content"
		};

		public static NoteDto MockNoteDto() => new()
		{
			Content = "MockNoteDto content"
		};

		public static Customer MockCustomer() => new()
		{
			CustomerId = 6,
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

		public static CustomerBasicDetailsDto MockCustomerBasicDetailsDto() => new()
		{
			FirstName = "One",
			LastName = "Two",
			PhoneNumber = "+123",
			Email = "a@a.aa",
			TotalPurchasesAmount = "666"
		};
	}
}
