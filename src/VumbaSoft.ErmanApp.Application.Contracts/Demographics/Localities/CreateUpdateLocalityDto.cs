using System;
using System.ComponentModel.DataAnnotations;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public class CreateUpdateLocalityDto
{
    [Required]
    public Guid DistrictCityId { get; set; }

    [Required]
    [StringLength(LocalityConsts.MaxNameLength)]
    public string Name { get; set; }

    public long Population { get; set; }

    [StringLength(LocalityConsts.MaxRemarksLength)]
    public string Remarks { get; set; }

    [StringLength(LocalityConsts.MaxDistrictCityCodeLength)]
    public string DistrictCityCode { get; set; }

    [StringLength(LocalityConsts.MaxLocalityCodeLength)]
    public string LocalityCode { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
}
