using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

[Authorize(ErmanAppPermissions.Subcontinents.Default)]
public class SubcontinentAppService : ApplicationService, ISubcontinentAppService
{
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly IContinentRepository _continentRepository;
    private readonly SubcontinentManager _subcontinentManager;

    public SubcontinentAppService(
        ISubcontinentRepository subcontinentRepository,
        IContinentRepository continentRepository,
        SubcontinentManager subcontinentManager)
    {
        _subcontinentRepository = subcontinentRepository;
        _continentRepository = continentRepository;
        _subcontinentManager = subcontinentManager;
    }

    public async Task<PagedResultDto<SubcontinentDto>> GetListAsync(GetSubcontinentsInput input)
    {
        var totalCount = await _subcontinentRepository.GetCountAsync();
        var subcontinents = await _subcontinentRepository.GetListAsync(
            input.FilterText,
            input.ContinentId,
            input.MaxResultCount,
            input.SkipCount,
            input.Sorting
        );

        var subcontinentDtos = ObjectMapper.Map<List<Subcontinent>, List<SubcontinentDto>>(subcontinents);
        await SetContinentNamesAsync(subcontinentDtos);

        return new PagedResultDto<SubcontinentDto>(totalCount, subcontinentDtos);
    }

    public async Task<SubcontinentDto> GetAsync(Guid id)
    {
        var subcontinent = await _subcontinentRepository.GetAsync(id);
        var subcontinentDto = ObjectMapper.Map<Subcontinent, SubcontinentDto>(subcontinent);
        await SetContinentNamesAsync(new List<SubcontinentDto> { subcontinentDto });
        return subcontinentDto;
    }

    [Authorize(ErmanAppPermissions.Subcontinents.Create)]
    public async Task<SubcontinentDto> CreateAsync(CreateUpdateSubcontinentDto input)
    {
        var subcontinent = await _subcontinentManager.CreateAsync(
            input.ContinentId, input.Name, input.Population, input.Remarks);

        await _subcontinentRepository.InsertAsync(subcontinent);
        return ObjectMapper.Map<Subcontinent, SubcontinentDto>(subcontinent);
    }

    [Authorize(ErmanAppPermissions.Subcontinents.Edit)]
    public async Task<SubcontinentDto> UpdateAsync(Guid id, CreateUpdateSubcontinentDto input)
    {
        var subcontinent = await _subcontinentRepository.GetAsync(id);

        subcontinent.SetContinentId(input.ContinentId);
        subcontinent.SetName(input.Name);
        subcontinent.SetPopulation(input.Population);
        subcontinent.SetRemarks(input.Remarks);

        await _subcontinentRepository.UpdateAsync(subcontinent);
        return ObjectMapper.Map<Subcontinent, SubcontinentDto>(subcontinent);
    }

    [Authorize(ErmanAppPermissions.Subcontinents.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _subcontinentRepository.DeleteAsync(id);
    }

    private async Task SetContinentNamesAsync(IReadOnlyCollection<SubcontinentDto> subcontinents)
    {
        if (subcontinents.Count == 0)
        {
            return;
        }

        var continentIds = subcontinents.Select(s => s.ContinentId).Distinct().ToList();
        var continents = await _continentRepository.GetListAsync(c => continentIds.Contains(c.Id));
        var continentLookup = continents.ToDictionary(c => c.Id, c => c.Name);

        foreach (var subcontinent in subcontinents)
        {
            subcontinent.ContinentName = continentLookup.GetValueOrDefault(subcontinent.ContinentId) ?? string.Empty;
        }
    }
}
