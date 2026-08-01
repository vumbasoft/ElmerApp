using VumbaSoft.ErmanApp.Demographics.Regions;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Domains.Regions;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreRegionManager_Tests : RegionManager_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
