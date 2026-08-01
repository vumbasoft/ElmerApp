using System.Threading.Tasks;
using Microsoft.Playwright;
using Shouldly;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages;

/// <summary>
/// Exercises the ABP default (OpenIddict) login page. Assumes the target
/// environment has the standard seeded admin account -
/// see E2ETestConsts.AdminUserName / AdminPassword to override.
/// </summary>
public class Login_Tests : E2ETestBase
{
    public Login_Tests(PlaywrightFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Admin_Can_Log_In_With_Valid_Credentials()
    {
        await Page.GotoAsync("/Account/Login");

        await Page.GetByLabel("Username or email address").FillAsync(E2ETestConsts.AdminUserName);
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).FillAsync(E2ETestConsts.AdminPassword);
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();

        await Page.WaitForURLAsync(url => !url.Contains("/Account/Login"));
        Page.Url.ShouldNotContain("/Account/Login");
    }

    [Fact]
    public async Task Invalid_Credentials_Show_Error_Message()
    {
        await Page.GotoAsync("/Account/Login");

        await Page.GetByLabel("Username or email address").FillAsync(E2ETestConsts.AdminUserName);
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).FillAsync("wrong-password");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();

        await Page.WaitForSelectorAsync("text=Invalid");
        Page.Url.ShouldContain("/Account/Login");
    }
}
