using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class Index_Tests : ErmanAppWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
