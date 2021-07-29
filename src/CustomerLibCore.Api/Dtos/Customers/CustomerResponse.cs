using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.Api.Dtos.Notes;

namespace CustomerLibCore.Api.Dtos.Customers
{
	public class CustomerResponse : IResponse, ICustomerBasicDetails
	{
		public string Self { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string TotalPurchasesAmount { get; set; }
		public AddressListResponse Addresses { get; set; }
		public NoteListResponse Notes { get; set; }
	}
}
