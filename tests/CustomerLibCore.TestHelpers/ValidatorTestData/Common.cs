using System.Collections.Generic;
using CustomerLibCore.Business.Localization;
using CustomerLibCore.TestHelpers.FluentValidation;
using static CustomerLibCore.TestHelpers.ValidatorTestData.ErrorMessages;
using Xunit;

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
					}, ("cannot have value", ValidationErrorMessages.CANNOT_HAVE_VALUE));
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

		public class CollectionRequiredNotEmpty<T> : TheoryData<List<T>, (string, string)>
		{
			public CollectionRequiredNotEmpty()
			{
				Add(null, CollectionRequiredNotEmptyMsg());
				Add(new(), CollectionRequiredNotEmptyMsg());
			}
		}
	}
}
