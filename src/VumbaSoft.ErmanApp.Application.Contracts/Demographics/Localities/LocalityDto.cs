using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public class LocalityDto : FullAuditedEntityDto<Guid>
{
    public Guid DistrictCityId { get; set; }
    public string DistrictCityName { get; set; }
    public string Name { get; set; }
    public long Population { get; set; }
    public string Remarks { get; set; }
    public string DistrictCityCode { get; set; }
    public string LocalityCode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
