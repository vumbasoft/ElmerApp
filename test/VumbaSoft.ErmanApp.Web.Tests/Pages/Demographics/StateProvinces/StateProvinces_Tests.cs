using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Uow;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages.Demographics.StateProvinces;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class StateProvinces_Tests : ErmanAppWebTestBase
{
    [Fact]
    public async Task Index_Page_Should_Render_StateProvinces_Table()
    {
        var html = await GetResponseAsStringAsync("/Demographics/StateProvinces");

        html.ShouldContain("id=\"StateProvincesTable\"");
        html.ShouldContain("id=\"NewStateProvinceButton\"");
    }

    [Fact]
    public async Task CreateModal_Should_Render_Expected_Form_Fields()
    {
        var html = await GetResponseAsStringAsync("/Demographics/StateProvinces/CreateModal");

        html.ShouldContain("id=\"StateProvince_Name\"");
        html.ShouldContain("id=\"StateProvince_CountryId\"");
        html.ShouldContain("id=\"StateProvince_Population\"");
        html.ShouldContain("id=\"StateProvince_RegionCode\"");
        html.ShouldContain("id=\"StateProvince_StateProvinceCode\"");
        html.ShouldContain("id=\"StateProvince_Remarks\"");
    }

    [Fact]
    public async Task Can_Create_A_StateProvince_Through_The_CreateModal_Form()
    {
        var countryId = await CreateCountryAsync();
        var name = "Web Test StateProvince " + Guid.NewGuid();
        var token = await GetAntiForgeryTokenAsync("/Demographics/StateProvinces/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/StateProvinces/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["StateProvince.Name"] = name,
                ["StateProvince.CountryId"] = countryId.ToString(),
                ["StateProvince.Population"] = "123",
                ["StateProvince.RegionCode"] = "RC",
                ["StateProvince.StateProvinceCode"] = "SP",
                ["StateProvince.Remarks"] = "Created from Web.Tests",
                ["__RequestVerificationToken"] = token
            }));

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Cannot_Create_A_StateProvince_Without_A_Name()
    {
        var countryId = await CreateCountryAsync();
        var token = await GetAntiForgeryTokenAsync("/Demographics/StateProvinces/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/StateProvinces/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["StateProvince.Name"] = "",
                ["StateProvince.CountryId"] = countryId.ToString(),
                ["__RequestVerificationToken"] = token
            }));

        response.IsSuccessStatusCode.ShouldBeFalse();
    }

    private async Task<Guid> CreateCountryAsync()
    {
        using var scope = Services.CreateScope();
        var continentManager = scope.ServiceProvider.GetRequiredService<ContinentManager>();
        var continentRepository = scope.ServiceProvider.GetRequiredService<IContinentRepository>();
        var subcontinentManager = scope.ServiceProvider.GetRequiredService<SubcontinentManager>();
        var subcontinentRepository = scope.ServiceProvider.GetRequiredService<ISubcontinentRepository>();
        var regionManager = scope.ServiceProvider.GetRequiredService<RegionManager>();
        var regionRepository = scope.ServiceProvider.GetRequiredService<IRegionRepository>();
        var countryManager = scope.ServiceProvider.GetRequiredService<CountryManager>();
        var countryRepository = scope.ServiceProvider.GetRequiredService<ICountryRepository>();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        using var uow = uowManager.Begin();
        var continent = await continentManager.CreateAsync("Continent " + Guid.NewGuid());
        await continentRepository.InsertAsync(continent, autoSave: true);

        var subcontinent = await subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
        await subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

        var region = await regionManager.CreateAsync(subcontinent.Id, "Region " + Guid.NewGuid());
        await regionRepository.InsertAsync(region, autoSave: true);

        var country = await countryManager.CreateAsync(region.Id, "Country " + Guid.NewGuid());
        await countryRepository.InsertAsync(country, autoSave: true);
        await uow.CompleteAsync();

        return country.Id;
    }

    private async Task<string> GetAntiForgeryTokenAsync(string url)
    {
        var html = await GetResponseAsStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        return doc.DocumentNode
            .SelectSingleNode("//input[@name='__RequestVerificationToken']")
            .GetAttributeValue("value", string.Empty);
    }
}
