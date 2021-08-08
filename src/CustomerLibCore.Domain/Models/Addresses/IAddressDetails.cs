namespace CustomerLibCore.Domain.Models
{
	/// <summary>
	/// The address object base.
	/// </summary>
	/// <typeparam name="TType">The type to use for the property <see cref="Type"/>.</typeparam>
	public interface IAddressDetails<TType>
	{
		public string Line { get; set; }
		public string Line2 { get; set; }
		public TType Type { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
	}
}
