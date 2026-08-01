using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Shouldly;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages;

/// <summary>
/// Drives the real Subcontinents CRUD page (/Demographics/Subcontinents) in a browser,
/// against an already-running instance of the app. See README.md for prerequisites.
/// Each test creates its own parent Continent through the UI first, so the suite
/// doesn't depend on run order or pre-existing data.
/// </summary>
public class Subcontinents_Tests : E2ETestBase
{
    public Subcontinents_Tests(PlaywrightFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Admin_Can_Create_A_Subcontinent_Through_The_UI()
    {
        await LoginAsAdminAsync();
        var continentName = await CreateContinentViaUiAsync();

        var name = "E2E Subcontinent " + Guid.NewGuid();

        await Page.GotoAsync("/Demographics/Subcontinents");
        await Page.ClickAsync("#NewSubcontinentButton");

        await Page.Locator("#Subcontinent_Name").FillAsync(name);
        await Page.Locator("#Subcontinent_ContinentId").SelectOptionAsync(new SelectOptionValue { Label = continentName });
        await Page.Locator("#Subcontinent_Population").FillAsync("100");
        await Page.Locator("#Subcontinent_Remarks").FillAsync("Created by Playwright E2E test");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var row = Page.Locator("#SubcontinentsTable tr", new PageLocatorOptions { HasText = name });
        await row.WaitForAsync();
        (await row.IsVisibleAsync()).ShouldBeTrue();
    }

    [Fact]
    public async Task Admin_Can_Edit_A_Subcontinent_Through_The_UI()
    {
        await LoginAsAdminAsync();
        var continentName = await CreateContinentViaUiAsync();

        var name = await CreateSubcontinentViaUiAsync(continentName);
        var newName = name + " (edited)";

        var row = Page.Locator("#SubcontinentsTable tr", new PageLocatorOptions { HasText = name });
        await row.GetByRole(AriaRole.Button).ClickAsync();
        await Page.Locator(".dropdown-item", new PageLocatorOptions { HasText = "Edit" }).ClickAsync();

        await Page.Locator("#Subcontinent_Name").FillAsync(newName);
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var updatedRow = Page.Locator("#SubcontinentsTable tr", new PageLocatorOptions { HasText = newName });
        await updatedRow.WaitForAsync();
        (await updatedRow.IsVisibleAsync()).ShouldBeTrue();
    }

    [Fact]
    public async Task Admin_Can_Delete_A_Subcontinent_Through_The_UI()
    {
        await LoginAsAdminAsync();
        var continentName = await CreateContinentViaUiAsync();

        var name = await CreateSubcontinentViaUiAsync(continentName);

        var row = Page.Locator("#SubcontinentsTable tr", new PageLocatorOptions { HasText = name });
        await row.GetByRole(AriaRole.Button).ClickAsync();
        await Page.Locator(".dropdown-item", new PageLocatorOptions { HasText = "Delete" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Yes" }).ClickAsync();

        await Page.Locator("#SubcontinentsTable tr", new PageLocatorOptions { HasText = name }).WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Detached
        });
    }

    private async Task<string> CreateContinentViaUiAsync()
    {
        var name = "E2E Continent " + Guid.NewGuid();

        await Page.GotoAsync("/Demographics/Continents");
        await Page.ClickAsync("#NewContinentButton");
        await Page.Locator("#Continent_Name").FillAsync(name);
        await Page.Locator("#Continent_Population").FillAsync("1");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var row = Page.Locator("#ContinentsTable tr", new PageLocatorOptions { HasText = name });
        await row.WaitForAsync();

        return name;
    }

    private async Task<string> CreateSubcontinentViaUiAsync(string continentName)
    {
        var name = "E2E Subcontinent " + Guid.NewGuid();

        await Page.GotoAsync("/Demographics/Subcontinents");
        await Page.ClickAsync("#NewSubcontinentButton");
        await Page.Locator("#Subcontinent_Name").FillAsync(name);
        await Page.Locator("#Subcontinent_ContinentId").SelectOptionAsync(new SelectOptionValue { Label = continentName });
        await Page.Locator("#Subcontinent_Population").FillAsync("1");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var row = Page.Locator("#SubcontinentsTable tr", new PageLocatorOptions { HasText = name });
        await row.WaitForAsync();

        return name;
    }

    private async Task LoginAsAdminAsync()
    {
        await Page.GotoAsync("/Account/Login");
        await Page.GetByLabel("Username or email address").FillAsync(E2ETestConsts.AdminUserName);
        await Page.GetByLabel("Password", new PageGetByLabelOptions { Exact = true }).FillAsync(E2ETestConsts.AdminPassword);
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
        await Page.WaitForURLAsync(url => !url.Contains("/Account/Login"));
    }
}
