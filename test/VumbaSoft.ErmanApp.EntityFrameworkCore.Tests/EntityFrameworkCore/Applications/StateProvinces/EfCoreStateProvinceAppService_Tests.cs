using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications.StateProvinces;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreStateProvinceAppService_Tests : StateProvinceAppService_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
