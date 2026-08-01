using System;
using System.ComponentModel.DataAnnotations;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public class CreateUpdateSubcontinentDto
{
    [Required]
    public Guid ContinentId { get; set; }

    [Required]
    [StringLength(SubcontinentConsts.MaxNameLength)]
    public string Name { get; set; }

    public long Population { get; set; }

    [StringLength(SubcontinentConsts.MaxRemarksLength)]
    public string? Remarks { get; set; }
}
