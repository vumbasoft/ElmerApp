using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Subcontinents;

public class EditModalModel : ErmanAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateUpdateSubcontinentDto Subcontinent { get; set; }

    public List<SelectListItem> Continents { get; set; } = new();

    private readonly ISubcontinentAppService _subcontinentAppService;
    private readonly IContinentAppService _continentAppService;

    public EditModalModel(
        ISubcontinentAppService subcontinentAppService,
        IContinentAppService continentAppService)
    {
        _subcontinentAppService = subcontinentAppService;
        _continentAppService = continentAppService;
    }

    public async Task OnGetAsync()
    {
        await LoadContinentsAsync();
        var subcontinentDto = await _subcontinentAppService.GetAsync(Id);
        Subcontinent = ObjectMapper.Map<SubcontinentDto, CreateUpdateSubcontinentDto>(subcontinentDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _subcontinentAppService.UpdateAsync(Id, Subcontinent);
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
