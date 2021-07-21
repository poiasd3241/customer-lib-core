using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Api.DTOs.Validators;
using CustomerLibCore.Business.Localization;
using FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.DTOs.Validators
{
	public class NoteDtoValidatorTest
	{
		#region Private members

		private static readonly NoteDtoValidator _noteDtoValidator = new();

		#endregion

		#region Invalid property - Content

		private class InvalidContentData : TheoryData<string, string, string>
		{
			public InvalidContentData()
			{
				Add(null, "required", ValidationErrorMessages.REQUIRED);
				Add("", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(" ", "cannot be empty or whitespace",
					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
				Add(new('a', 1001), "max 1000 characters",
					ValidationErrorMessages.TextMaxLength(1000));
			}
		}

		[Theory]
		[ClassData(typeof(InvalidContentData))]
		public void ShouldInvalidateByBadContent(
			string content, string expectedErrorMessage, string confirmErrorMessage)
		{
			var invalidPropertyName = nameof(NoteDto.Content);
			var note = NoteDtoValidatorFixture.MockNoteDto();
			note.Content = content;

			var errors = _noteDtoValidator.Validate(note, options =>
				options.IncludeProperties(invalidPropertyName)).Errors;

			var error = Assert.Single(errors);
			Assert.Equal(invalidPropertyName, error.PropertyName);
			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
		}

		#endregion

		#region All properties

		[Fact]
		public void ShouldValidateNoteDtoAllProperties()
		{
			var note = NoteDtoValidatorFixture.MockNoteDto();

			var result = _noteDtoValidator.Validate(note);

			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateNoteDtoByAllProperties()
		{
			// Given
			var note = NoteDtoValidatorFixture.MockNoteDto();
			note.Content = null;

			// When
			var errors = _noteDtoValidator.Validate(note).Errors;

			// Then
			var error = Assert.Single(errors);
			Assert.Equal(nameof(NoteDto.Content), error.PropertyName);
			Assert.Equal(ValidationErrorMessages.REQUIRED, error.ErrorMessage);
		}

		#endregion
	}

	public class NoteDtoValidatorFixture
	{
		/// <returns>The mocked note DTO with valid properties 
		/// (according to <see cref="NoteDtoValidator"/>).</returns>
		public static NoteDto MockNoteDto() => new()
		{
			Content = "text"
		};
	}
}
