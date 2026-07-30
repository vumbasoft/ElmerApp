using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public class GetCountriesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public Guid? RegionId { get; set; }
}
