using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public class GetRegionsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public Guid? SubcontinentId { get; set; }
}
