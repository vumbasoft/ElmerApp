using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public class GetStateProvincesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public Guid? CountryId { get; set; }
}
