using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerLibCore.Domain.Models
{
	[Serializable]
	public class Customer : ICustomerDetails<decimal?>
	{
		public int CustomerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public decimal? TotalPurchasesAmount { get; set; }
		public IEnumerable<Address> Addresses { get; set; }
		public IEnumerable<Note> Notes { get; set; }

		//public override bool EqualsByValue(object customerToCompareTo)
		//{
		//	if (customerToCompareTo is null)
		//	{
		//		return false;
		//	}

		//	EnsureSameEntityType(customerToCompareTo);
		//	var customer = (Customer)customerToCompareTo;

		//	return
		//		CustomerId == customer.CustomerId &&
		//		PhoneNumber == customer.PhoneNumber &&
		//		Email == customer.Email &&
		//		TotalPurchasesAmount == customer.TotalPurchasesAmount &&
		//		Address.ListsEqualByValues(Addresses, customer.Addresses) &&
		//		Note.ListsEqualByValues(Notes, customer.Notes);
		//}
	}
}
