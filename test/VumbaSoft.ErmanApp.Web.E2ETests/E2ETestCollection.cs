using Xunit;

namespace VumbaSoft.ErmanApp;

[CollectionDefinition(Name)]
public class E2ETestCollection : ICollectionFixture<PlaywrightFixture>
{
    public const string Name = "E2E";
}
