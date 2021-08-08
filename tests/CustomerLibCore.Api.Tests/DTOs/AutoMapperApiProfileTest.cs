using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CustomerLibCore.Api.Controllers;
using CustomerLibCore.Api.Dtos;
using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Addresses.Response;
using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Api.Dtos.Customers.Response;
using CustomerLibCore.Api.Dtos.Notes.Request;
using CustomerLibCore.Api.Dtos.Notes.Response;
using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.Models;
using Xunit;

namespace CustomerLibCore.Api.Tests.DTOs
{
	public class AutoMapperApiProfileTest
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
						cfg.AddProfile(new AutoMapperApiProfile());
					});

					_mapper = config.CreateMapper();
				}

				return _mapper;
			}
		}

		#region Constructor

		[Fact]
		public void ShouldCreateAutoMapperApiProfile()
		{
			var profile = new AutoMapperApiProfile();

			Assert.NotNull(profile);
		}

		#endregion

		#region Address -> AddressResponse, IEnumerable -> AddressListResponse

		[Fact]
		public void ShouldMapFromAddressToAddressResponse()
		{
			// Given
			var address = MockAddress();
			var expectedSelf = LinkHelper.Address(address.CustomerId, address.AddressId);

			// When
			var addressResponse = Mapper.Map<AddressResponse>(address);

			// Then
			Assert.Equal(expectedSelf, addressResponse.Self);
			Assert.Equal(address.Line, addressResponse.Line);
			Assert.Equal(address.Line2, addressResponse.Line2);
			Assert.Equal(address.Type.ToString(), addressResponse.Type);
			Assert.Equal(address.City, addressResponse.City);
			Assert.Equal(address.PostalCode, addressResponse.PostalCode);
			Assert.Equal(address.State, addressResponse.State);
			Assert.Equal(address.Country, addressResponse.Country);
		}

		[Fact]
		public void ShouldMapFromAddressesToAddressListResponse()
		{
			// Given
			var address = MockAddress();
			var addressResponse = Mapper.Map<AddressResponse>(address);

			var addresses = new List<Address>() { address };

			// When
			var addressListResponse = Mapper.Map<AddressListResponse>(addresses);

			// Then
			Assert.Null(addressListResponse.Self);

			Assert.True(AddressResponsesEqualByValues(
				addressResponse, addressListResponse.Items.ElementAt(0)));
		}

		#endregion

		#region AddressRequest -> Address

		[Theory]
		[InlineData("0")]
		[InlineData("1")]
		[InlineData("2")]
		[InlineData("666")]
		[InlineData("whatever")]
		public void ShouldThrowOnMapFromAddressRequestToAddressForBadType(string type)
		{
			// Given
			var addressRequest = MockAddressRequest();
			addressRequest.Type = type;

			// When
			var ex = Assert.Throws<AutoMapperMappingException>(() =>
				Mapper.Map<Address>(addressRequest));

			// Then
			Assert.Equal($"the Type must be a name of a defined {nameof(AddressType)} enum",
				ex.InnerException.Message);
		}

		[Fact]
		public void ShouldMapFromAddressRequestToAddress()
		{
			// Given
			var addressRequest = MockAddressRequest();

			// When
			var address = Mapper.Map<Address>(addressRequest);

			// Then
			Assert.Equal(0, address.AddressId);
			Assert.Equal(0, address.CustomerId);

			Assert.Equal(addressRequest.Line, address.Line);
			Assert.Equal(addressRequest.Line2, address.Line2);
			Assert.Equal(AddressType.Shipping, address.Type);
			Assert.Equal(addressRequest.City, address.City);
			Assert.Equal(addressRequest.PostalCode, address.PostalCode);
			Assert.Equal(addressRequest.State, address.State);
			Assert.Equal(addressRequest.Country, address.Country);
		}

		#endregion

		#region Note -> NoteResponse, IEnumerable -> NoteListResponse

		[Fact]
		public void ShouldMapFromNoteToNoteResponse()
		{
			// Given
			var note = MockNote();
			var expectedSelf = LinkHelper.Note(note.CustomerId, note.NoteId);

			// When
			var noteResponse = Mapper.Map<NoteResponse>(note);

			// Then
			Assert.Equal(expectedSelf, noteResponse.Self);
			Assert.Equal(note.Content, noteResponse.Content);
		}

		[Fact]
		public void ShouldMapFromNotesToNoteListResponse()
		{
			// Given
			var note = MockNote();
			var noteResponse = Mapper.Map<NoteResponse>(note);

			var notes = new List<Note>() { note };

			// When
			var noteListResponse = Mapper.Map<NoteListResponse>(notes);

			// Then
			Assert.Null(noteListResponse.Self);

			Assert.True(NoteResponsesEqualByValues(
				noteResponse, noteListResponse.Items.ElementAt(0)));
		}

		#endregion

		#region NoteRequest -> Note

		[Fact]
		public void ShouldMapFromNoteRequestToNote()
		{
			// Given
			var notRequeste = MockNoteRequest();

			// When
			var note = Mapper.Map<Note>(notRequeste);

			// Then
			Assert.Equal(0, note.NoteId);
			Assert.Equal(0, note.CustomerId);

			Assert.Equal(notRequeste.Content, note.Content);
		}

		#endregion

		#region Customer -> CustomerResponse

		private class TotalPurchasesAmountData : TheoryData<string, decimal?>
		{
			public TotalPurchasesAmountData()
			{
				Add(null, null);
				Add("0", 0);
				Add("1", 1);
				Add("1.1", 1.1M);
				Add("-2.22", -2.22M);
				Add("666", 666);
			}
		}

		[Theory]
		[ClassData(typeof(TotalPurchasesAmountData))]
		public void ShouldMapFromCustomerToCustomerResponse(
			string totalPurchasesAmountText, decimal? totalPurchasesAmountNumber)
		{
			// Given
			var customer = MockCustomer();
			customer.TotalPurchasesAmount = totalPurchasesAmountNumber;

			var address = MockAddress();
			var note = MockNote();

			customer.Addresses = new[] { address };
			customer.Notes = new[] { note };

			var addressResponse = Mapper.Map<AddressResponse>(address);
			var noteResponse = Mapper.Map<NoteResponse>(note);

			var expectedSelf = LinkHelper.Customer(customer.CustomerId);
			var expectedAddressesSelf = LinkHelper.Addresses(customer.CustomerId);
			var expectedNotesSelf = LinkHelper.Notes(customer.CustomerId);

			// When
			var customerResponse = Mapper.Map<CustomerResponse>(customer);

			// Then
			Assert.Equal(expectedSelf, customerResponse.Self);
			Assert.Equal(customer.FirstName, customerResponse.FirstName);
			Assert.Equal(customer.LastName, customerResponse.LastName);
			Assert.Equal(customer.PhoneNumber, customerResponse.PhoneNumber);
			Assert.Equal(customer.Email, customerResponse.Email);
			Assert.Equal(totalPurchasesAmountText, customerResponse.TotalPurchasesAmount);

			var addressListResponse = customerResponse.Addresses;
			var noteListResponse = customerResponse.Notes;

			Assert.Equal(expectedAddressesSelf, addressListResponse.Self);

			var actualAddressListResponse = Assert.Single(addressListResponse.Items);
			Assert.True(AddressResponsesEqualByValues(addressResponse, actualAddressListResponse));

			Assert.Equal(expectedNotesSelf, noteListResponse.Self);

			var actualNoteListResponse = Assert.Single(noteListResponse.Items);
			Assert.True(NoteResponsesEqualByValues(noteResponse, actualNoteListResponse));
		}

		[Fact]
		public void ShouldMapFromCustomerToCustomerResponseWithNullAddressesAndNotes()
		{
			// Given
			var customer = MockCustomer();
			customer.Addresses = null;
			customer.Notes = null;

			var expectedSelf = LinkHelper.Customer(customer.CustomerId);
			var expectedAddressesSelf = LinkHelper.Addresses(customer.CustomerId);
			var expectedNotesSelf = LinkHelper.Notes(customer.CustomerId);

			// When
			var customerResponse = Mapper.Map<CustomerResponse>(customer);

			// Then
			Assert.Equal(expectedSelf, customerResponse.Self);
			Assert.Equal(customer.FirstName, customerResponse.FirstName);
			Assert.Equal(customer.LastName, customerResponse.LastName);
			Assert.Equal(customer.PhoneNumber, customerResponse.PhoneNumber);
			Assert.Equal(customer.Email, customerResponse.Email);
			Assert.Equal(customer.TotalPurchasesAmount.ToString(),
				customerResponse.TotalPurchasesAmount);

			var addressListResponse = customerResponse.Addresses;
			var noteListResponse = customerResponse.Notes;

			Assert.Equal(expectedAddressesSelf, addressListResponse.Self);
			Assert.Null(addressListResponse.Items);

			Assert.Equal(expectedNotesSelf, noteListResponse.Self);
			Assert.Null(noteListResponse.Items);
		}

		#endregion

		#region PagedResult<Customer> -> CustomerPagedResponse

		[Fact]
		public void ShouldMapFromPagedResultCustomerToCustomerPagedResponse()
		{
			// Given
			var page = 2;
			var pageSize = 5;
			var lastPage = 3;
			var customer = MockCustomer();

			var customerResponse = Mapper.Map<CustomerResponse>(customer);

			var pagedResult = MockCustomerPagedResult();
			pagedResult.Page = page;
			pagedResult.PageSize = pageSize;
			pagedResult.LastPage = lastPage;
			pagedResult.Items = new[] { customer };

			var expectedSelf = LinkHelper.CustomersPage(page, pageSize);
			var expectedPrevious = LinkHelper.CustomersPage(page - 1, pageSize);
			var expectedNext = LinkHelper.CustomersPage(page + 1, pageSize);

			// When
			var pagedResponse = Mapper.Map<CustomerPagedResponse>(pagedResult);

			// Then
			Assert.Equal(expectedSelf, pagedResponse.Self);
			Assert.Equal(expectedPrevious, pagedResponse.Previous);
			Assert.Equal(expectedNext, pagedResponse.Next);

			Assert.Equal(page, pagedResponse.Page);
			Assert.Equal(pageSize, pagedResponse.PageSize);
			Assert.Equal(lastPage, pagedResponse.LastPage);

			var actualCustomerResponse = Assert.Single(pagedResponse.Items);

			Assert.True(CustomerResponsesEqualByValues(customerResponse, actualCustomerResponse));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		public void ShouldMapFromCustomerPagedResultToCustomerPagedResponseWithNullPreviousLink(
			int page)
		{
			// Given
			var pagedResult = MockCustomerPagedResult();
			pagedResult.Page = page;

			// When
			var pagedResponse = Mapper.Map<CustomerPagedResponse>(pagedResult);

			// Then
			Assert.Null(pagedResponse.Previous);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(1, 0)]
		[InlineData(1, 1)]
		[InlineData(2, 1)]
		[InlineData(2, 2)]
		[InlineData(5, 3)]
		public void ShouldMapFromCustomerPagedResultToCustomerPagedResponseWithNullNextLink(
			int page, int lastPage)
		{
			// Given
			var pagedResult = MockCustomerPagedResult();
			pagedResult.Page = page;
			pagedResult.LastPage = lastPage;

			// When
			var pagedResponse = Mapper.Map<CustomerPagedResponse>(pagedResult);

			// Then
			Assert.Null(pagedResponse.Next);
		}

		#endregion

		#region CustomerCreateRequest - Customer

		[Theory]
		[ClassData(typeof(TotalPurchasesAmountData))]
		public void ShouldMapFromCustomerCreateRequestToCustomer(
			string totalPurchasesAmountText, decimal? totalPurchasesAmountNumber)
		{
			// Given
			var addressRequest = MockAddressRequest();
			var noteRequest = MockNoteRequest();

			var customerCreateRequest = MockCustomerCreateRequest();
			customerCreateRequest.TotalPurchasesAmount = totalPurchasesAmountText;
			customerCreateRequest.Addresses = new[] { addressRequest };
			customerCreateRequest.Notes = new[] { noteRequest };

			var address = Mapper.Map<Address>(addressRequest);
			var note = Mapper.Map<Note>(noteRequest);

			// When
			var customer = Mapper.Map<Customer>(customerCreateRequest);

			// Then
			Assert.Equal(0, customer.CustomerId);

			Assert.Equal(customerCreateRequest.FirstName, customer.FirstName);
			Assert.Equal(customerCreateRequest.LastName, customer.LastName);
			Assert.Equal(customerCreateRequest.PhoneNumber, customer.PhoneNumber);
			Assert.Equal(customerCreateRequest.Email, customer.Email);
			Assert.Equal(totalPurchasesAmountNumber, customer.TotalPurchasesAmount);

			var actualAddress = Assert.Single(customer.Addresses);
			Assert.True(AddressesEqualByValuesExcludingIds(address, actualAddress));

			var actualNote = Assert.Single(customer.Notes);
			Assert.True(NotesEqualByValuesExcludingIds(note, actualNote));
		}

		#endregion

		#region CustomerUpdateRequest - Customer

		[Theory]
		[ClassData(typeof(TotalPurchasesAmountData))]
		public void ShouldMapFromCustomerUpdateRequestToCustomer(
			string totalPurchasesAmountText, decimal? totalPurchasesAmountNumber)
		{
			// Given
			var customerUpdateRequest = MockCustomerUpdateRequest();
			customerUpdateRequest.TotalPurchasesAmount = totalPurchasesAmountText;

			// When
			var customer = Mapper.Map<Customer>(customerUpdateRequest);

			// Then
			Assert.Equal(0, customer.CustomerId);

			Assert.Equal(customerUpdateRequest.FirstName, customer.FirstName);
			Assert.Equal(customerUpdateRequest.LastName, customer.LastName);
			Assert.Equal(customerUpdateRequest.PhoneNumber, customer.PhoneNumber);
			Assert.Equal(customerUpdateRequest.Email, customer.Email);
			Assert.Equal(totalPurchasesAmountNumber, customer.TotalPurchasesAmount);

			Assert.Null(customer.Addresses);
			Assert.Null(customer.Notes);
		}

		#endregion

		#region Equals helpers

		private static bool AddressResponsesEqualByValues(
			AddressResponse item1, AddressResponse item2)
		{
			return
				item1.Self == item2.Self &&
				item1.Line == item2.Line &&
				item1.Line2 == item2.Line2 &&
				item1.Type == item2.Type &&
				item1.City == item2.City &&
				item1.PostalCode == item2.PostalCode &&
				item1.State == item2.State &&
				item1.Country == item2.Country;
		}

		private static bool AddressListResponsesEqualByValues(
			AddressListResponse item1, AddressListResponse item2)
		{
			AddressResponse inner1, inner2;

			for (int i = 0; i < item1.Items.Count(); i++)
			{
				inner1 = item1.Items.ElementAt(i);
				inner2 = item2.Items.ElementAt(i);

				if (AddressResponsesEqualByValues(inner1, inner2) == false)
				{
					return false;
				}
			}

			return item1.Self == item2.Self;
		}

		private static bool AddressesEqualByValuesExcludingIds(Address item1, Address item2)
		{
			return
				item1.Line == item2.Line &&
				item1.Line2 == item2.Line2 &&
				item1.Type == item2.Type &&
				item1.City == item2.City &&
				item1.PostalCode == item2.PostalCode &&
				item1.State == item2.State &&
				item1.Country == item2.Country;
		}

		private static bool NoteResponsesEqualByValues(NoteResponse item1, NoteResponse item2)
		{
			return
				item1.Self == item2.Self &&
				item1.Content == item2.Content;
		}

		private static bool NoteListResponsesEqualByValues(
			NoteListResponse item1, NoteListResponse item2)
		{
			NoteResponse inner1, inner2;

			for (int i = 0; i < item1.Items.Count(); i++)
			{
				inner1 = item1.Items.ElementAt(i);
				inner2 = item2.Items.ElementAt(i);

				if (NoteResponsesEqualByValues(inner1, inner2) == false)
				{
					return false;
				}
			}

			return item1.Self == item2.Self;
		}

		private static bool NotesEqualByValuesExcludingIds(Note item1, Note item2)
		{
			return item1.Content == item2.Content;
		}

		private static bool CustomerResponsesEqualByValues(
			CustomerResponse item1, CustomerResponse item2)
		{
			return
				item1.Self == item2.Self &&
				item1.FirstName == item2.FirstName &&
				item1.LastName == item2.LastName &&
				item1.Email == item2.Email &&
				item1.PhoneNumber == item2.PhoneNumber &&
				item1.TotalPurchasesAmount == item2.TotalPurchasesAmount &&

				AddressListResponsesEqualByValues(item1.Addresses, item2.Addresses) &&
				NoteListResponsesEqualByValues(item1.Notes, item2.Notes);
		}

		#endregion

		#region Mocks - Address

		public static Address MockAddress() => new()
		{
			AddressId = 2,
			CustomerId = 3,
			Line = "Line1",
			Line2 = "Line21",
			Type = AddressType.Shipping,
			City = "City1",
			PostalCode = "123456",
			State = "State1",
			Country = "United States"
		};

		public static AddressResponse MockAddressResponse() => new()
		{
			Self = "Self1",
			Line = "Line1",
			Line2 = "Line21",
			Type = "Shipping",
			City = "City1",
			PostalCode = "123456",
			State = "State1",
			Country = "United States"
		};

		public static AddressListResponse MockAddressListResponse() => new()
		{
			Self = "Self1",
			Items = new[] { MockAddressResponse() }
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

		#endregion

		#region Mocks - Note

		public static Note MockNote() => new()
		{
			NoteId = 1,
			CustomerId = 2,
			Content = "Content1"
		};

		public static NoteRequest MockNoteRequest() => new()
		{
			Content = "Content1"
		};

		public static NoteResponse MockNoteResponse() => new()
		{
			Self = "Self1",
			Content = "Content1"
		};

		public static NoteListResponse MockNoteListResponse() => new()
		{
			Self = "Self1",
			Items = new[] { MockNoteResponse() }
		};

		#endregion

		#region Mocks - Customer

		public static Customer MockCustomer() => new()
		{
			CustomerId = 6,
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@b.c",
			TotalPurchasesAmount = 666,
			Addresses = new[] { MockAddress() },
			Notes = new[] { MockNote() }
		};

		public static PagedResult<Customer> MockCustomerPagedResult() => new()
		{
			Page = 2,
			PageSize = 5,
			LastPage = 3,
			Items = new[] { MockCustomer() }
		};

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

		public static CustomerUpdateRequest MockCustomerUpdateRequest() => new()
		{
			FirstName = "FirstName1",
			LastName = "LastName1",
			PhoneNumber = "+123456789",
			Email = "a@b.c",
			TotalPurchasesAmount = "666"
		};

		#endregion
	}
}
