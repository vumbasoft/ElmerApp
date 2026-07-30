using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public interface IDistrictCityAppService : IApplicationService
{
    Task<PagedResultDto<DistrictCityDto>> GetListAsync(GetDistrictCitiesInput input);

    Task<DistrictCityDto> GetAsync(Guid id);

    Task<DistrictCityDto> CreateAsync(CreateUpdateDistrictCityDto input);

    Task<DistrictCityDto> UpdateAsync(Guid id, CreateUpdateDistrictCityDto input);

    Task DeleteAsync(Guid id);
}
