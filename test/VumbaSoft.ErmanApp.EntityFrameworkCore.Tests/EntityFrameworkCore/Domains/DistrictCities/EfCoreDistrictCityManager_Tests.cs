using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Domains.DistrictCities;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreDistrictCityManager_Tests : DistrictCityManager_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
