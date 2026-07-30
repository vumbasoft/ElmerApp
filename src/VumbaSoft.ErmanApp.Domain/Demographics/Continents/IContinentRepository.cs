using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public interface IContinentRepository : IRepository<Continent, Guid>
{
    Task<List<Continent>> GetListAsync(
        string filterText = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null
    );

    Task<Continent> FindByNameAsync(string name);
}
