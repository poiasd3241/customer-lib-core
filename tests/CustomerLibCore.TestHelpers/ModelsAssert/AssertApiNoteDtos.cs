using CustomerLibCore.Api.Dtos.Notes.Request;
using CustomerLibCore.Api.Dtos.Validators.Notes.Request;
using CustomerLibCore.Domain.FluentValidation;
using Xunit;

namespace CustomerLibCore.TestHelpers.ModelsAssert
{
	public class AssertApiNoteDtos : IModelAssert<NoteRequest>

	{
		private readonly NoteRequestValidator _requestValidator = new();

		public void Meaningful(NoteRequest obj)
		{
			Assert.NotEqual(default, obj.Content);

			_requestValidator.Validate(obj).WithInternalValidationException();
		}
	}
}
