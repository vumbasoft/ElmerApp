using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public interface IRegionRepository : IRepository<Region, Guid>
{
    Task<List<Region>> GetListAsync(
        string filterText = null,
        Guid? subcontinentId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null
    );

    Task<Region> FindByNameAsync(string name);
}
