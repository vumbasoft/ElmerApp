using System.Threading.Tasks;
using Microsoft.Playwright;
using Shouldly;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages;

public class HomePage_Tests : E2ETestBase
{
    public HomePage_Tests(PlaywrightFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Anonymous_User_Sees_Login_Link()
    {
        await Page.GotoAsync("/");

        var loginLink = Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Login" });
        await loginLink.WaitForAsync();
        (await loginLink.IsVisibleAsync()).ShouldBeTrue();
    }

    [Fact]
    public async Task Login_Link_Navigates_To_Account_Login_Page()
    {
        await Page.GotoAsync("/");

        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Login" }).ClickAsync();

        await Page.WaitForURLAsync("**/Account/Login**");
        Page.Url.ShouldContain("/Account/Login");
    }
}
