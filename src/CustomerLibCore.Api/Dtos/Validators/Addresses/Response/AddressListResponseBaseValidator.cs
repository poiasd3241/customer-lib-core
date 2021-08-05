using CustomerLibCore.Api.Dtos.Addresses.Response;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Addresses.Response
{
	/// <summary>
	/// The fluent validator for <see cref="AddressListResponse"/> objects.
	/// </summary>
	public class AddressListResponseBaseValidator : AbstractValidator<AddressListResponse>
	{
		public AddressListResponseBaseValidator(bool areItemsRequired)
		{
			Include(new ListResponseValidator<AddressResponse>(
				new AddressResponseValidator(), areItemsRequired));
		}
	}
}
