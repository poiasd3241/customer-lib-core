using Xunit;

namespace CustomerLibCore.Data.IntegrationTests.Repositories
{
    [CollectionDefinition(nameof(NotDbSafeResourceCollection), DisableParallelization = true)]
    public class NotDbSafeResourceCollection
    { }
}
