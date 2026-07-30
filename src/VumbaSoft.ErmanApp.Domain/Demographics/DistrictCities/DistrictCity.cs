using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public class DistrictCity : FullAuditedAggregateRoot<Guid>
{
    public virtual Guid StateProvinceId { get; private set; }
    public virtual string Name { get; private set; }
    public virtual long Population { get; private set; }
    public virtual string Remarks { get; private set; }
    public virtual string CountryCode { get; private set; }
    public virtual decimal Latitude { get; private set; }
    public virtual decimal Longitude { get; private set; }

    protected DistrictCity()
    {
    }

    public DistrictCity(
        Guid id,
        Guid stateProvinceId,
        [NotNull] string name,
        long population = 0,
        string remarks = null,
        string countryCode = null,
        decimal latitude = 0,
        decimal longitude = 0)
        : base(id)
    {
        SetStateProvinceId(stateProvinceId);
        SetName(name);
        SetPopulation(population);
        SetRemarks(remarks);
        SetCountryCode(countryCode);
        SetLatitude(latitude);
        SetLongitude(longitude);
    }

    public DistrictCity SetStateProvinceId(Guid stateProvinceId)
    {
        if (stateProvinceId == Guid.Empty)
        {
            throw new ArgumentException("StateProvinceId cannot be empty.", nameof(stateProvinceId));
        }

        StateProvinceId = stateProvinceId;
        return this;
    }

    public DistrictCity SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: DistrictCityConsts.MaxNameLength);
        return this;
    }

    public DistrictCity SetPopulation(long population)
    {
        Population = Check.Range(population, nameof(population), 0, long.MaxValue);
        return this;
    }

    public DistrictCity SetRemarks(string remarks)
    {
        Remarks = Check.Length(remarks, nameof(remarks), DistrictCityConsts.MaxRemarksLength);
        return this;
    }

    public DistrictCity SetCountryCode(string countryCode)
    {
        CountryCode = Check.Length(countryCode, nameof(countryCode), DistrictCityConsts.MaxCountryCodeLength);
        return this;
    }

    public DistrictCity SetLatitude(decimal latitude)
    {
        Latitude = latitude;
        return this;
    }

    public DistrictCity SetLongitude(decimal longitude)
    {
        Longitude = longitude;
        return this;
    }
}
