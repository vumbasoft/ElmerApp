using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public class SubcontinentDto : FullAuditedEntityDto<Guid>
{
    public Guid ContinentId { get; set; }
    public string ContinentName { get; set; }
    public string Name { get; set; }
    public long Population { get; set; }
    public string? Remarks { get; set; }
}
