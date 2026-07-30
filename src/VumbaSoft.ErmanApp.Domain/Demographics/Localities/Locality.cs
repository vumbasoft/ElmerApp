using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public class Locality : FullAuditedAggregateRoot<Guid>
{
    public virtual Guid DistrictCityId { get; private set; }
    public virtual string Name { get; private set; }
    public virtual long Population { get; private set; }
    public virtual string Remarks { get; private set; }
    public virtual string DistrictCityCode { get; private set; }
    public virtual string LocalityCode { get; private set; }
    public virtual decimal Latitude { get; private set; }
    public virtual decimal Longitude { get; private set; }

    protected Locality()
    {
    }

    public Locality(
        Guid id,
        Guid districtCityId,
        [NotNull] string name,
        long population = 0,
        string remarks = null,
        string districtCityCode = null,
        string localityCode = null,
        decimal latitude = 0,
        decimal longitude = 0)
        : base(id)
    {
        SetDistrictCityId(districtCityId);
        SetName(name);
        SetPopulation(population);
        SetRemarks(remarks);
        SetDistrictCityCode(districtCityCode);
        SetLocalityCode(localityCode);
        SetLatitude(latitude);
        SetLongitude(longitude);
    }

    public Locality SetDistrictCityId(Guid districtCityId)
    {
        if (districtCityId == Guid.Empty)
        {
            throw new ArgumentException("DistrictCityId cannot be empty.", nameof(districtCityId));
        }

        DistrictCityId = districtCityId;
        return this;
    }

    public Locality SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: LocalityConsts.MaxNameLength);
        return this;
    }

    public Locality SetPopulation(long population)
    {
        Population = Check.Range(population, nameof(population), 0, long.MaxValue);
        return this;
    }

    public Locality SetRemarks(string remarks)
    {
        Remarks = Check.Length(remarks, nameof(remarks), LocalityConsts.MaxRemarksLength);
        return this;
    }

    public Locality SetDistrictCityCode(string districtCityCode)
    {
        DistrictCityCode = Check.Length(districtCityCode, nameof(districtCityCode), LocalityConsts.MaxDistrictCityCodeLength);
        return this;
    }

    public Locality SetLocalityCode(string localityCode)
    {
        LocalityCode = Check.Length(localityCode, nameof(localityCode), LocalityConsts.MaxLocalityCodeLength);
        return this;
    }

    public Locality SetLatitude(decimal latitude)
    {
        Latitude = latitude;
        return this;
    }

    public Locality SetLongitude(decimal longitude)
    {
        Longitude = longitude;
        return this;
    }
}
