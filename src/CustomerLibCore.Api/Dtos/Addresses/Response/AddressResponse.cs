namespace CustomerLibCore.Api.Dtos.Addresses.Response
{
	public class AddressResponse : IResponse, IDtoAddressDetails
	{
		public string Self { get; set; }
		public string Line { get; set; }
		public string Line2 { get; set; }
		public string Type { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
	}
}
