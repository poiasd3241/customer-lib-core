using CustomerLibCore.Api.Dtos.Addresses.Response;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Addresses.Response
{
	/// <summary>
	/// The fluent validator for <see cref="AddressListResponse"/> objects.
	/// </summary>
	public class AddressListResponseValidator : AddressListResponseBaseValidator
	{
		//public AddressListResponseValidator()
		//{
		//	Include(new ListResponseValidator<AddressResponse>(
		//		new AddressResponseValidator(), true));
		//}
		public AddressListResponseValidator() : base(areItemsRequired: true)
		{
		}
	}
}
