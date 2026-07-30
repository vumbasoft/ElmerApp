using System.ComponentModel.DataAnnotations;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public class CreateUpdateContinentDto
{
    [Required]
    [StringLength(ContinentConsts.MaxNameLength)]
    public string Name { get; set; }

    public long Population { get; set; }

    [StringLength(ContinentConsts.MaxRemarksLength)]
    public string Remarks { get; set; }
}
