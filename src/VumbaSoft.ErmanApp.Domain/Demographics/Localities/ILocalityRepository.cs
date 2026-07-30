using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public interface ILocalityRepository : IRepository<Locality, Guid>
{
    Task<List<Locality>> GetListAsync(
        string filterText = null,
        Guid? districtCityId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null
    );

    Task<Locality> FindByNameAsync(string name);
}
