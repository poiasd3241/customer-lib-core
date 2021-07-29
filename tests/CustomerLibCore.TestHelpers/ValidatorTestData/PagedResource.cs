using CustomerLibCore.Api.Dtos;
using CustomerLibCore.Business.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using static CustomerLibCore.TestHelpers.ValidatorTestData.ErrorMessages;
using Xunit;

namespace CustomerLibCore.TestHelpers.ValidatorTestData
{
	public class PagedResource
	{
		public class PageByItself : TheoryData<int, (string, string)>
		{
			public PageByItself()
			{
				Add(-1, NumberGreaterThan0Msg());
				Add(0, NumberGreaterThan0Msg());
			}
		}

		public class PageSizeByItself : TheoryData<int, (string, string)>
		{
			public PageSizeByItself()
			{
				Add(-1, NumberGreaterThan0Msg());
				Add(0, NumberGreaterThan0Msg());
			}
		}

		public class LastPageByItself : TheoryData<int, (string, string)>
		{
			public LastPageByItself()
			{
				Add(-1, NumberGreaterThan0Msg());
				Add(0, NumberGreaterThan0Msg());
			}
		}

		public class PageAgainstLastPage : TheoryData<(int, int), (string, string)>
		{
			public PageAgainstLastPage()
			{
				this.AddMany(new[]
				{
						(2, 1),
						(3, 1),
						(3, 2),
					}, ("must be less than or equal to the LastPage value",
					ValidationErrorMessages.NumberLessThanOrEqualToPropertyValue(
						nameof(IPagedResourceBase.LastPage))));
			}
		}
	}
}
