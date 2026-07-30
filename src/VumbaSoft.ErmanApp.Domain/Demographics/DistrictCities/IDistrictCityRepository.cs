using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public interface IDistrictCityRepository : IRepository<DistrictCity, Guid>
{
    Task<List<DistrictCity>> GetListAsync(
        string filterText = null,
        Guid? stateProvinceId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null
    );

    Task<DistrictCity> FindByNameAsync(string name);
}
