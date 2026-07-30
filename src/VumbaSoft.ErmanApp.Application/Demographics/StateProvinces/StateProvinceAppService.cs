using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

[Authorize(ErmanAppPermissions.StateProvinces.Default)]
public class StateProvinceAppService : ApplicationService, IStateProvinceAppService
{
    private readonly IStateProvinceRepository _stateProvinceRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly StateProvinceManager _stateProvinceManager;

    public StateProvinceAppService(
        IStateProvinceRepository stateProvinceRepository,
        ICountryRepository countryRepository,
        StateProvinceManager stateProvinceManager)
    {
        _stateProvinceRepository = stateProvinceRepository;
        _countryRepository = countryRepository;
        _stateProvinceManager = stateProvinceManager;
    }

    public async Task<PagedResultDto<StateProvinceDto>> GetListAsync(GetStateProvincesInput input)
    {
        var totalCount = await _stateProvinceRepository.GetCountAsync();
        var stateProvinces = await _stateProvinceRepository.GetListAsync(
            input.FilterText,
            input.CountryId,
            input.MaxResultCount,
            input.SkipCount,
            input.Sorting
        );

        var stateProvinceDtos = ObjectMapper.Map<List<StateProvince>, List<StateProvinceDto>>(stateProvinces);
        await SetCountryNamesAsync(stateProvinceDtos);

        return new PagedResultDto<StateProvinceDto>(totalCount, stateProvinceDtos);
    }

    public async Task<StateProvinceDto> GetAsync(Guid id)
    {
        var stateProvince = await _stateProvinceRepository.GetAsync(id);
        var stateProvinceDto = ObjectMapper.Map<StateProvince, StateProvinceDto>(stateProvince);
        await SetCountryNamesAsync(new List<StateProvinceDto> { stateProvinceDto });
        return stateProvinceDto;
    }

    [Authorize(ErmanAppPermissions.StateProvinces.Create)]
    public async Task<StateProvinceDto> CreateAsync(CreateUpdateStateProvinceDto input)
    {
        var stateProvince = await _stateProvinceManager.CreateAsync(
            input.CountryId,
            input.Name,
            input.Population,
            input.Remarks,
            input.RegionCode,
            input.StateProvinceCode
        );

        await _stateProvinceRepository.InsertAsync(stateProvince);
        return ObjectMapper.Map<StateProvince, StateProvinceDto>(stateProvince);
    }

    [Authorize(ErmanAppPermissions.StateProvinces.Edit)]
    public async Task<StateProvinceDto> UpdateAsync(Guid id, CreateUpdateStateProvinceDto input)
    {
        var stateProvince = await _stateProvinceRepository.GetAsync(id);

        stateProvince.SetCountryId(input.CountryId);
        stateProvince.SetName(input.Name);
        stateProvince.SetPopulation(input.Population);
        stateProvince.SetRemarks(input.Remarks);
        stateProvince.SetRegionCode(input.RegionCode);
        stateProvince.SetStateProvinceCode(input.StateProvinceCode);

        await _stateProvinceRepository.UpdateAsync(stateProvince);
        return ObjectMapper.Map<StateProvince, StateProvinceDto>(stateProvince);
    }

    [Authorize(ErmanAppPermissions.StateProvinces.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _stateProvinceRepository.DeleteAsync(id);
    }

    private async Task SetCountryNamesAsync(IReadOnlyCollection<StateProvinceDto> stateProvinces)
    {
        if (stateProvinces.Count == 0)
        {
            return;
        }

        var countryIds = stateProvinces.Select(s => s.CountryId).Distinct().ToList();
        var countries = await _countryRepository.GetListAsync(c => countryIds.Contains(c.Id));
        var countryLookup = countries.ToDictionary(c => c.Id, c => c.Name);

        foreach (var stateProvince in stateProvinces)
        {
            stateProvince.CountryName = countryLookup.GetValueOrDefault(stateProvince.CountryId) ?? string.Empty;
        }
    }
}
