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

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public class EfCoreRegionRepository
    : EfCoreRepository<ErmanAppDbContext, Region, Guid>, IRegionRepository
{
    public EfCoreRegionRepository(IDbContextProvider<ErmanAppDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<Region>> GetListAsync(
        string filterText = null,
        Guid? subcontinentId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(!filterText.IsNullOrWhiteSpace(), r => r.Name.Contains(filterText))
            .WhereIf(subcontinentId.HasValue, r => r.SubcontinentId == subcontinentId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Region.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    public async Task<Region> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }
}
