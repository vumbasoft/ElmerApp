using VumbaSoft.ErmanApp.Demographics.Regions;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications.Regions;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreRegionAppService_Tests : RegionAppService_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
