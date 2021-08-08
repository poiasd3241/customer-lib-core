using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.Data.Entities
{
	[Table("Customers")]
	public class CustomerEntity : ICustomerDetails<decimal?>, IEntity<CustomerEntity>
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CustomerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public decimal? TotalPurchasesAmount { get; set; }

		public CustomerEntity Copy()
		{
			throw new NotImplementedException();
		}

		public bool EqualsByValueExcludingId(CustomerEntity customer2) =>
			customer2 is not null &&
			FirstName == customer2.FirstName &&
			LastName == customer2.LastName &&
			PhoneNumber == customer2.PhoneNumber &&
			Email == customer2.Email &&
			TotalPurchasesAmount == customer2.TotalPurchasesAmount;

		public bool EqualsByValue(CustomerEntity customer2) =>
			EqualsByValueExcludingId(customer2) &&
			CustomerId == customer2.CustomerId;
	}
}
