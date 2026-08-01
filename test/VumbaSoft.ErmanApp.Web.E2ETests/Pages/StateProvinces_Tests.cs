using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Shouldly;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages;

/// <summary>
/// Drives the real StateProvinces CRUD page (/Demographics/StateProvinces) in a browser,
/// against an already-running instance of the app. See README.md for prerequisites. Each
/// test creates its own Continent -&gt; Subcontinent -&gt; Region -&gt; Country chain through the
/// UI first, so the suite doesn't depend on run order or pre-existing data.
/// </summary>
public class StateProvinces_Tests : E2ETestBase
{
    public StateProvinces_Tests(PlaywrightFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Admin_Can_Create_A_StateProvince_Through_The_UI()
    {
        await LoginAsAdminAsync();
        var countryName = await CreateCountryViaUiAsync();

        var name = "E2E StateProvince " + Guid.NewGuid();

        await Page.GotoAsync("/Demographics/StateProvinces");
        await Page.ClickAsync("#NewStateProvinceButton");

        await Page.Locator("#StateProvince_Name").FillAsync(name);
        await Page.Locator("#StateProvince_CountryId").SelectOptionAsync(new SelectOptionValue { Label = countryName });
        await Page.Locator("#StateProvince_Population").FillAsync("100");
        await Page.Locator("#StateProvince_RegionCode").FillAsync("RC");
        await Page.Locator("#StateProvince_StateProvinceCode").FillAsync("SP");
        await Page.Locator("#StateProvince_Remarks").FillAsync("Created by Playwright E2E test");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var row = Page.Locator("#StateProvincesTable tr", new PageLocatorOptions { HasText = name });
        await row.WaitForAsync();
        (await row.IsVisibleAsync()).ShouldBeTrue();
    }

    [Fact]
    public async Task Admin_Can_Edit_A_StateProvince_Through_The_UI()
    {
        await LoginAsAdminAsync();
        var countryName = await CreateCountryViaUiAsync();

        var name = await CreateStateProvinceViaUiAsync(countryName);
        var newName = name + " (edited)";

        var row = Page.Locator("#StateProvincesTable tr", new PageLocatorOptions { HasText = name });
        await row.GetByRole(AriaRole.Button).ClickAsync();
        await Page.Locator(".dropdown-item", new PageLocatorOptions { HasText = "Edit" }).ClickAsync();

        await Page.Locator("#StateProvince_Name").FillAsync(newName);
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var updatedRow = Page.Locator("#StateProvincesTable tr", new PageLocatorOptions { HasText = newName });
        await updatedRow.WaitForAsync();
        (await updatedRow.IsVisibleAsync()).ShouldBeTrue();
    }

    [Fact]
    public async Task Admin_Can_Delete_A_StateProvince_Through_The_UI()
    {
        await LoginAsAdminAsync();
        var countryName = await CreateCountryViaUiAsync();

        var name = await CreateStateProvinceViaUiAsync(countryName);

        var row = Page.Locator("#StateProvincesTable tr", new PageLocatorOptions { HasText = name });
        await row.GetByRole(AriaRole.Button).ClickAsync();
        await Page.Locator(".dropdown-item", new PageLocatorOptions { HasText = "Delete" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Yes" }).ClickAsync();

        await Page.Locator("#StateProvincesTable tr", new PageLocatorOptions { HasText = name }).WaitForAsync(new LocatorWaitForOptions
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

    private async Task<string> CreateSubcontinentViaUiAsync()
    {
        var continentName = await CreateContinentViaUiAsync();
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

    private async Task<string> CreateRegionViaUiAsync()
    {
        var subcontinentName = await CreateSubcontinentViaUiAsync();
        var name = "E2E Region " + Guid.NewGuid();

        await Page.GotoAsync("/Demographics/Regions");
        await Page.ClickAsync("#NewRegionButton");
        await Page.Locator("#Region_Name").FillAsync(name);
        await Page.Locator("#Region_SubcontinentId").SelectOptionAsync(new SelectOptionValue { Label = subcontinentName });
        await Page.Locator("#Region_Population").FillAsync("1");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var row = Page.Locator("#RegionsTable tr", new PageLocatorOptions { HasText = name });
        await row.WaitForAsync();

        return name;
    }

    private async Task<string> CreateCountryViaUiAsync()
    {
        var regionName = await CreateRegionViaUiAsync();
        var name = "E2E Country " + Guid.NewGuid();

        await Page.GotoAsync("/Demographics/Countries");
        await Page.ClickAsync("#NewCountryButton");
        await Page.Locator("#Country_Name").FillAsync(name);
        await Page.Locator("#Country_RegionId").SelectOptionAsync(new SelectOptionValue { Label = regionName });
        await Page.Locator("#Country_Population").FillAsync("1");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var row = Page.Locator("#CountriesTable tr", new PageLocatorOptions { HasText = name });
        await row.WaitForAsync();

        return name;
    }

    private async Task<string> CreateStateProvinceViaUiAsync(string countryName)
    {
        var name = "E2E StateProvince " + Guid.NewGuid();

        await Page.GotoAsync("/Demographics/StateProvinces");
        await Page.ClickAsync("#NewStateProvinceButton");
        await Page.Locator("#StateProvince_Name").FillAsync(name);
        await Page.Locator("#StateProvince_CountryId").SelectOptionAsync(new SelectOptionValue { Label = countryName });
        await Page.Locator("#StateProvince_Population").FillAsync("1");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Save" }).ClickAsync();

        var row = Page.Locator("#StateProvincesTable tr", new PageLocatorOptions { HasText = name });
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
