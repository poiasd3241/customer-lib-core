using CustomerLibCore.Api.Dtos.Addresses.Response;

namespace CustomerLibCore.Api.Dtos.Validators.Addresses.Response
{
	/// <summary>
	/// The fluent validator for <see cref="AddressListResponse"/> objects.
	/// </summary>
	public class AddressListResponseValidator : AddressListResponseBaseValidator
	{
		public AddressListResponseValidator() : base(areItemsRequired: true)
		{
		}
	}
}
