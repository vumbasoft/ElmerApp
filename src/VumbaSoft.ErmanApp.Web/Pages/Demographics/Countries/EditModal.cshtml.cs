using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Countries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Countries;

public class EditModalModel : ErmanAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateUpdateCountryDto Country { get; set; }

    public List<SelectListItem> Regions { get; set; } = new();

    private readonly ICountryAppService _countryAppService;
    private readonly IRegionAppService _regionAppService;

    public EditModalModel(
        ICountryAppService countryAppService,
        IRegionAppService regionAppService)
    {
        _countryAppService = countryAppService;
        _regionAppService = regionAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadRegionsAsync();
        var countryDto = await _countryAppService.GetAsync(Id);
        Country = ObjectMapper.Map<CountryDto, CreateUpdateCountryDto>(countryDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _countryAppService.UpdateAsync(Id, Country);
        return NoContent();
    }

    private async Task LoadRegionsAsync()
    {
        var regions = await _regionAppService.GetListAsync(new GetRegionsInput
        {
            MaxResultCount = 1000,
            Sorting = nameof(RegionDto.Name)
        });
        Regions = regions.Items.Select(region => new SelectListItem(region.Name, region.Id.ToString())).ToList();
    }
}
