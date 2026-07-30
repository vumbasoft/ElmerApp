using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public class GetContinentsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
}
