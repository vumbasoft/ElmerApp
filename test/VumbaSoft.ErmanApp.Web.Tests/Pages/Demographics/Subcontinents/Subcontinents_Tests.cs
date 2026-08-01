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
using Xunit;

namespace VumbaSoft.ErmanApp.Pages.Demographics.Subcontinents;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class Subcontinents_Tests : ErmanAppWebTestBase
{
    [Fact]
    public async Task Index_Page_Should_Render_Subcontinents_Table()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Subcontinents");

        html.ShouldContain("id=\"SubcontinentsTable\"");
        html.ShouldContain("id=\"NewSubcontinentButton\"");
    }

    [Fact]
    public async Task CreateModal_Should_Render_Expected_Form_Fields()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Subcontinents/CreateModal");

        html.ShouldContain("id=\"Subcontinent_Name\"");
        html.ShouldContain("id=\"Subcontinent_ContinentId\"");
        html.ShouldContain("id=\"Subcontinent_Population\"");
        html.ShouldContain("id=\"Subcontinent_Remarks\"");
    }

    [Fact]
    public async Task Can_Create_A_Subcontinent_Through_The_CreateModal_Form()
    {
        var continentId = await CreateContinentAsync();
        var name = "Web Test Subcontinent " + Guid.NewGuid();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Subcontinents/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Subcontinents/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Subcontinent.Name"] = name,
                ["Subcontinent.ContinentId"] = continentId.ToString(),
                ["Subcontinent.Population"] = "123",
                ["Subcontinent.Remarks"] = "Created from Web.Tests",
                ["__RequestVerificationToken"] = token
            }));

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Cannot_Create_A_Subcontinent_Without_A_Name()
    {
        var continentId = await CreateContinentAsync();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Subcontinents/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Subcontinents/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Subcontinent.Name"] = "",
                ["Subcontinent.ContinentId"] = continentId.ToString(),
                ["__RequestVerificationToken"] = token
            }));

        response.IsSuccessStatusCode.ShouldBeFalse();
    }

    private async Task<Guid> CreateContinentAsync()
    {
        using var scope = Services.CreateScope();
        var continentManager = scope.ServiceProvider.GetRequiredService<ContinentManager>();
        var continentRepository = scope.ServiceProvider.GetRequiredService<IContinentRepository>();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        using var uow = uowManager.Begin();
        var continent = await continentManager.CreateAsync("Continent " + Guid.NewGuid());
        await continentRepository.InsertAsync(continent, autoSave: true);
        await uow.CompleteAsync();

        return continent.Id;
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
