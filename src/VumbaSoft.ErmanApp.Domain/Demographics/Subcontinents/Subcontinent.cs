using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public class Subcontinent : FullAuditedAggregateRoot<Guid>
{
    public virtual Guid ContinentId { get; private set; }
    public virtual string Name { get; private set; }
    public virtual long Population { get; private set; }
    public virtual string Remarks { get; private set; }

    protected Subcontinent()
    {
    }

    public Subcontinent(Guid id, Guid continentId, [NotNull] string name, long population = 0, string remarks = null)
        : base(id)
    {
        SetContinentId(continentId);
        SetName(name);
        SetPopulation(population);
        Remarks = remarks;
    }

    public Subcontinent SetContinentId(Guid continentId)
    {
        if (continentId == Guid.Empty)
        {
            throw new ArgumentException("ContinentId cannot be empty.", nameof(continentId));
        }

        ContinentId = continentId;
        return this;
    }

    public Subcontinent SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: SubcontinentConsts.MaxNameLength);
        return this;
    }

    public Subcontinent SetPopulation(long population)
    {
        Population = Check.Range(population, nameof(population), 0, long.MaxValue);
        return this;
    }

    public Subcontinent SetRemarks(string remarks)
    {
        Remarks = Check.Length(remarks, nameof(remarks), SubcontinentConsts.MaxRemarksLength);
        return this;
    }
}
