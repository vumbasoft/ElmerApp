using System;
using System.ComponentModel.DataAnnotations;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public class CreateUpdateStateProvinceDto
{
    [Required]
    public Guid CountryId { get; set; }

    [Required]
    [StringLength(StateProvinceConsts.MaxNameLength)]
    public string Name { get; set; }

    public long Population { get; set; }

    [StringLength(StateProvinceConsts.MaxRemarksLength)]
    public string Remarks { get; set; }

    [StringLength(StateProvinceConsts.MaxRegionCodeLength)]
    public string RegionCode { get; set; }

    [StringLength(StateProvinceConsts.MaxStateProvinceCodeLength)]
    public string StateProvinceCode { get; set; }
}
