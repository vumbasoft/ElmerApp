using System;
using System.ComponentModel.DataAnnotations;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public class CreateUpdateDistrictCityDto
{
    [Required]
    public Guid StateProvinceId { get; set; }

    [Required]
    [StringLength(DistrictCityConsts.MaxNameLength)]
    public string Name { get; set; }

    public long Population { get; set; }

    [StringLength(DistrictCityConsts.MaxRemarksLength)]
    public string Remarks { get; set; }

    [StringLength(DistrictCityConsts.MaxCountryCodeLength)]
    public string CountryCode { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
}
