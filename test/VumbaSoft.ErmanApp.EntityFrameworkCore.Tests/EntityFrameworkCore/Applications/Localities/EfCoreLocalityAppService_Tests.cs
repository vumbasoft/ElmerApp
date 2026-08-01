using VumbaSoft.ErmanApp.Demographics.Localities;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications.Localities;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreLocalityAppService_Tests : LocalityAppService_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
