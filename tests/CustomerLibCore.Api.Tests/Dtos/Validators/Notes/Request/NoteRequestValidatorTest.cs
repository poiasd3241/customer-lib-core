using System.Collections.Generic;
using CustomerLibCore.Api.Dtos.Notes.Request;
using CustomerLibCore.Api.Dtos.Validators.Notes.Request;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Notes
{
	public class NoteRequestValidatorTest
	{
		#region Private members

		private static readonly NoteRequestValidator _validator = new();

		#endregion

		#region Invalid property - Content

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Note.Content))]
		public void ShouldInvalidateByBadContent(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(NoteRequest.Content);

			var note = new NoteRequestValidatorFixture().MockValid();
			note.Content = propertyValue;

			// When
			var errors = _validator.ValidateProperty(note, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObject()
		{
			var note = new NoteRequestValidatorFixture().MockValid();

			var result = _validator.Validate(note);

			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (note, details) = new NoteRequestValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(note).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class NoteRequestValidatorFixture : IValidatorFixture<NoteRequest>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="NoteRequestValidator"/>).</returns>
		public NoteRequest MockValid() => new()
		{
			Content = "Content1"
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="NoteRequest.Content"/> = <see langword="null"/>;
		/// <br/>
		/// (according to <see cref="NoteRequestValidator"/>).</returns>
		public NoteRequest MockInvalid() => new()
		{
			Content = null
		};

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="NoteRequestValidator"/>).</returns>
		public (NoteRequest invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(NoteRequest.Content), ValidationErrorMessages.REQUIRED)
			};

			return (MockInvalid(), details);
		}
	}
}
