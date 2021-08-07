using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Notes.Response;
using CustomerLibCore.Api.Dtos.Validators.Notes.Response;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Notes
{
	public class NoteListResponseValidatorTest
	{
		#region Private members

		private static readonly NoteListResponseValidator _validator = new();

		#endregion

		#region Invalid property - Self

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.HrefLink))]
		public void ShouldInvalidateByBadSelf(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(NoteListResponse.Self);

			var notes = new NoteListResponseValidatorFixture().MockValid();
			notes.Self = propertyValue;

			// When
			var errors = _validator.ValidateProperty(notes, propertyName);

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		#endregion

		#region Invalid property - Items [IEnumerable]

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Common.Required))]
		public void ShouldInvalidateByBadItemsNull((string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(NoteListResponse.Items);

			var notes = new NoteListResponseValidatorFixture().MockValid();
			notes.Items = null;

			// When
			var errors = _validator.Validate(notes).Errors;

			// Then
			errors.AssertSinglePropertyInvalid(propertyName, errorMessages);
		}

		[Fact]
		public void ShouldInvalidateByBadItemsElement()
		{
			// Given
			var propertyName = nameof(NoteListResponse.Items);

			var (noteResponse, details) = new NoteResponseValidatorFixture()
				.MockInvalidWithDetails();

			var notes = new NoteListResponseValidatorFixture().MockValid();
			notes.Items = new[] { noteResponse };

			// When
			var errors = _validator.Validate(notes).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages($"{propertyName}[0]", details);
		}

		#endregion

		#region Full object

		[Fact]
		public void ShouldValidateFullObject()
		{
			// Given
			var notes = new NoteListResponseValidatorFixture().MockValid();

			// When
			var result = _validator.Validate(notes);

			// Then
			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (notes, details) = new NoteListResponseValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(notes).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class NoteListResponseValidatorFixture : IValidatorFixture<NoteListResponse>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="NoteListResponseValidator"/>).</returns>
		public NoteListResponse MockValid() => new()
		{
			Self = "Self1",
			Items = new[] { new NoteResponseValidatorFixture().MockValid() }
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="NoteListResponse.Self"/> = <see langword="null"/>;
		/// <br/>
		/// <see cref="NoteListResponse.Items"/> = 
		/// <see cref="NoteResponseValidatorFixture.MockInvalid"/>;
		/// <br/>
		/// (according to <see cref="NoteListResponseValidator"/>).</returns>
		public NoteListResponse MockInvalid()
		{
			var noteResponse = new NoteResponseValidatorFixture().MockInvalid();

			return new()
			{
				Self = null,
				Items = new[] { noteResponse }
			};
		}

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="NoteListResponseValidator"/>).</returns>
		public (NoteListResponse invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			IEnumerable<(string propertyName, string errorMessage)> details = new (string, string)[]
			{
				(nameof(NoteListResponse.Self), ValidationErrorMessages.REQUIRED),
			};

			var (_, invalidNoteResponseDetails) = new NoteResponseValidatorFixture()
				.MockInvalidWithDetails();

			foreach (var detail in invalidNoteResponseDetails)
			{
				details = details.AppendDetail($"{nameof(NoteListResponse.Items)}[0]", detail);
			}

			return (MockInvalid(), details);
		}
	}
}
