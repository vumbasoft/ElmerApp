using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public class Region : FullAuditedAggregateRoot<Guid>
{
    public virtual Guid SubcontinentId { get; private set; }
    public virtual string Name { get; private set; }
    public virtual long Population { get; private set; }
    public virtual string Remarks { get; private set; }

    protected Region()
    {
    }

    public Region(Guid id, Guid subcontinentId, [NotNull] string name, long population = 0, string remarks = null)
        : base(id)
    {
        SetSubcontinentId(subcontinentId);
        SetName(name);
        SetPopulation(population);
        Remarks = remarks;
    }

    public Region SetSubcontinentId(Guid subcontinentId)
    {
        if (subcontinentId == Guid.Empty)
        {
            throw new ArgumentException("SubcontinentId cannot be empty.", nameof(subcontinentId));
        }

        SubcontinentId = subcontinentId;
        return this;
    }

    public Region SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: RegionConsts.MaxNameLength);
        return this;
    }

    public Region SetPopulation(long population)
    {
        Population = Check.Range(population, nameof(population), 0, long.MaxValue);
        return this;
    }

    public Region SetRemarks(string remarks)
    {
        Remarks = Check.Length(remarks, nameof(remarks), RegionConsts.MaxRemarksLength);
        return this;
    }
}
