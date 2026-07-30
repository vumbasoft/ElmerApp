using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public interface IRegionAppService : IApplicationService
{
    Task<PagedResultDto<RegionDto>> GetListAsync(GetRegionsInput input);

    Task<RegionDto> GetAsync(Guid id);

    Task<RegionDto> CreateAsync(CreateUpdateRegionDto input);

    Task<RegionDto> UpdateAsync(Guid id, CreateUpdateRegionDto input);

    Task DeleteAsync(Guid id);
}
