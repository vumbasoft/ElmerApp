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
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages.Demographics.Countries;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class Countries_Tests : ErmanAppWebTestBase
{
    [Fact]
    public async Task Index_Page_Should_Render_Countries_Table()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Countries");

        html.ShouldContain("id=\"CountriesTable\"");
        html.ShouldContain("id=\"NewCountryButton\"");
    }

    [Fact]
    public async Task CreateModal_Should_Render_Expected_Form_Fields()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Countries/CreateModal");

        html.ShouldContain("id=\"Country_Name\"");
        html.ShouldContain("id=\"Country_RegionId\"");
        html.ShouldContain("id=\"Country_Population\"");
        html.ShouldContain("id=\"Country_ISO3\"");
        html.ShouldContain("id=\"Country_Remarks\"");
    }

    [Fact]
    public async Task Can_Create_A_Country_Through_The_CreateModal_Form()
    {
        var regionId = await CreateRegionAsync();
        var name = "Web Test Country " + Guid.NewGuid();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Countries/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Countries/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Country.Name"] = name,
                ["Country.RegionId"] = regionId.ToString(),
                ["Country.Population"] = "123",
                ["Country.ISO3"] = "TST",
                ["Country.Remarks"] = "Created from Web.Tests",
                ["__RequestVerificationToken"] = token
            }));

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Cannot_Create_A_Country_Without_A_Name()
    {
        var regionId = await CreateRegionAsync();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Countries/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Countries/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Country.Name"] = "",
                ["Country.RegionId"] = regionId.ToString(),
                ["__RequestVerificationToken"] = token
            }));

        response.IsSuccessStatusCode.ShouldBeFalse();
    }

    private async Task<Guid> CreateRegionAsync()
    {
        using var scope = Services.CreateScope();
        var continentManager = scope.ServiceProvider.GetRequiredService<ContinentManager>();
        var continentRepository = scope.ServiceProvider.GetRequiredService<IContinentRepository>();
        var subcontinentManager = scope.ServiceProvider.GetRequiredService<SubcontinentManager>();
        var subcontinentRepository = scope.ServiceProvider.GetRequiredService<ISubcontinentRepository>();
        var regionManager = scope.ServiceProvider.GetRequiredService<RegionManager>();
        var regionRepository = scope.ServiceProvider.GetRequiredService<IRegionRepository>();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        using var uow = uowManager.Begin();
        var continent = await continentManager.CreateAsync("Continent " + Guid.NewGuid());
        await continentRepository.InsertAsync(continent, autoSave: true);

        var subcontinent = await subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
        await subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

        var region = await regionManager.CreateAsync(subcontinent.Id, "Region " + Guid.NewGuid());
        await regionRepository.InsertAsync(region, autoSave: true);
        await uow.CompleteAsync();

        return region.Id;
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
