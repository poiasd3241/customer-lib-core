using System.Collections.Generic;
using CustomerLibCore.Data.Entities;
using CustomerLibCore.Data.Entities.Validators;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Data.Tests.Entities.Validators
{
	public class NoteEntityValidatorTest
	{
		#region Private members

		private static readonly NoteEntityValidator _validator = new();

		#endregion

		#region Invalid property - Content

		[Theory]
		[ClassData(typeof(TestHelpers.ValidatorTestData.Note.Content))]
		public void ShouldInvalidateByBadContent(
			string propertyValue, (string expected, string confirm) errorMessages)
		{
			// Given
			var propertyName = nameof(NoteEntity.Content);

			var note = new NoteEntityValidatorFixture().MockValid();
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
			var note = new NoteEntityValidatorFixture().MockValid();

			var result = _validator.Validate(note);

			Assert.True(result.IsValid);
		}

		[Fact]
		public void ShouldInvalidateFullObject()
		{
			// Given
			var (note, details) = new NoteEntityValidatorFixture()
				.MockInvalidWithDetails();

			// When
			var errors = _validator.Validate(note).Errors;

			// Then
			errors.AssertContainPropertyNamesAndErrorMessages(details);
		}

		#endregion
	}

	public class NoteEntityValidatorFixture : IValidatorFixture<NoteEntity>
	{
		/// <returns>The mocked object with valid properties 
		/// (according to <see cref="NoteEntityValidator"/>).</returns>
		public NoteEntity MockValid() => new()
		{
			Content = "Content1"
		};

		/// <returns>The mocked object with invalid properties:
		/// <br/>
		/// <see cref="NoteEntity.Content"/> = <see langword="null"/>;
		/// <br/>
		/// (according to <see cref="NoteEntityValidator"/>).</returns>
		public NoteEntity MockInvalid() => new()
		{
			Content = null
		};

		/// <returns>
		/// - invalidObject: <see cref="MockInvalid"/>;
		/// <br/>
		/// - details: values corresponding to all invalid properties of the object;
		/// <br/>
		/// (according to <see cref="NoteEntityValidator"/>).</returns>
		public (NoteEntity invalidObject,
			IEnumerable<(string propertyName, string errorMessage)> details)
			MockInvalidWithDetails()
		{
			var details = new (string, string)[]
			{
				(nameof(NoteEntity.Content), ValidationErrorMessages.REQUIRED)
			};

			return (MockInvalid(), details);
		}
	}
}
