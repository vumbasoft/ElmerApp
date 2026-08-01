using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Domains.StateProvinces;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreStateProvinceManager_Tests : StateProvinceManager_Tests<ErmanAppEntityFrameworkCoreTestModule>
{
}
