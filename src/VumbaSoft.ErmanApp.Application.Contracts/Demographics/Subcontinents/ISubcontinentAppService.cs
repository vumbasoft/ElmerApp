using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public interface ISubcontinentAppService : IApplicationService
{
    Task<PagedResultDto<SubcontinentDto>> GetListAsync(GetSubcontinentsInput input);

    Task<SubcontinentDto> GetAsync(Guid id);

    Task<SubcontinentDto> CreateAsync(CreateUpdateSubcontinentDto input);

    Task<SubcontinentDto> UpdateAsync(Guid id, CreateUpdateSubcontinentDto input);

    Task DeleteAsync(Guid id);
}
