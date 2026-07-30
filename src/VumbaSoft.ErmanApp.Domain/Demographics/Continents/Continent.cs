using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public class Continent : FullAuditedAggregateRoot<Guid>
{
    public virtual string Name { get; private set; }
    public virtual long Population { get; private set; }
    public virtual string Remarks { get; private set; }

    protected Continent()
    {
    }

    public Continent(Guid id, [NotNull] string name, long population = 0, string remarks = null)
        : base(id)
    {
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

    public Continent SetRemarks(string remarks)
    {
        Remarks = Check.Length(remarks, nameof(remarks), ContinentConsts.MaxRemarksLength);
        return this;
    }
}
