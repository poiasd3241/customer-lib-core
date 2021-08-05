namespace CustomerLibCore.Api.Dtos.Addresses.Request
{
	public class AddressRequest : IAddressDetails
	{
		public string Line { get; set; }
		public string Line2 { get; set; }
		public string Type { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
	}
}
