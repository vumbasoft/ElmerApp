using VumbaSoft.ErmanApp.Demographics.Continents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Domains.Continents;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreContinentManager_Tests : ContinentManager_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
