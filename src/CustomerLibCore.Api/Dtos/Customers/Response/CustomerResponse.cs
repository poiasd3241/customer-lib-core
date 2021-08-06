﻿using CustomerLibCore.Api.Dtos.Addresses.Response;
using CustomerLibCore.Api.Dtos.Notes.Response;

namespace CustomerLibCore.Api.Dtos.Customers.Response
{
	public class CustomerResponse : IResponse, IDtoCustomerDetails
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
