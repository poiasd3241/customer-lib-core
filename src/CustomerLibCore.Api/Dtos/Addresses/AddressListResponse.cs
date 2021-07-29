﻿using System.Collections.Generic;

namespace CustomerLibCore.Api.Dtos.Addresses
{
	public class AddressListResponse : IListResponse<AddressResponse>
	{
		public string Self { get; set; }
		public IEnumerable<AddressResponse> Items { get; set; }
	}
}
