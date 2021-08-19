using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CustomerLibCore.TestHelpers
{
	public class AssertX
	{
		#region Public Methods

		public static void Contains<T>(IEnumerable<T> expected, IEnumerable<T> collection) =>
			Assert.True(ContainsInternal(expected, collection));
		public static void Unique<T>(IEnumerable<T> collection) =>
			Assert.True(UniqueInternal(collection));

		public static void DoesNotContain<T>(IEnumerable<T> expected, IEnumerable<T> collection) =>
			Assert.False(ContainsInternal(expected, collection));

		public static void DoesNotContain<T>(IEnumerable<T> expected, IEnumerable<T> collection,
			IEnumerable<T> distinctCollection)
		{
			Distinct(collection, distinctCollection);

			Assert.False(ContainsInternal(expected, distinctCollection));
		}

		public static void Distinct<T>(IEnumerable<T> collection, IEnumerable<T> distinctCollection)
		{
			Assert.True(UniqueInternal(distinctCollection));
			Assert.True(DistinctInternal(collection, distinctCollection));
		}

		public static void ContainsAny<T>(IEnumerable<T> expected, IEnumerable<T> collection) =>
			Assert.True(ContainsAnyInternal(expected, collection));

		public static void ContainsAny<T>(IEnumerable<T> expected,
			IEnumerable<T> collection, IEnumerable<T> distinctCollection)
		{
			Distinct(collection, distinctCollection);

			ContainsAny(expected, distinctCollection);
		}

		#endregion

		#region Private Methods

		private static bool ContainsInternal<T>(IEnumerable<T> expected, IEnumerable<T> collection)
		{
			foreach (var item in expected)
			{
				if (collection.Contains(item) == false)
				{
					return false;
				}
			}

			return true;
		}

		private static bool UniqueInternal<T>(IEnumerable<T> collection)
		{
			foreach (var item in collection)
			{
				if (collection.Where(x =>
					EqualityComparer<T>.Default.Equals(x, item)).Count() != 1)
				{
					return false;
				}
			}

			return true;
		}

		private static bool DistinctInternal<T>(
			IEnumerable<T> collection, IEnumerable<T> distinctCollection)
		{
			foreach (var item in collection)
			{
				if (distinctCollection.Where(x =>
					EqualityComparer<T>.Default.Equals(x, item)).Count() != 1)
				{
					return false;
				}
			}

			return true;
		}

		private static bool ContainsAnyInternal<T>(
			IEnumerable<T> expected, IEnumerable<T> collection)
		{
			foreach (var item in expected)
			{
				if (collection.Contains(item))
				{
					return false;
				}
			}

			return true;
		}

		#endregion
	}
}
