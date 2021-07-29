using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.Api.Dtos.Notes;

namespace CustomerLibCore.Api.Dtos.Customers
{
	public class CustomerCreateRequest : ICustomerBasicDetails
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string TotalPurchasesAmount { get; set; }
		public IEnumerable<AddressRequest> Addresses { get; set; }
		public IEnumerable<NoteRequest> Notes { get; set; }
	}
}
