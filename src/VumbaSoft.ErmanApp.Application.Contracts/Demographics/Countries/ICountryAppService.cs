using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public interface ICountryAppService : IApplicationService
{
    Task<PagedResultDto<CountryDto>> GetListAsync(GetCountriesInput input);

    Task<CountryDto> GetAsync(Guid id);

    Task<CountryDto> CreateAsync(CreateUpdateCountryDto input);

    Task<CountryDto> UpdateAsync(Guid id, CreateUpdateCountryDto input);

    Task DeleteAsync(Guid id);
}
