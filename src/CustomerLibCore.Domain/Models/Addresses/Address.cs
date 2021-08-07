using System;
using CustomerLibCore.Domain.Enums;

namespace CustomerLibCore.Domain.Models
{
	public class Address : IAddressDetails<AddressType>
	{
		public int AddressId { get; set; }
		public int CustomerId { get; set; }
		public string Line { get; set; }
		public string Line2 { get; set; }
		public AddressType Type { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string State { get; set; }
		public string Country { get; set; }

		//public override bool EqualsByValue(object addressToCompareTo)
		//{
		//	if (addressToCompareTo is null)
		//	{
		//		return false;
		//	}

		//	EnsureSameEntityType(addressToCompareTo);
		//	var address = (Address)addressToCompareTo;

		//	return
		//		AddressId == address.AddressId &&
		//		CustomerId == address.CustomerId &&
		//		Line == address.Line &&
		//		Line2 == address.Line2 &&
		//		Type == address.Type &&
		//		City == address.City &&
		//		PostalCode == address.PostalCode &&
		//		State == address.State &&
		//		Country == address.Country;
		//}

		//public static bool ListsEqualByValues(
		//	IEnumerable<Address> list1, IEnumerable<Address> list2) =>
		//		EntitiesHelper.ListsEqualByValues(list1, list2);

		//public Address Copy() => new()
		//{
		//	AddressId = AddressId,
		//	CustomerId = CustomerId,
		//	Line = Line,
		//	Line2 = Line2,
		//	Type = Type,
		//	City = City,
		//	PostalCode = PostalCode,
		//	State = State,
		//	Country = Country
		//};

	}
}
