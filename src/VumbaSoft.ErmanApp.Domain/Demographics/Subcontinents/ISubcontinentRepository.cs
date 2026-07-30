using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public interface ISubcontinentRepository : IRepository<Subcontinent, Guid>
{
    Task<List<Subcontinent>> GetListAsync(
        string filterText = null,
        Guid? continentId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null
    );

    Task<Subcontinent> FindByNameAsync(string name);
}
