using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Domain.Tests.Models.Validators
{
	public class NoteValidatorTest
	{
		#region Private members

		private static readonly NoteValidator _validator = new();

		#endregion

		#region Invalid property - Content

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Note.Content))]
		public void ShouldInvalidateByBadContent(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(Note.Content);

			var note = new NoteValidatorFixture().MockValid();
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
			var note = new NoteValidatorFixture().MockValid();

			var result = _validator.Validate(note);

			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (note, details) = new NoteValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(note).Errors;

			// Then
			Assert.Equal(details.Count(), errors.Count);

			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class NoteValidatorFixture : IValidatorFixture<Note>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="NoteValidator"/>).</returns>
		public Note MockValid() => new()
		{
			Content = "Content1"
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="Note.Content"/> = <see langword="null"/>;
		/// <br/>
		/// (according to <see cref="NoteValidator"/>).</returns>
		public Note MockInvalid() => new()
		{
			Content = null
		};

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="NoteValidator"/>).</returns>
		public (Note invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(Note.Content), ValidationErrorMessages.REQUIRED)
			};

			return (MockInvalid(), details);
		}
	}
}
