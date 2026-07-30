using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public class GetSubcontinentsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public Guid? ContinentId { get; set; }
}
