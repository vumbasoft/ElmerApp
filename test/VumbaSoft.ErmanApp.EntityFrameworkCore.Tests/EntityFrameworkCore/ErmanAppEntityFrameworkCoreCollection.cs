using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore;

[CollectionDefinition(ErmanAppTestConsts.CollectionDefinitionName)]
public class ErmanAppEntityFrameworkCoreCollection : ICollectionFixture<ErmanAppEntityFrameworkCoreFixture>
{

}
