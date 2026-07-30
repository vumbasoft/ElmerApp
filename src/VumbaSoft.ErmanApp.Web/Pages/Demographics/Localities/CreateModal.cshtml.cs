using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Localities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Localities;

public class CreateModalModel : ErmanAppPageModel
{
    [BindProperty]
    public CreateUpdateLocalityDto Locality { get; set; }

    public List<SelectListItem> DistrictCities { get; set; } = new();

    private readonly ILocalityAppService _localityAppService;
    private readonly IDistrictCityAppService _districtCityAppService;

    public CreateModalModel(
        ILocalityAppService localityAppService,
        IDistrictCityAppService districtCityAppService)
    {
        _localityAppService = localityAppService;
        _districtCityAppService = districtCityAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadDistrictCitiesAsync();
        Locality = new CreateUpdateLocalityDto
        {
            DistrictCityId = DistrictCities.FirstOrDefault()?.Value is { } districtCityId ? Guid.Parse(districtCityId) : Guid.Empty
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _localityAppService.CreateAsync(Locality);
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
