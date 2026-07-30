using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public class DistrictCityDto : FullAuditedEntityDto<Guid>
{
    public Guid StateProvinceId { get; set; }
    public string StateProvinceName { get; set; }
    public string Name { get; set; }
    public long Population { get; set; }
    public string Remarks { get; set; }
    public string CountryCode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
