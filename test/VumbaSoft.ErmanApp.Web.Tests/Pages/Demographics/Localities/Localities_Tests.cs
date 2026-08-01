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
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages.Demographics.Localities;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class Localities_Tests : ErmanAppWebTestBase
{
    [Fact]
    public async Task Index_Page_Should_Render_Localities_Table()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Localities");

        html.ShouldContain("id=\"LocalitiesTable\"");
        html.ShouldContain("id=\"NewLocalityButton\"");
    }

    [Fact]
    public async Task CreateModal_Should_Render_Expected_Form_Fields()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Localities/CreateModal");

        html.ShouldContain("id=\"Locality_Name\"");
        html.ShouldContain("id=\"Locality_DistrictCityId\"");
        html.ShouldContain("id=\"Locality_Population\"");
        html.ShouldContain("id=\"Locality_DistrictCityCode\"");
        html.ShouldContain("id=\"Locality_LocalityCode\"");
        html.ShouldContain("id=\"Locality_Latitude\"");
        html.ShouldContain("id=\"Locality_Longitude\"");
        html.ShouldContain("id=\"Locality_Remarks\"");
    }

    [Fact]
    public async Task Can_Create_A_Locality_Through_The_CreateModal_Form()
    {
        var districtCityId = await CreateDistrictCityAsync();
        var name = "Web Test Locality " + Guid.NewGuid();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Localities/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Localities/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Locality.Name"] = name,
                ["Locality.DistrictCityId"] = districtCityId.ToString(),
                ["Locality.Population"] = "123",
                ["Locality.DistrictCityCode"] = "DC",
                ["Locality.LocalityCode"] = "LC",
                ["Locality.Latitude"] = "12.34",
                ["Locality.Longitude"] = "56.78",
                ["Locality.Remarks"] = "Created from Web.Tests",
                ["__RequestVerificationToken"] = token
            }));

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Cannot_Create_A_Locality_Without_A_Name()
    {
        var districtCityId = await CreateDistrictCityAsync();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Localities/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Localities/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Locality.Name"] = "",
                ["Locality.DistrictCityId"] = districtCityId.ToString(),
                ["__RequestVerificationToken"] = token
            }));

        response.IsSuccessStatusCode.ShouldBeFalse();
    }

    private async Task<Guid> CreateDistrictCityAsync()
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
        var districtCityManager = scope.ServiceProvider.GetRequiredService<DistrictCityManager>();
        var districtCityRepository = scope.ServiceProvider.GetRequiredService<IDistrictCityRepository>();
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

        var districtCity = await districtCityManager.CreateAsync(stateProvince.Id, "DistrictCity " + Guid.NewGuid());
        await districtCityRepository.InsertAsync(districtCity, autoSave: true);
        await uow.CompleteAsync();

        return districtCity.Id;
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
