//using CustomerLibCore.Business.Entities;
//using CustomerLibCore.Business.Localization;
//using CustomerLibCore.Business.Validators;
//using FluentValidation;
//using Xunit;
//namespace CustomerLibCore.Business.Tests.Validators
//{
//	public class NoteValidatorTest
//	{
//		#region Private members

//		private static readonly NoteValidator _noteValidator = new();

//		#endregion

//		#region Invalid property - Content

//		private class InvalidContentData : TheoryData<string, string, string>
//		{
//			public InvalidContentData()
//			{
//				Add(null, "required", ValidationErrorMessages.REQUIRED);
//				Add("", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(" ", "cannot be empty or whitespace",
//					ValidationErrorMessages.TEXT_EMPTY_OR_WHITESPACE);
//				Add(new('a', 1001), "max 1000 characters",
//					ValidationErrorMessages.TextMaxLength(1000));
//			}
//		}

//		[Theory]
//		[ClassData(typeof(InvalidContentData))]
//		public void ShouldInvalidateByBadContent(
//			string content, (string expected, string confirm) errorMessage)
//		{
//			var propertyName = nameof(Note.Content);
//			var note = NoteValidatorFixture.MockNote();
//			note.Content = content;

//			var errors = _noteValidator.Validate(note, options =>
//				options.IncludeProperties(propertyName)).Errors;

//			var error = Assert.Single(errors);
//			Assert.Equal(propertyName, error.PropertyName);
//			Assert.Equal(expectedErrorMessage, error.ErrorMessage);
//			Assert.Equal(expectedErrorMessage, confirmErrorMessage);
//		}

//		#endregion

//		#region All properties

//		[Fact]
//		public void ShouldValidateNoteAllProperties()
//		{
//			var note = NoteValidatorFixture.MockNote();

//			var result = _noteValidator.Validate(note);

//			Assert.True(result.IsValid);
//		}

//		[Fact]
//		public void ShouldInvalidateNoteByAllProperties()
//		{
//			// Given
//			var note = NoteValidatorFixture.MockNote();
//			note.Content = null;

//			// When
//			var errors = _noteValidator.Validate(note).Errors;

//			// Then
//			var error = Assert.Single(errors);
//			Assert.Equal(nameof(Note.Content), error.PropertyName);
//			Assert.Equal(ValidationErrorMessages.REQUIRED, error.ErrorMessage);
//		}

//		#endregion
//	}

//	public class NoteValidatorFixture
//	{
//		/// <returns>The mocked note with valid properties 
//		/// (according to <see cref="NoteValidator"/>).</returns>
//		public static Note MockNote() => new()
//		{
//			Content = "text"
//		};
//	}
//}
