using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using VumbaSoft.ErmanApp.Demographics.Regions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Regions;

public class EditModalModel : ErmanAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateUpdateRegionDto Region { get; set; }

    public List<SelectListItem> Subcontinents { get; set; } = new();

    private readonly IRegionAppService _regionAppService;
    private readonly ISubcontinentAppService _subcontinentAppService;

    public EditModalModel(
        IRegionAppService regionAppService,
        ISubcontinentAppService subcontinentAppService)
    {
        _regionAppService = regionAppService;
        _subcontinentAppService = subcontinentAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadSubcontinentsAsync();
        var regionDto = await _regionAppService.GetAsync(Id);
        Region = ObjectMapper.Map<RegionDto, CreateUpdateRegionDto>(regionDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _regionAppService.UpdateAsync(Id, Region);
        return NoContent();
    }

    private async Task LoadSubcontinentsAsync()
    {
        var subcontinents = await _subcontinentAppService.GetListAsync(new GetSubcontinentsInput
        {
            MaxResultCount = 1000,
            Sorting = nameof(SubcontinentDto.Name)
        });
        Subcontinents = subcontinents.Items.Select(subcontinent => new SelectListItem(subcontinent.Name, subcontinent.Id.ToString())).ToList();
    }
}
