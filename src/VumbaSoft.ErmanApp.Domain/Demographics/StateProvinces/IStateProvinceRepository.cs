using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public interface IStateProvinceRepository : IRepository<StateProvince, Guid>
{
    Task<List<StateProvince>> GetListAsync(
        string filterText = null,
        Guid? countryId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null
    );

    Task<StateProvince> FindByNameAsync(string name);
}
