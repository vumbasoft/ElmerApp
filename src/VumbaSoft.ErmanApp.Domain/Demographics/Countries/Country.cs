using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public class Country : FullAuditedAggregateRoot<Guid>
{
    public virtual Guid RegionId { get; private set; }
    public virtual string Name { get; private set; }
    public virtual long Population { get; private set; }
    public virtual string? Remarks { get; private set; }
    public virtual string? FormalName { get; private set; }
    public virtual string? NativeName { get; private set; }
    public virtual string? ISO3 { get; private set; }
    public virtual string? ISO2 { get; private set; }
    public virtual string? CCN3 { get; private set; }
    public virtual string? PhoneCode { get; private set; }
    public virtual string? Capital { get; private set; }
    public virtual string? Currency { get; private set; }
    public virtual string? Emoji { get; private set; }
    public virtual string? EmojiU { get; private set; }

    protected Country()
    {
    }

    public Country(
        Guid id,
        Guid regionId,
        [NotNull] string name,
        long population = 0,
        string? remarks = null,
        string? formalName = null,
        string? nativeName = null,
        string? iso3 = null,
        string? iso2 = null,
        string? ccn3 = null,
        string? phoneCode = null,
        string? capital = null,
        string? currency = null,
        string? emoji = null,
        string? emojiU = null)
        : base(id)
    {
        SetRegionId(regionId);
        SetName(name);
        SetPopulation(population);
        SetRemarks(remarks);
        SetFormalName(formalName);
        SetNativeName(nativeName);
        SetISO3(iso3);
        SetISO2(iso2);
        SetCCN3(ccn3);
        SetPhoneCode(phoneCode);
        SetCapital(capital);
        SetCurrency(currency);
        SetEmoji(emoji);
        SetEmojiU(emojiU);
    }

    public Country SetRegionId(Guid regionId)
    {
        if (regionId == Guid.Empty)
        {
            throw new ArgumentException("RegionId cannot be empty.", nameof(regionId));
        }

        RegionId = regionId;
        return this;
    }

    public Country SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: CountryConsts.MaxNameLength);
        return this;
    }

    public Country SetPopulation(long population)
    {
        Population = Check.Range(population, nameof(population), 0, long.MaxValue);
        return this;
    }

    public Country SetRemarks(string? remarks)
    {
        Remarks = Check.Length(remarks, nameof(remarks), CountryConsts.MaxRemarksLength);
        return this;
    }

    public Country SetFormalName(string? formalName)
    {
        FormalName = Check.Length(formalName, nameof(formalName), CountryConsts.MaxFormalNameLength);
        return this;
    }

    public Country SetNativeName(string? nativeName)
    {
        NativeName = Check.Length(nativeName, nameof(nativeName), CountryConsts.MaxNativeNameLength);
        return this;
    }

    public Country SetISO3(string? iso3)
    {
        ISO3 = Check.Length(iso3, nameof(iso3), CountryConsts.MaxIso3Length);
        return this;
    }

    public Country SetISO2(string? iso2)
    {
        ISO2 = Check.Length(iso2, nameof(iso2), CountryConsts.MaxIso2Length);
        return this;
    }

    public Country SetCCN3(string? ccn3)
    {
        CCN3 = Check.Length(ccn3, nameof(ccn3), CountryConsts.MaxCcn3Length);
        return this;
    }

    public Country SetPhoneCode(string? phoneCode)
    {
        PhoneCode = Check.Length(phoneCode, nameof(phoneCode), CountryConsts.MaxPhoneCodeLength);
        return this;
    }

    public Country SetCapital(string? capital)
    {
        Capital = Check.Length(capital, nameof(capital), CountryConsts.MaxCapitalLength);
        return this;
    }

    public Country SetCurrency(string? currency)
    {
        Currency = Check.Length(currency, nameof(currency), CountryConsts.MaxCurrencyLength);
        return this;
    }

    public Country SetEmoji(string? emoji)
    {
        Emoji = Check.Length(emoji, nameof(emoji), CountryConsts.MaxEmojiLength);
        return this;
    }

    public Country SetEmojiU(string? emojiU)
    {
        EmojiU = Check.Length(emojiU, nameof(emojiU), CountryConsts.MaxEmojiULength);
        return this;
    }
}
