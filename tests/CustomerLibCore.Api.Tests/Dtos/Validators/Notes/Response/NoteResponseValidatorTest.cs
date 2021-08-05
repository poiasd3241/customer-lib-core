using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Api.Dtos.Notes.Response;
using CustomerLibCore.Api.Dtos.Validators.Notes;
using CustomerLibCore.Api.Dtos.Validators.Notes.Response;
using CustomerLibCore.Domain.Enums;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;

namespace CustomerLibCore.Api.Tests.Dtos.Validators.Notes
{
    public class NoteResponseValidatorTest
    {
        #region Private members

        private static readonly NoteResponseValidator _validator = new();

        #endregion

        #region Invalid property - Self

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Common.HrefLink))]
        public void ShouldInvalidateByBadSelf(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            // Given
            var propertyName = nameof(NoteResponse.Self);

            var note = new NoteResponseValidatorFixture().MockValid();
            note.Self = propertyValue;

            // When
            var errors = _validator.ValidateProperty(note, propertyName);

            // Then
            errors.AssertSinglePropertyInvalid(propertyName,
                errorMessages);
        }

        #endregion

        #region Invalid property - Content

        [Theory]
        [ClassData(typeof(TestHelpers.ValidatorTestData.Note.Content))]
        public void ShouldInvalidateByBadContent(
            string propertyValue, (string expected, string confirm) errorMessages)
        {
            // Given
            var propertyName = nameof(NoteResponse.Content);

            var note = new NoteResponseValidatorFixture().MockValid();
            note.Content = propertyValue;

            // When
            var errors = _validator.ValidateProperty(note, propertyName);

            // Then
            errors.AssertSinglePropertyInvalid(
                propertyName, errorMessages);
        }

        #endregion

        #region Full object

        [Fact]
        public void ShouldValidateFullObject()
        {
            var note = new NoteResponseValidatorFixture().MockValid();

            var result = _validator.Validate(note);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldInvalidateFullObject()
        {
            // Given
            var (note, details) = new NoteResponseValidatorFixture()
                .MockInvalidWithDetails();

            // When
            var errors = _validator.Validate(note).Errors;

            // Then
            Assert.Equal(details.Count(), errors.Count);

            errors.AssertContainPropertyNamesAndErrorMessages(details);
        }

        #endregion
    }

    public class NoteResponseValidatorFixture : IValidatorFixture<NoteResponse>
    {
        /// <returns>The mocked object with valid properties 
        /// (according to <see cref="NoteResponseValidator"/>).</returns>
        public NoteResponse MockValid() => new()
        {
            Self = "Self1",
            Content = "Content1"
        };

        /// <returns>The mocked object with invalid properties:
        /// <br/>
        /// <see cref="NoteResponse.Self"/> = <see langword="null"/>,
        /// <br/>
        /// <see cref="NoteResponse.Content"/> = <see langword="null"/>
        /// <br/>
        /// (according to <see cref="NoteResponseValidator"/>).</returns>
        public NoteResponse MockInvalid() => new()
        {
            Self = null,
            Content = null
        };

        /// <returns>
        /// invalidObject: <see cref="MockInvalid"/>
        /// <br/>
        /// details: values corresponding to all invalid properties of the object
        /// <br/>
        /// (according to <see cref="NoteResponseValidator"/>).</returns>
        public (NoteResponse invalidObject,
            IEnumerable<(string propertyName, string errorMessage)> details)
            MockInvalidWithDetails()
        {
            var details = new (string, string)[]
            {
                (nameof(NoteResponse.Self), ValidationErrorMessages.REQUIRED),
                (nameof(NoteResponse.Content), ValidationErrorMessages.REQUIRED)
            };

            return (MockInvalid(), details);
        }
    }
}
