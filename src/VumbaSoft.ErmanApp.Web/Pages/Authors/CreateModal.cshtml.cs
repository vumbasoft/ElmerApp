using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Authors;
using Microsoft.AspNetCore.Mvc;
namespace VumbaSoft.ErmanApp.Web.Pages.Authors
{
    public class CreateModalModel : ErmanAppPageModel
    {
        [BindProperty]
        public CreateUpdateAuthorDto Author { get; set; }
        private readonly IAuthorAppService _authorAppService;
        public CreateModalModel(IAuthorAppService authorAppService)
        {
            _authorAppService = authorAppService;
        }
        public void OnGet()
        {
            Author = new CreateUpdateAuthorDto();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _authorAppService.CreateAsync(Author);
            return NoContent();
        }
    }
}
