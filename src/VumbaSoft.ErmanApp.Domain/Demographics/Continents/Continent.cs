using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public class Continent : FullAuditedAggregateRoot<Guid>
{
    public virtual string Name { get; private set; }
    public virtual long Population { get; private set; }
    public virtual string? Remarks { get; private set; }
    public virtual ICollection<Subcontinent> Subcontinents { get; protected set; }

    protected Continent()
    {
        Subcontinents = new Collection<Subcontinent>();
    }

    public Continent(Guid id, [NotNull] string name, long population = 0, string? remarks = null)
        : base(id)
    {
        Subcontinents = new Collection<Subcontinent>();
        SetName(name);
        SetPopulation(population);
        Remarks = remarks;
    }

    public Continent SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: ContinentConsts.MaxNameLength);
        return this;
    }

    public Continent SetPopulation(long population)
    {
        Population = Check.Range(population, nameof(population), 0, long.MaxValue);
        return this;
    }

    public Continent SetRemarks(string? remarks)
    {
        Remarks = Check.Length(remarks, nameof(remarks), ContinentConsts.MaxRemarksLength);
        return this;
    }
}
