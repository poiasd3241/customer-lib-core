using Xunit;

namespace CustomerLibCore.Data.IntegrationTests
{
	[CollectionDefinition(nameof(NotDbSafeResourceCollection), DisableParallelization = true)]
	public class NotDbSafeResourceCollection
	{ }
}
