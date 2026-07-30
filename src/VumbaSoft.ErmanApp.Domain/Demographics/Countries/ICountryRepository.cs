using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public interface ICountryRepository : IRepository<Country, Guid>
{
    Task<List<Country>> GetListAsync(
        string filterText = null,
        Guid? regionId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null
    );

    Task<Country> FindByNameAsync(string name);
}
