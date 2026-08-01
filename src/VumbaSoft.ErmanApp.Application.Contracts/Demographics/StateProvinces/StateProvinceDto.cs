using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public class StateProvinceDto : FullAuditedEntityDto<Guid>
{
    public Guid CountryId { get; set; }
    public string CountryName { get; set; }
    public string Name { get; set; }
    public long Population { get; set; }
    public string? Remarks { get; set; }
    public string? RegionCode { get; set; }
    public string? StateProvinceCode { get; set; }
}
