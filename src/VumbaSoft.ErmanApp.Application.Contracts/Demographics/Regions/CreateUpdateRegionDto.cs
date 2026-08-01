using System;
using System.ComponentModel.DataAnnotations;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public class CreateUpdateRegionDto
{
    [Required]
    public Guid SubcontinentId { get; set; }

    [Required]
    [StringLength(RegionConsts.MaxNameLength)]
    public string Name { get; set; }

    public long Population { get; set; }

    [StringLength(RegionConsts.MaxRemarksLength)]
    public string? Remarks { get; set; }
}
