using System.Collections.Generic;

namespace CustomerLibCore.Api.DTOs
{
	public class CustomerDto
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string TotalPurchasesAmount { get; set; }
		public List<AddressDto> Addresses { get; set; }
		public List<NoteDto> Notes { get; set; }
	}
}
