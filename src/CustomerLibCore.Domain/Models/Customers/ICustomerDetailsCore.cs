namespace CustomerLibCore.Domain.Models
{
	/// <summary>
	/// The customer object base core.
	/// </summary>
	public interface ICustomerDetailsCore
	{
		string FirstName { get; set; }
		string LastName { get; set; }
		string PhoneNumber { get; set; }
		string Email { get; set; }
	}
}
