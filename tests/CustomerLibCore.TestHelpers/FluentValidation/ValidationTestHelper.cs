using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Xunit;

namespace CustomerLibCore.TestHelpers.FluentValidation
{
	public class ValidationTestHelper
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
		public static void AssertValidationFailuresEqualByPropertyNameAndErrorMessage
			(IEnumerable<ValidationFailure> expected, IEnumerable<ValidationFailure> actual,
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
		public static void AssertValidationFailuresContainPropertyNames
			(IEnumerable<ValidationFailure> failures, string[] propertyNames)
		{
			var failurePropertyNames = failures.Select(f => f.PropertyName);

			foreach (var propertyName in propertyNames)
			{
				Assert.Contains(propertyName, failurePropertyNames);
			}
		}
	}
}
