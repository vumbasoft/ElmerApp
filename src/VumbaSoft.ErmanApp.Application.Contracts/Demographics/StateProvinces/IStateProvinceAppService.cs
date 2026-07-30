using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public interface IStateProvinceAppService : IApplicationService
{
    Task<PagedResultDto<StateProvinceDto>> GetListAsync(GetStateProvincesInput input);

    Task<StateProvinceDto> GetAsync(Guid id);

    Task<StateProvinceDto> CreateAsync(CreateUpdateStateProvinceDto input);

    Task<StateProvinceDto> UpdateAsync(Guid id, CreateUpdateStateProvinceDto input);

    Task DeleteAsync(Guid id);
}
