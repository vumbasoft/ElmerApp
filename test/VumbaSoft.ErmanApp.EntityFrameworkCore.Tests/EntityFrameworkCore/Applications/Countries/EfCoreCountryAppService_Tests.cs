using VumbaSoft.ErmanApp.Demographics.Countries;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications.Countries;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreCountryAppService_Tests : CountryAppService_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
