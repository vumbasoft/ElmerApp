using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public class StateProvince : FullAuditedAggregateRoot<Guid>
{
    public virtual Guid CountryId { get; private set; }
    public virtual string Name { get; private set; }
    public virtual long Population { get; private set; }
    public virtual string? Remarks { get; private set; }
    public virtual string? RegionCode { get; private set; }
    public virtual string? StateProvinceCode { get; private set; }
    public virtual ICollection<DistrictCity> DistrictCities { get; protected set; }

    protected StateProvince()
    {
        DistrictCities = new Collection<DistrictCity>();
    }

    public StateProvince(
        Guid id,
        Guid countryId,
        [NotNull] string name,
        long population = 0,
        string? remarks = null,
        string? regionCode = null,
        string? stateProvinceCode = null)
        : base(id)
    {
        DistrictCities = new Collection<DistrictCity>();
        SetCountryId(countryId);
        SetName(name);
        SetPopulation(population);
        SetRemarks(remarks);
        SetRegionCode(regionCode);
        SetStateProvinceCode(stateProvinceCode);
    }

    public StateProvince SetCountryId(Guid countryId)
    {
        if (countryId == Guid.Empty)
        {
            throw new ArgumentException("CountryId cannot be empty.", nameof(countryId));
        }

        CountryId = countryId;
        return this;
    }

    public StateProvince SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: StateProvinceConsts.MaxNameLength);
        return this;
    }

    public StateProvince SetPopulation(long population)
    {
        Population = Check.Range(population, nameof(population), 0, long.MaxValue);
        return this;
    }

    public StateProvince SetRemarks(string? remarks)
    {
        Remarks = Check.Length(remarks, nameof(remarks), StateProvinceConsts.MaxRemarksLength);
        return this;
    }

    public StateProvince SetRegionCode(string? regionCode)
    {
        RegionCode = Check.Length(regionCode, nameof(regionCode), StateProvinceConsts.MaxRegionCodeLength);
        return this;
    }

    public StateProvince SetStateProvinceCode(string? stateProvinceCode)
    {
        StateProvinceCode = Check.Length(stateProvinceCode, nameof(stateProvinceCode), StateProvinceConsts.MaxStateProvinceCodeLength);
        return this;
    }
}
