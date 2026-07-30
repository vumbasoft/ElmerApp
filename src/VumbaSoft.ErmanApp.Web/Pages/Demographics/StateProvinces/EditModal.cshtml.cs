using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.StateProvinces;

public class EditModalModel : ErmanAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateUpdateStateProvinceDto StateProvince { get; set; }

    public List<SelectListItem> Countries { get; set; } = new();

    private readonly IStateProvinceAppService _stateProvinceAppService;
    private readonly ICountryAppService _countryAppService;

    public EditModalModel(
        IStateProvinceAppService stateProvinceAppService,
        ICountryAppService countryAppService)
    {
        _stateProvinceAppService = stateProvinceAppService;
        _countryAppService = countryAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadCountriesAsync();
        var stateProvinceDto = await _stateProvinceAppService.GetAsync(Id);
        StateProvince = ObjectMapper.Map<StateProvinceDto, CreateUpdateStateProvinceDto>(stateProvinceDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _stateProvinceAppService.UpdateAsync(Id, StateProvince);
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
