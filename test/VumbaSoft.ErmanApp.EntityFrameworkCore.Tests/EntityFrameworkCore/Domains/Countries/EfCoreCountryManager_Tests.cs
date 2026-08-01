using VumbaSoft.ErmanApp.Demographics.Countries;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Domains.Countries;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreCountryManager_Tests : CountryManager_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
