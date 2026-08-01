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
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages.Demographics.DistrictCities;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class DistrictCities_Tests : ErmanAppWebTestBase
{
    [Fact]
    public async Task Index_Page_Should_Render_DistrictCities_Table()
    {
        var html = await GetResponseAsStringAsync("/Demographics/DistrictCities");

        html.ShouldContain("id=\"DistrictCitiesTable\"");
        html.ShouldContain("id=\"NewDistrictCityButton\"");
    }

    [Fact]
    public async Task CreateModal_Should_Render_Expected_Form_Fields()
    {
        var html = await GetResponseAsStringAsync("/Demographics/DistrictCities/CreateModal");

        html.ShouldContain("id=\"DistrictCity_Name\"");
        html.ShouldContain("id=\"DistrictCity_StateProvinceId\"");
        html.ShouldContain("id=\"DistrictCity_Population\"");
        html.ShouldContain("id=\"DistrictCity_CountryCode\"");
        html.ShouldContain("id=\"DistrictCity_Latitude\"");
        html.ShouldContain("id=\"DistrictCity_Longitude\"");
        html.ShouldContain("id=\"DistrictCity_Remarks\"");
    }

    [Fact]
    public async Task Can_Create_A_DistrictCity_Through_The_CreateModal_Form()
    {
        var stateProvinceId = await CreateStateProvinceAsync();
        var name = "Web Test DistrictCity " + Guid.NewGuid();
        var token = await GetAntiForgeryTokenAsync("/Demographics/DistrictCities/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/DistrictCities/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["DistrictCity.Name"] = name,
                ["DistrictCity.StateProvinceId"] = stateProvinceId.ToString(),
                ["DistrictCity.Population"] = "123",
                ["DistrictCity.CountryCode"] = "TC",
                ["DistrictCity.Latitude"] = "12.34",
                ["DistrictCity.Longitude"] = "56.78",
                ["DistrictCity.Remarks"] = "Created from Web.Tests",
                ["__RequestVerificationToken"] = token
            }));

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Cannot_Create_A_DistrictCity_Without_A_Name()
    {
        var stateProvinceId = await CreateStateProvinceAsync();
        var token = await GetAntiForgeryTokenAsync("/Demographics/DistrictCities/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/DistrictCities/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["DistrictCity.Name"] = "",
                ["DistrictCity.StateProvinceId"] = stateProvinceId.ToString(),
                ["__RequestVerificationToken"] = token
            }));

        response.IsSuccessStatusCode.ShouldBeFalse();
    }

    private async Task<Guid> CreateStateProvinceAsync()
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
        var stateProvinceManager = scope.ServiceProvider.GetRequiredService<StateProvinceManager>();
        var stateProvinceRepository = scope.ServiceProvider.GetRequiredService<IStateProvinceRepository>();
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

        var stateProvince = await stateProvinceManager.CreateAsync(country.Id, "StateProvince " + Guid.NewGuid());
        await stateProvinceRepository.InsertAsync(stateProvince, autoSave: true);
        await uow.CompleteAsync();

        return stateProvince.Id;
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
