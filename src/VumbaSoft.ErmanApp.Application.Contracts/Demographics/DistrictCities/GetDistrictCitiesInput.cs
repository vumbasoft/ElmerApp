using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public class GetDistrictCitiesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public Guid? StateProvinceId { get; set; }
}
