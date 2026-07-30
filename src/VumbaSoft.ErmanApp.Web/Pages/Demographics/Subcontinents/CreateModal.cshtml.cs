using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Subcontinents;

public class CreateModalModel : ErmanAppPageModel
{
    [BindProperty]
    public CreateUpdateSubcontinentDto Subcontinent { get; set; }

    public List<SelectListItem> Continents { get; set; } = new();

    private readonly ISubcontinentAppService _subcontinentAppService;
    private readonly IContinentAppService _continentAppService;

    public CreateModalModel(
        ISubcontinentAppService subcontinentAppService,
        IContinentAppService continentAppService)
    {
        _subcontinentAppService = subcontinentAppService;
        _continentAppService = continentAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadContinentsAsync();
        Subcontinent = new CreateUpdateSubcontinentDto
        {
            ContinentId = Continents.FirstOrDefault()?.Value is { } continentId ? Guid.Parse(continentId) : Guid.Empty
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _subcontinentAppService.CreateAsync(Subcontinent);
        return NoContent();
    }

    private async Task LoadContinentsAsync()
    {
        var continents = await _continentAppService.GetListAsync(new GetContinentsInput
        {
            MaxResultCount = 1000,
            Sorting = nameof(ContinentDto.Name)
        });
        Continents = continents.Items.Select(continent => new SelectListItem(continent.Name, continent.Id.ToString())).ToList();
    }
}
