using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Request;
using CustomerLibCore.Domain.FluentValidation;
using Xunit;

namespace CustomerLibCore.TestHelpers.ModelsAssert
{
	public class AssertApiAddressDtos : IModelAssert<AddressRequest>

	{
		private readonly AddressRequestValidator _requestValidator = new();

		public void Meaningful(AddressRequest obj)
		{
			Assert.NotEqual(default, obj.Line);
			Assert.NotEqual(default, obj.Line2);
			Assert.NotEqual(default, obj.Type);
			Assert.NotEqual(default, obj.City);
			Assert.NotEqual(default, obj.PostalCode);
			Assert.NotEqual(default, obj.State);
			Assert.NotEqual(default, obj.Country);

			AssertX.Unique(new[]
			{
				obj.Line,
				obj.Line2,
				obj.Type,
				obj.City,
				obj.PostalCode,
				obj.State,
				obj.Country
			});

			_requestValidator.Validate(obj).WithInternalValidationException();
		}
	}
}
