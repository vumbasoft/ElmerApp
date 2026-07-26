using VumbaSoft.ErmanApp.Samples;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ErmanAppEntityFrameworkCoreTestModule>
{

}
