using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.TestHelpers.FluentValidation
{
	public static class ValidationFailureExtensions
	{
		/// <summary>
		/// Asserts that the collections are equal by count and the elements' fields:
		/// <see cref="ValidationFailure.PropertyName"/> and 
		/// <see cref="ValidationFailure.ErrorMessage"/>.
		/// The element order in the collections doesn't matter.
		/// </summary>
		/// <param name="expected">The first collection.</param>
		/// <param name="actual">The second collection.</param>
		/// <param name="expectedCount">The expected element count in each collection.</param>
		public static void AssertEqualByPropertyNameAndErrorMessage
			(this IEnumerable<ValidationFailure> expected, IEnumerable<ValidationFailure> actual,
			int expectedCount)
		{
			Assert.Equal(expectedCount, expected.Count());
			Assert.Equal(expectedCount, actual.Count());

			var sortedExpected = expected.OrderBy(t => t.PropertyName);
			var sortedActual = actual.OrderBy(t => t.PropertyName);

			ValidationFailure sortedExpectedItem, sortedActualItem;

			for (int i = 0; i < expectedCount; i++)
			{
				sortedExpectedItem = sortedExpected.ElementAt(i);
				sortedActualItem = sortedActual.ElementAt(i);

				Assert.Equal(sortedExpectedItem.PropertyName, sortedActualItem.PropertyName);
				Assert.Equal(sortedExpectedItem.ErrorMessage, sortedActualItem.ErrorMessage);
			}
		}

		/// <summary>
		/// Asserts that the collection contains the elements 
		/// with the specified <see cref="ValidationFailure.PropertyName"/> values.
		/// </summary>
		/// <param name="failures">The collection.</param>
		/// <param name="propertyNames">The property names the collection's elements must contain.
		/// </param>
		public static void AssertContainPropertyNames(this IEnumerable<ValidationFailure> failures,
			string[] propertyNames)
		{
			var failurePropertyNames = failures.Select(f => f.PropertyName);

			foreach (var propertyName in propertyNames)
			{
				Assert.Contains(propertyName, failurePropertyNames);
			}
		}

		/// <summary>
		/// Asserts that the collection contains the elements 
		/// with the specified <see cref="ValidationFailure.PropertyName"/> and
		/// <see cref="ValidationFailure.ErrorMessage"/> values.
		/// </summary>
		/// <param name="failures">The collection.</param>
		/// <param name="expectedDetails">The (property name, error message) pairs 
		/// the collection must contain.</param>
		/// </param>
		public static void AssertContainPropertyNamesAndErrorMessages(
			this IEnumerable<ValidationFailure> failures,
			IEnumerable<(string propertyName, string errorMessage)> expectedDetails)
		{
			var failurePropertyNames = failures.Select(f => f.PropertyName);

			foreach (var (propertyName, errorMessage) in expectedDetails)
			{
				var failure = failures.First(f => f.PropertyName == propertyName);

				Assert.Equal(errorMessage, failure.ErrorMessage);
			}
		}

		/// <summary>
		/// Asserts that the collection contains the elements 
		/// with the specified <see cref="ValidationFailure.PropertyName"/> and
		/// <see cref="ValidationFailure.ErrorMessage"/> values.
		/// </summary>
		/// <param name="failures">The collection.</param>
		/// <param name="expectedDetails">The (property name, error message) pairs 
		/// the collection must contain (without parent property name prepended).</param>
		/// <param name="parentPropertyName">The name of the parent property to prepend
		/// to all property names in <paramref name="expectedDetails"/>.</param>
		public static void AssertContainPropertyNamesAndErrorMessages(
			this IEnumerable<ValidationFailure> failures, string parentPropertyName,
			IEnumerable<(string propertyName, string errorMessage)> expectedDetails)
		{
			var expectedDetailsAfterPrepend = expectedDetails.Select(e =>
				($"{parentPropertyName}.{e.propertyName}", e.errorMessage));

			failures.AssertContainPropertyNamesAndErrorMessages(
				expectedDetailsAfterPrepend);
		}

		/// <summary>
		/// Asserts that the collection contains a single error 
		/// with the specified <see cref="ValidationFailure.PropertyName"/> 
		/// (<paramref name="propertyName"/>) and 
		/// <see cref="ValidationFailure.ErrorMessage"/> (<paramref name="expectedErrorMessage"/>).
		/// <br/>
		/// Asserts that the <paramref name="confirmErrorMessage"/> and 
		/// <paramref name="confirmErrorMessage"/> are equal.
		/// </summary>
		/// <param name="errors">The collection.</param>
		/// <param name="propertyName">The invalid property's name.</param>
		/// <param name="expectedErrorMessage">The expected validation error message.</param>
		/// <param name="confirmErrorMessage">The validation error message to confirm.</param>
		public static void AssertSinglePropertyInvalid(this IEnumerable<ValidationFailure> errors,
			string propertyName, (string expected, string confirm) errorMessages)
		{
			var failure = Assert.Single(errors);
			Assert.Equal(propertyName, failure.PropertyName);
			Assert.Equal(errorMessages.expected, failure.ErrorMessage);
			Assert.Equal(errorMessages.expected, errorMessages.confirm);
		}
	}
}
