using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Localities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Localities;

public class EditModalModel : ErmanAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateUpdateLocalityDto Locality { get; set; }

    public List<SelectListItem> DistrictCities { get; set; } = new();

    private readonly ILocalityAppService _localityAppService;
    private readonly IDistrictCityAppService _districtCityAppService;

    public EditModalModel(
        ILocalityAppService localityAppService,
        IDistrictCityAppService districtCityAppService)
    {
        _localityAppService = localityAppService;
        _districtCityAppService = districtCityAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadDistrictCitiesAsync();
        var localityDto = await _localityAppService.GetAsync(Id);
        Locality = ObjectMapper.Map<LocalityDto, CreateUpdateLocalityDto>(localityDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _localityAppService.UpdateAsync(Id, Locality);
        return NoContent();
    }

    private async Task LoadDistrictCitiesAsync()
    {
        var districtCities = await _districtCityAppService.GetListAsync(new GetDistrictCitiesInput
        {
            MaxResultCount = 1000,
            Sorting = nameof(DistrictCityDto.Name)
        });
        DistrictCities = districtCities.Items.Select(districtCity => new SelectListItem(districtCity.Name, districtCity.Id.ToString())).ToList();
    }
}
