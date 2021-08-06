using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Notes.Request;

namespace CustomerLibCore.Api.Dtos.Customers.Request
{
	public class CustomerCreateRequest : IDtoCustomerDetails
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
