using System.Collections.Generic;
using CustomerLibCore.Domain.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using Xunit;
using static CustomerLibCore.TestHelpers.ValidatorTestData.ErrorMessages;

namespace CustomerLibCore.TestHelpers.ValidatorTestData
{
	public class Common
	{
		public class Required : TheoryData<(string, string)>
		{
			public Required()
			{
				Add(RequiredMsg());
			}
		}

		public class CannotHaveValue
		{
			public class String : TheoryData<string, (string, string)>
			{
				public String()
				{
					this.AddMany(new[]
					{
						"",
						" ",
						"  ",
						" a",
						"a ",
						" a ",
						"a a",
						"whatever",
						"1",
					}, ("cannot have value", Domain.Localization.ErrorMessages.CANNOT_HAVE_VALUE));
				}
			}
		}

		public class HrefLink : TheoryData<string, (string, string)>
		{
			public HrefLink()
			{
				this.AddRequiredData();
				this.AddEmptyOrContainWhitespaceData();
			}
		}

		public class CollectionNotEmpty<T> : TheoryData<List<T>, (string, string)>
		{
			public CollectionNotEmpty()
			{
				Add(null, CollectionNotEmptyMsg());
				Add(new(), CollectionNotEmptyMsg());
			}
		}
	}
}
