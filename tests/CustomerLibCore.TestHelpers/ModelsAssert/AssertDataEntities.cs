using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using CustomerLibCore.Domain.FluentValidation;
using Xunit;

namespace CustomerLibCore.TestHelpers.ModelsAssert
{
	public class AssertDataEntities : IModelIdsAssert<AddressEntity>, IModelIdsAssert<NoteEntity>,
		IModelIdsAssert<CustomerEntity>
	{
		private readonly AddressEntityValidator _addressValidator = new();
		private readonly NoteEntityValidator _noteValidator = new();
		private readonly CustomerEntityValidator _customerValidator = new();

		public void Meaningful(AddressEntity obj)
		{
			Assert.NotEqual(default, obj.Line);
			Assert.NotEqual(default, obj.Line2);
			Assert.NotEqual(default, obj.Type);
			Assert.NotEqual(default, obj.City);
			Assert.NotEqual(default, obj.PostalCode);
			Assert.NotEqual(default, obj.State);
			Assert.NotEqual(default, obj.Country);

			// string
			AssertX.Unique(new[]
			{
				obj.Line,
				obj.Line2,
				obj.City,
				obj.PostalCode,
				obj.State,
				obj.Country
			});

			_addressValidator.Validate(obj).WithInternalValidationException();
		}

		public void MeaningfulWithIds(AddressEntity obj)
		{
			Meaningful(obj);

			Assert.NotEqual(default, obj.AddressId);
			Assert.NotEqual(default, obj.CustomerId);

			AssertX.Unique(new[]
			{
				obj.AddressId,
				obj.CustomerId
			});
		}

		public void Meaningful(NoteEntity obj)
		{
			Assert.NotEqual(default, obj.Content);

			_noteValidator.Validate(obj).WithInternalValidationException();
		}

		public void MeaningfulWithIds(NoteEntity obj)
		{
			Meaningful(obj);

			Assert.NotEqual(default, obj.NoteId);
			Assert.NotEqual(default, obj.CustomerId);

			AssertX.Unique(new[]
			{
				obj.NoteId,
				obj.CustomerId
			});
		}

		public void Meaningful(CustomerEntity obj)
		{
			Assert.NotEqual(default, obj.FirstName);
			Assert.NotEqual(default, obj.LastName);
			Assert.NotEqual(default, obj.PhoneNumber);
			Assert.NotEqual(default, obj.Email);
			Assert.NotEqual(default, obj.TotalPurchasesAmount);

			// string
			AssertX.Unique(new[]
			{
				obj.FirstName,
				obj.LastName,
				obj.PhoneNumber,
				obj.Email,
			});

			_customerValidator.Validate(obj).WithInternalValidationException();
		}

		public void MeaningfulWithIds(CustomerEntity obj)
		{
			Meaningful(obj);

			Assert.NotEqual(default, obj.CustomerId);
		}
	}
}
