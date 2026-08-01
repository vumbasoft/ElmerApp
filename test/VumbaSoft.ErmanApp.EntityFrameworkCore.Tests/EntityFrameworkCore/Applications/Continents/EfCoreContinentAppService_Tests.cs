using VumbaSoft.ErmanApp.Demographics.Continents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications.Continents;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreContinentAppService_Tests : ContinentAppService_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
