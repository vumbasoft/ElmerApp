using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using VumbaSoft.ErmanApp.Demographics.Regions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Regions;

public class CreateModalModel : ErmanAppPageModel
{
    [BindProperty]
    public CreateUpdateRegionDto Region { get; set; }

    public List<SelectListItem> Subcontinents { get; set; } = new();

    private readonly IRegionAppService _regionAppService;
    private readonly ISubcontinentAppService _subcontinentAppService;

    public CreateModalModel(
        IRegionAppService regionAppService,
        ISubcontinentAppService subcontinentAppService)
    {
        _regionAppService = regionAppService;
        _subcontinentAppService = subcontinentAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadSubcontinentsAsync();
        Region = new CreateUpdateRegionDto
        {
            SubcontinentId = Subcontinents.FirstOrDefault()?.Value is { } subcontinentId ? Guid.Parse(subcontinentId) : Guid.Empty
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _regionAppService.CreateAsync(Region);
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
