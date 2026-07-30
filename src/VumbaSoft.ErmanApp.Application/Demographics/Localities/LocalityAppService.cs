using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

[Authorize(ErmanAppPermissions.Localities.Default)]
public class LocalityAppService : ApplicationService, ILocalityAppService
{
    private readonly ILocalityRepository _localityRepository;
    private readonly IDistrictCityRepository _districtCityRepository;
    private readonly LocalityManager _localityManager;

    public LocalityAppService(
        ILocalityRepository localityRepository,
        IDistrictCityRepository districtCityRepository,
        LocalityManager localityManager)
    {
        _localityRepository = localityRepository;
        _districtCityRepository = districtCityRepository;
        _localityManager = localityManager;
    }

    public async Task<PagedResultDto<LocalityDto>> GetListAsync(GetLocalitiesInput input)
    {
        var totalCount = await _localityRepository.GetCountAsync();
        var localities = await _localityRepository.GetListAsync(
            input.FilterText,
            input.DistrictCityId,
            input.MaxResultCount,
            input.SkipCount,
            input.Sorting
        );

        var localityDtos = ObjectMapper.Map<List<Locality>, List<LocalityDto>>(localities);
        await SetDistrictCityNamesAsync(localityDtos);

        return new PagedResultDto<LocalityDto>(totalCount, localityDtos);
    }

    public async Task<LocalityDto> GetAsync(Guid id)
    {
        var locality = await _localityRepository.GetAsync(id);
        var localityDto = ObjectMapper.Map<Locality, LocalityDto>(locality);
        await SetDistrictCityNamesAsync(new List<LocalityDto> { localityDto });
        return localityDto;
    }

    [Authorize(ErmanAppPermissions.Localities.Create)]
    public async Task<LocalityDto> CreateAsync(CreateUpdateLocalityDto input)
    {
        var locality = await _localityManager.CreateAsync(
            input.DistrictCityId,
            input.Name,
            input.Population,
            input.Remarks,
            input.DistrictCityCode,
            input.LocalityCode,
            input.Latitude,
            input.Longitude
        );

        await _localityRepository.InsertAsync(locality);
        return ObjectMapper.Map<Locality, LocalityDto>(locality);
    }

    [Authorize(ErmanAppPermissions.Localities.Edit)]
    public async Task<LocalityDto> UpdateAsync(Guid id, CreateUpdateLocalityDto input)
    {
        var locality = await _localityRepository.GetAsync(id);

        locality.SetDistrictCityId(input.DistrictCityId);
        locality.SetName(input.Name);
        locality.SetPopulation(input.Population);
        locality.SetRemarks(input.Remarks);
        locality.SetDistrictCityCode(input.DistrictCityCode);
        locality.SetLocalityCode(input.LocalityCode);
        locality.SetLatitude(input.Latitude);
        locality.SetLongitude(input.Longitude);

        await _localityRepository.UpdateAsync(locality);
        return ObjectMapper.Map<Locality, LocalityDto>(locality);
    }

    [Authorize(ErmanAppPermissions.Localities.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _localityRepository.DeleteAsync(id);
    }

    private async Task SetDistrictCityNamesAsync(IReadOnlyCollection<LocalityDto> localities)
    {
        if (localities.Count == 0)
        {
            return;
        }

        var districtCityIds = localities.Select(l => l.DistrictCityId).Distinct().ToList();
        var districtCities = await _districtCityRepository.GetListAsync(d => districtCityIds.Contains(d.Id));
        var districtCityLookup = districtCities.ToDictionary(d => d.Id, d => d.Name);

        foreach (var locality in localities)
        {
            locality.DistrictCityName = districtCityLookup.GetValueOrDefault(locality.DistrictCityId) ?? string.Empty;
        }
    }
}
