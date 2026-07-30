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

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public class EfCoreDistrictCityRepository
    : EfCoreRepository<ErmanAppDbContext, DistrictCity, Guid>, IDistrictCityRepository
{
    public EfCoreDistrictCityRepository(IDbContextProvider<ErmanAppDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<DistrictCity>> GetListAsync(
        string filterText = null,
        Guid? stateProvinceId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(!filterText.IsNullOrWhiteSpace(), d => d.Name.Contains(filterText))
            .WhereIf(stateProvinceId.HasValue, d => d.StateProvinceId == stateProvinceId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(DistrictCity.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    public async Task<DistrictCity> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(d => d.Name == name);
    }
}
