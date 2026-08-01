using System;
using Volo.Abp.Application.Dtos;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public class CountryDto : FullAuditedEntityDto<Guid>
{
    public Guid RegionId { get; set; }
    public string RegionName { get; set; }
    public string Name { get; set; }
    public long Population { get; set; }
    public string? Remarks { get; set; }
    public string? FormalName { get; set; }
    public string? NativeName { get; set; }
    public string? ISO3 { get; set; }
    public string? ISO2 { get; set; }
    public string? CCN3 { get; set; }
    public string? PhoneCode { get; set; }
    public string? Capital { get; set; }
    public string? Currency { get; set; }
    public string? Emoji { get; set; }
    public string? EmojiU { get; set; }
}
