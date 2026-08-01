using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Domains.Subcontinents;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreSubcontinentManager_Tests : SubcontinentManager_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
