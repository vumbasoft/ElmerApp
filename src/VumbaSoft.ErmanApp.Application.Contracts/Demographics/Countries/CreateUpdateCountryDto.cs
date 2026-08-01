using System;
using System.ComponentModel.DataAnnotations;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public class CreateUpdateCountryDto
{
    [Required]
    public Guid RegionId { get; set; }

    [Required]
    [StringLength(CountryConsts.MaxNameLength)]
    public string Name { get; set; }

    public long Population { get; set; }

    [StringLength(CountryConsts.MaxRemarksLength)]
    public string? Remarks { get; set; }

    [StringLength(CountryConsts.MaxFormalNameLength)]
    public string? FormalName { get; set; }

    [StringLength(CountryConsts.MaxNativeNameLength)]
    public string? NativeName { get; set; }

    [StringLength(CountryConsts.MaxIso3Length)]
    public string? ISO3 { get; set; }

    [StringLength(CountryConsts.MaxIso2Length)]
    public string? ISO2 { get; set; }

    [StringLength(CountryConsts.MaxCcn3Length)]
    public string? CCN3 { get; set; }

    [StringLength(CountryConsts.MaxPhoneCodeLength)]
    public string? PhoneCode { get; set; }

    [StringLength(CountryConsts.MaxCapitalLength)]
    public string? Capital { get; set; }

    [StringLength(CountryConsts.MaxCurrencyLength)]
    public string? Currency { get; set; }

    [StringLength(CountryConsts.MaxEmojiLength)]
    public string? Emoji { get; set; }

    [StringLength(CountryConsts.MaxEmojiULength)]
    public string? EmojiU { get; set; }
}
