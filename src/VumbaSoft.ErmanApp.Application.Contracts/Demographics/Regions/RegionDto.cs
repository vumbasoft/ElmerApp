using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public class RegionDto : FullAuditedEntityDto<Guid>
{
    public Guid SubcontinentId { get; set; }
    public string SubcontinentName { get; set; }
    public string Name { get; set; }
    public long Population { get; set; }
    public string Remarks { get; set; }
}
