using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public class GetLocalitiesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public Guid? DistrictCityId { get; set; }
}
