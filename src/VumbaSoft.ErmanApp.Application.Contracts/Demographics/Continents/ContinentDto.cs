using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public class ContinentDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public long Population { get; set; }
    public string Remarks { get; set; }
}
