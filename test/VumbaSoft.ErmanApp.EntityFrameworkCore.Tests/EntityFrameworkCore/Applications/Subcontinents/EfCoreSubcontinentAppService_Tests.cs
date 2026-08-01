using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications.Subcontinents;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreSubcontinentAppService_Tests : SubcontinentAppService_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
