using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications.DistrictCities;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreDistrictCityAppService_Tests : DistrictCityAppService_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
