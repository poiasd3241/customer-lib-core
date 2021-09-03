using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Api.Dtos.Validators.Customers.Request;
using CustomerLibCore.Domain.FluentValidation;
using Xunit;

namespace CustomerLibCore.TestHelpers.ModelsAssert
{
	public class AssertApiCustomerDtos : IModelAssert<CustomerCreateRequest>,
		IModelAssert<CustomerEditRequest>

	{
		private readonly CustomerCreateRequestValidator _createRequestValidator = new();
		private readonly CustomerEditRequestValidator _editRequestValidator = new();

		private readonly AssertApiAddressDtos _addressAssert = new();
		private readonly AssertApiNoteDtos _noteAssert = new();

		public void Meaningful(CustomerCreateRequest obj)
		{
			MeaningfulRequestDetails(obj);

			Assert.NotEqual(default, obj.Addresses);
			Assert.NotEqual(default, obj.Notes);

			foreach (var address in obj.Addresses)
			{
				_addressAssert.Meaningful(address);
			}

			foreach (var note in obj.Notes)
			{
				_noteAssert.Meaningful(note);
			}

			_createRequestValidator.Validate(obj).WithInternalValidationException();
		}

		public void Meaningful(CustomerEditRequest obj)
		{
			MeaningfulRequestDetails(obj);

			_editRequestValidator.Validate(obj).WithInternalValidationException();
		}

		#region Private Methods

		private static void MeaningfulRequestDetails(IDtoCustomerDetails obj)
		{
			Assert.NotEqual(default, obj.FirstName);
			Assert.NotEqual(default, obj.LastName);
			Assert.NotEqual(default, obj.PhoneNumber);
			Assert.NotEqual(default, obj.Email);
			Assert.NotEqual(default, obj.TotalPurchasesAmount);

			AssertX.Unique(new[]
			{
				obj.FirstName,
				obj.LastName,
				obj.PhoneNumber,
				obj.Email,
				obj.TotalPurchasesAmount
			});
		}

		#endregion
	}
}
