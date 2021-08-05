using System;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerLibCore.Domain.Enums;

namespace CustomerLibCore.Data.Entities
{
	[Serializable]
	public class AddressEntity : IEntity<AddressEntity>
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int AddressId { get; set; }
		public int CustomerId { get; set; }
		public string Line { get; set; }
		public string Line2 { get; set; }

		[Column("AddressTypeId")]
		public AddressType Type { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string State { get; set; }
		public string Country { get; set; }

		public AddressEntity Copy() => new()
		{
			AddressId = AddressId,
			CustomerId = CustomerId,
			Line = Line,
			Line2 = Line2,
			Type = Type,
			City = City,
			PostalCode = PostalCode,
			State = State,
			Country = Country
		};

		public bool EqualsByValue(AddressEntity address2) =>
			address2 is not null &&
			AddressId == address2.AddressId &&
			CustomerId == address2.CustomerId &&
			Line == address2.Line &&
			Line2 == address2.Line2 &&
			Type == address2.Type &&
			City == address2.City &&
			PostalCode == address2.PostalCode &&
			State == address2.State &&
			Country == address2.Country;
	}
}
