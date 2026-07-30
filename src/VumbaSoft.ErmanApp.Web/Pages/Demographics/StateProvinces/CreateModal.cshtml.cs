using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.StateProvinces;

public class CreateModalModel : ErmanAppPageModel
{
    [BindProperty]
    public CreateUpdateStateProvinceDto StateProvince { get; set; }

    public List<SelectListItem> Countries { get; set; } = new();

    private readonly IStateProvinceAppService _stateProvinceAppService;
    private readonly ICountryAppService _countryAppService;

    public CreateModalModel(
        IStateProvinceAppService stateProvinceAppService,
        ICountryAppService countryAppService)
    {
        _stateProvinceAppService = stateProvinceAppService;
        _countryAppService = countryAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadCountriesAsync();
        StateProvince = new CreateUpdateStateProvinceDto
        {
            CountryId = Countries.FirstOrDefault()?.Value is { } countryId ? Guid.Parse(countryId) : Guid.Empty
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _stateProvinceAppService.CreateAsync(StateProvince);
        return NoContent();
    }

    private async Task LoadCountriesAsync()
    {
        var countries = await _countryAppService.GetListAsync(new GetCountriesInput
        {
            MaxResultCount = 1000,
            Sorting = nameof(CountryDto.Name)
        });
        Countries = countries.Items.Select(country => new SelectListItem(country.Name, country.Id.ToString())).ToList();
    }
}
