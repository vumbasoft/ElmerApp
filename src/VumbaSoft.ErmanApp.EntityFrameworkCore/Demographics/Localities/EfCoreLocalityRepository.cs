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

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public class EfCoreLocalityRepository
    : EfCoreRepository<ErmanAppDbContext, Locality, Guid>, ILocalityRepository
{
    public EfCoreLocalityRepository(IDbContextProvider<ErmanAppDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<Locality>> GetListAsync(
        string filterText = null,
        Guid? districtCityId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(!filterText.IsNullOrWhiteSpace(), l => l.Name.Contains(filterText))
            .WhereIf(districtCityId.HasValue, l => l.DistrictCityId == districtCityId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Locality.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    public async Task<Locality> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(l => l.Name == name);
    }
}
