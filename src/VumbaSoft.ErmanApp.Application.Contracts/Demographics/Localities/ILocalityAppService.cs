using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public interface ILocalityAppService : IApplicationService
{
    Task<PagedResultDto<LocalityDto>> GetListAsync(GetLocalitiesInput input);

    Task<LocalityDto> GetAsync(Guid id);

    Task<LocalityDto> CreateAsync(CreateUpdateLocalityDto input);

    Task<LocalityDto> UpdateAsync(Guid id, CreateUpdateLocalityDto input);

    Task DeleteAsync(Guid id);
}
