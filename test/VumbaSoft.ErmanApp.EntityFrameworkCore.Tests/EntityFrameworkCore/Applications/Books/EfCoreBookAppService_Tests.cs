using VumbaSoft.ErmanApp.Books;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Applications.Books;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<ErmanAppEntityFrameworkCoreTestModule>
{

}