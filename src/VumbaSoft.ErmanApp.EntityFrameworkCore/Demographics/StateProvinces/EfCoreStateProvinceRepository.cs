using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using VumbaSoft.ErmanApp.EntityFrameworkCore;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public class EfCoreStateProvinceRepository
    : EfCoreRepository<ErmanAppDbContext, StateProvince, Guid>, IStateProvinceRepository
{
    public EfCoreStateProvinceRepository(IDbContextProvider<ErmanAppDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<StateProvince>> GetListAsync(
        string filterText = null,
        Guid? countryId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(!filterText.IsNullOrWhiteSpace(), s => s.Name.Contains(filterText))
            .WhereIf(countryId.HasValue, s => s.CountryId == countryId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(StateProvince.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    public async Task<StateProvince> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(s => s.Name == name);
    }
}
