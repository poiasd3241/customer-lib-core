using System.Collections.Generic;

namespace CustomerLibCore.Api.Dtos.Customers
{
	public class CustomerPagedResponse : IPagedResponse<CustomerResponse>
	{
		public string Self { get; set; }
		public string Previous { get; set; }
		public string Next { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int LastPage { get; set; }
		public IEnumerable<CustomerResponse> Items { get; set; }
	}
}
