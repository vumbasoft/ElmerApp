using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public interface IContinentAppService : IApplicationService
{
    Task<PagedResultDto<ContinentDto>> GetListAsync(GetContinentsInput input);

    Task<ContinentDto> GetAsync(Guid id);

    Task<ContinentDto> CreateAsync(CreateUpdateContinentDto input);

    Task<ContinentDto> UpdateAsync(Guid id, CreateUpdateContinentDto input);

    Task DeleteAsync(Guid id);
}
