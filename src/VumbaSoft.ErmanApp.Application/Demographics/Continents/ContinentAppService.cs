using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VumbaSoft.ErmanApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

[Authorize(ErmanAppPermissions.Continents.Default)]
public class ContinentAppService : ApplicationService, IContinentAppService
{
    private readonly IContinentRepository _continentRepository;
    private readonly ContinentManager _continentManager;

    public ContinentAppService(
        IContinentRepository continentRepository,
        ContinentManager continentManager)
    {
        _continentRepository = continentRepository;
        _continentManager = continentManager;
    }

    public async Task<PagedResultDto<ContinentDto>> GetListAsync(GetContinentsInput input)
    {
        var totalCount = await _continentRepository.GetCountAsync();
        var continents = await _continentRepository.GetListAsync(
            input.FilterText,
            input.MaxResultCount,
            input.SkipCount,
            input.Sorting
        );

        return new PagedResultDto<ContinentDto>(
            totalCount,
            ObjectMapper.Map<List<Continent>, List<ContinentDto>>(continents)
        );
    }

    public async Task<ContinentDto> GetAsync(Guid id)
    {
        var continent = await _continentRepository.GetAsync(id);
        return ObjectMapper.Map<Continent, ContinentDto>(continent);
    }

    [Authorize(ErmanAppPermissions.Continents.Create)]
    public async Task<ContinentDto> CreateAsync(CreateUpdateContinentDto input)
    {
        var continent = await _continentManager.CreateAsync(input.Name, input.Population, input.Remarks);
        await _continentRepository.InsertAsync(continent);
        return ObjectMapper.Map<Continent, ContinentDto>(continent);
    }

    [Authorize(ErmanAppPermissions.Continents.Edit)]
    public async Task<ContinentDto> UpdateAsync(Guid id, CreateUpdateContinentDto input)
    {
        var continent = await _continentRepository.GetAsync(id);

        continent.SetName(input.Name);
        continent.SetPopulation(input.Population);
        continent.SetRemarks(input.Remarks);

        await _continentRepository.UpdateAsync(continent);
        return ObjectMapper.Map<Continent, ContinentDto>(continent);
    }

    [Authorize(ErmanAppPermissions.Continents.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _continentRepository.DeleteAsync(id);
    }
}
