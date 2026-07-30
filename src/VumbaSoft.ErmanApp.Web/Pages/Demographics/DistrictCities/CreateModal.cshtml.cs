using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.DistrictCities;

public class CreateModalModel : ErmanAppPageModel
{
    [BindProperty]
    public CreateUpdateDistrictCityDto DistrictCity { get; set; }

    public List<SelectListItem> StateProvinces { get; set; } = new();

    private readonly IDistrictCityAppService _districtCityAppService;
    private readonly IStateProvinceAppService _stateProvinceAppService;

    public CreateModalModel(
        IDistrictCityAppService districtCityAppService,
        IStateProvinceAppService stateProvinceAppService)
    {
        _districtCityAppService = districtCityAppService;
        _stateProvinceAppService = stateProvinceAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadStateProvincesAsync();
        DistrictCity = new CreateUpdateDistrictCityDto
        {
            StateProvinceId = StateProvinces.FirstOrDefault()?.Value is { } stateProvinceId ? Guid.Parse(stateProvinceId) : Guid.Empty
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _districtCityAppService.CreateAsync(DistrictCity);
        return NoContent();
    }

    private async Task LoadStateProvincesAsync()
    {
        var stateProvinces = await _stateProvinceAppService.GetListAsync(new GetStateProvincesInput
        {
            MaxResultCount = 1000,
            Sorting = nameof(StateProvinceDto.Name)
        });
        StateProvinces = stateProvinces.Items.Select(stateProvince => new SelectListItem(stateProvince.Name, stateProvince.Id.ToString())).ToList();
    }
}
