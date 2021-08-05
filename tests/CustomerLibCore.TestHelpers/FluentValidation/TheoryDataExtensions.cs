using System;
using CustomerLibCore.Domain.Enums;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CustomerLibCore.TestHelpers.ValidatorTestData;

namespace CustomerLibCore.TestHelpers.FluentValidation
{
	public static class TheoryDataExtensions
	{
		public static void AddEmptyOrWhitespaceData(
			this TheoryData<string, (string, string)> theoryData)
		{
			var msg = ErrorMessages.EmptyOrWhitespaceMsg();

			var values = new string[]
			{
				"",
				" ",
				"  ",
			};

			theoryData.AddMany(values, msg);
		}

		public static void AddRequiredData<T>(
			this TheoryData<T, (string, string)> theoryData) where T : class
		{
			theoryData.Add(null, ErrorMessages.RequiredMsg());
		}

		public static void AddEmptyOrContainWhitespaceData(
			this TheoryData<string, (string, string)> theoryData)
		{
			var msg = ErrorMessages.EmptyOrContainWhitespaceMsg();

			var values = new string[]
			{
				"",
				" ",
				"  ",
				" a",
				"a ",
				" a ",
				"a a",
			};

			theoryData.AddMany(values, msg);
		}

		public static void AddUnknownEnumTypeByNameData<T>(
			this TheoryData<string, (string, string)> theoryData,
			string[] additionalInvalidValuesToCheck = null)
		{
			if (typeof(T).IsEnum == false)
			{
				throw new ArgumentException($"{nameof(T)} must be an enum");
			}

			var validNames = Enum.GetNames(typeof(T));

			var validIntValues = ((IEnumerable<T>)Enum.GetValues(typeof(T)))
				.Select(x => Convert.ToInt32(x));

			var msg = ErrorMessages.UnknownTypeMsg();

			IEnumerable<string> values = new string[]
			{
				"",
				" ",
				"  ",
			};

			foreach (var validName in validNames)
			{
				values.AppendForTrimProtection(validName);
			}

			foreach (var validIntValue in validIntValues)
			{
				values = values.Append(validIntValue.ToString());

				values.AppendForTrimProtection(validIntValue.ToString());
			}

			if (additionalInvalidValuesToCheck is not null)
			{
				if (additionalInvalidValuesToCheck.Length == 0)
				{
					throw new ArgumentException($"{nameof(additionalInvalidValuesToCheck)} " +
						$"must be either null or contain at least 1 element",
						nameof(additionalInvalidValuesToCheck));
				}

				foreach (var addtionalValue in additionalInvalidValuesToCheck)
				{
					values = values.Append(addtionalValue);
				}
			}

			theoryData.AddMany(values, msg);
		}

		private static IEnumerable<string> AppendForTrimProtection(
			this IEnumerable<string> values, string newValue)
		{
			values = values.Append($" {newValue}");
			values = values.Append($"{newValue} ");
			values = values.Append($" {newValue} ");

			return values;

		}
		#region Valid

		public static void AddUnknownEnumTypeDataByNamesValid<T>(
			this TheoryData<string, (string, string)> theoryData)
		{
			if (typeof(T).IsEnum == false)
			{
				throw new ArgumentException($"{nameof(T)} must be an enum");
			}

			var validNames = Enum.GetNames(typeof(T));

			var msg = ErrorMessages.UnknownTypeMsg();

			IEnumerable<string> values = Array.Empty<string>();

			foreach (var validName in validNames)
			{
				values = values.Append(validName);
			}

			theoryData.AddMany(values, msg);
		}

		#endregion

		public static void AddMany<T>(this TheoryData<T, (string, string)> theoryData,
			IEnumerable<T> values, (string expected, string confirm) errorMessage)
		{
			foreach (var value in values)
			{
				theoryData.Add(value, errorMessage);
			}
		}
	}
}
