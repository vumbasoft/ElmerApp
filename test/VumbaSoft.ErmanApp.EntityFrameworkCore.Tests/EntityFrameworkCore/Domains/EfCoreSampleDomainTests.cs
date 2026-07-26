using VumbaSoft.ErmanApp.Samples;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Domains;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ErmanAppEntityFrameworkCoreTestModule>
{

}
