using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Shouldly;
using Xunit;

namespace VumbaSoft.ErmanApp.Pages.Demographics.Continents;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class Continents_Tests : ErmanAppWebTestBase
{
    [Fact]
    public async Task Index_Page_Should_Render_Continents_Table()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Continents");

        html.ShouldContain("id=\"ContinentsTable\"");
        html.ShouldContain("id=\"NewContinentButton\"");
    }

    [Fact]
    public async Task CreateModal_Should_Render_Expected_Form_Fields()
    {
        var html = await GetResponseAsStringAsync("/Demographics/Continents/CreateModal");

        html.ShouldContain("id=\"Continent_Name\"");
        html.ShouldContain("id=\"Continent_Population\"");
        html.ShouldContain("id=\"Continent_Remarks\"");
    }

    [Fact]
    public async Task Can_Create_A_Continent_Through_The_CreateModal_Form()
    {
        var name = "Web Test Continent " + Guid.NewGuid();
        var token = await GetAntiForgeryTokenAsync("/Demographics/Continents/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Continents/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Continent.Name"] = name,
                ["Continent.Population"] = "123",
                ["Continent.Remarks"] = "Created from Web.Tests",
                ["__RequestVerificationToken"] = token
            }));

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Cannot_Create_A_Continent_Without_A_Name()
    {
        var token = await GetAntiForgeryTokenAsync("/Demographics/Continents/CreateModal");

        var response = await Client.PostAsync(
            "/Demographics/Continents/CreateModal",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Continent.Name"] = "",
                ["Continent.Population"] = "1",
                ["__RequestVerificationToken"] = token
            }));

        response.IsSuccessStatusCode.ShouldBeFalse();
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
