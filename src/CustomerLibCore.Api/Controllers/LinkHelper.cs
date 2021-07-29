namespace CustomerLibCore.Api.Controllers
{
	public class LinkHelper
	{
		private static readonly string _apiRoot = "/api";

		private static readonly string _customers = $"{_apiRoot}/customers";
		private static readonly string _customersPage =
			$"{_apiRoot}/customers?page={{0}}&pageSize={{1}}";

		private static readonly string _customer = $"{_customers}/{{0}}";

		private static readonly string _addresses = $"{_customers}/{{0}}/addresses";
		private static readonly string _address = $"{_customers}/{{0}}/addresses/{{1}}";

		private static readonly string _notes = $"{_customers}/{{0}}/notes";
		private static readonly string _note = $"{_customers}/{{0}}/notes/{{1}}";

		public static string CustomersPage(int page, int pageSize) =>
			string.Format(_customersPage, page, pageSize);

		public static string Customer(int customerId) =>
			string.Format(_customer, customerId);

		public static string Addresses(int customerId) =>
			string.Format(_addresses, customerId);
		public static string Address(int customerId, int addressId) =>
			string.Format(_address, customerId, addressId);

		public static string Notes(int customerId) =>
			string.Format(_notes, customerId);
		public static string Note(int customerId, int noteId) =>
			string.Format(_note, customerId, noteId);
	}
}
