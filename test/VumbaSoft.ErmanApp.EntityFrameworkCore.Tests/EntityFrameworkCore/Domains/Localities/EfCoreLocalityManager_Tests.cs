using VumbaSoft.ErmanApp.Demographics.Localities;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Domains.Localities;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreLocalityManager_Tests : LocalityManager_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
