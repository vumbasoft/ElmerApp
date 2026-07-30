using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Continents;
using Microsoft.AspNetCore.Mvc;

namespace VumbaSoft.ErmanApp.Web.Pages.Demographics.Continents;

public class CreateModalModel : ErmanAppPageModel
{
    [BindProperty]
    public CreateUpdateContinentDto Continent { get; set; }

    private readonly IContinentAppService _continentAppService;

    public CreateModalModel(IContinentAppService continentAppService)
    {
        _continentAppService = continentAppService;
    }

    public void OnGet()
    {
        Continent = new CreateUpdateContinentDto();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _continentAppService.CreateAsync(Continent);
        return NoContent();
    }
}
