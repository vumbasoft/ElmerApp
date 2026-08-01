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
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages.Demographics.Regions;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class Regions_Tests : ErmanAppWebTestBase
{
    [Fact]
    public async Task Index_Page_Should_Render_Regions_Table()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Regions");

        html.ShouldContain("id=\"RegionsTable\"");
        html.ShouldContain("id=\"NewRegionButton\"");
    }

    [Fact]
    public async Task CreateModal_Should_Render_Expected_Form_Fields()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Regions/CreateModal");

        html.ShouldContain("id=\"Region_Name\"");
        html.ShouldContain("id=\"Region_SubcontinentId\"");
        html.ShouldContain("id=\"Region_Population\"");
        html.ShouldContain("id=\"Region_Remarks\"");
    }

    [Fact]
    public async Task Can_Create_A_Region_Through_The_CreateModal_Form()
    {
        var subcontinentId = await CreateSubcontinentAsync();
        var name = "Web Test Region " + Guid.NewGuid();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Regions/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Regions/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Region.Name"] = name,
                ["Region.SubcontinentId"] = subcontinentId.ToString(),
                ["Region.Population"] = "123",
                ["Region.Remarks"] = "Created from Web.Tests",
                ["__RequestVerificationToken"] = token
            }));

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Cannot_Create_A_Region_Without_A_Name()
    {
        var subcontinentId = await CreateSubcontinentAsync();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Regions/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Regions/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Region.Name"] = "",
                ["Region.SubcontinentId"] = subcontinentId.ToString(),
                ["__RequestVerificationToken"] = token
            }));

        response.IsSuccessStatusCode.ShouldBeFalse();
    }

    private async Task<Guid> CreateSubcontinentAsync()
    {
        using var scope = Services.CreateScope();
        var continentManager = scope.ServiceProvider.GetRequiredService<ContinentManager>();
        var continentRepository = scope.ServiceProvider.GetRequiredService<IContinentRepository>();
        var subcontinentManager = scope.ServiceProvider.GetRequiredService<SubcontinentManager>();
        var subcontinentRepository = scope.ServiceProvider.GetRequiredService<ISubcontinentRepository>();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        using var uow = uowManager.Begin();
        var continent = await continentManager.CreateAsync("Continent " + Guid.NewGuid());
        await continentRepository.InsertAsync(continent, autoSave: true);

        var subcontinent = await subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
        await subcontinentRepository.InsertAsync(subcontinent, autoSave: true);
        await uow.CompleteAsync();

        return subcontinent.Id;
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
