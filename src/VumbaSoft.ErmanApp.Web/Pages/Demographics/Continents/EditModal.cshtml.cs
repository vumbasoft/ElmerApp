using System;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Continents;
using Microsoft.AspNetCore.Mvc;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Continents;

public class EditModalModel : ErmanAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateUpdateContinentDto Continent { get; set; }

    private readonly IContinentAppService _continentAppService;

    public EditModalModel(IContinentAppService continentAppService)
    {
        _continentAppService = continentAppService;
    }

    public async Task OnGetAsync()
    {
        var continentDto = await _continentAppService.GetAsync(Id);
        Continent = ObjectMapper.Map<ContinentDto, CreateUpdateContinentDto>(continentDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _continentAppService.UpdateAsync(Id, Continent);
        return NoContent();
    }
}
