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

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public class EfCoreSubcontinentRepository
    : EfCoreRepository<ErmanAppDbContext, Subcontinent, Guid>, ISubcontinentRepository
{
    public EfCoreSubcontinentRepository(IDbContextProvider<ErmanAppDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<Subcontinent>> GetListAsync(
        string filterText = null,
        Guid? continentId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(!filterText.IsNullOrWhiteSpace(), s => s.Name.Contains(filterText))
            .WhereIf(continentId.HasValue, s => s.ContinentId == continentId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Subcontinent.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    public async Task<Subcontinent> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(s => s.Name == name);
    }
}
