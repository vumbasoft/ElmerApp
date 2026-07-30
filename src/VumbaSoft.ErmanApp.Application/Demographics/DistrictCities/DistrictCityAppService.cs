using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

[Authorize(ErmanAppPermissions.DistrictCities.Default)]
public class DistrictCityAppService : ApplicationService, IDistrictCityAppService
{
    private readonly IDistrictCityRepository _districtCityRepository;
    private readonly IStateProvinceRepository _stateProvinceRepository;
    private readonly DistrictCityManager _districtCityManager;

    public DistrictCityAppService(
        IDistrictCityRepository districtCityRepository,
        IStateProvinceRepository stateProvinceRepository,
        DistrictCityManager districtCityManager)
    {
        _districtCityRepository = districtCityRepository;
        _stateProvinceRepository = stateProvinceRepository;
        _districtCityManager = districtCityManager;
    }

    public async Task<PagedResultDto<DistrictCityDto>> GetListAsync(GetDistrictCitiesInput input)
    {
        var totalCount = await _districtCityRepository.GetCountAsync();
        var districtCities = await _districtCityRepository.GetListAsync(
            input.FilterText,
            input.StateProvinceId,
            input.MaxResultCount,
            input.SkipCount,
            input.Sorting
        );

        var districtCityDtos = ObjectMapper.Map<List<DistrictCity>, List<DistrictCityDto>>(districtCities);
        await SetStateProvinceNamesAsync(districtCityDtos);

        return new PagedResultDto<DistrictCityDto>(totalCount, districtCityDtos);
    }

    public async Task<DistrictCityDto> GetAsync(Guid id)
    {
        var districtCity = await _districtCityRepository.GetAsync(id);
        var districtCityDto = ObjectMapper.Map<DistrictCity, DistrictCityDto>(districtCity);
        await SetStateProvinceNamesAsync(new List<DistrictCityDto> { districtCityDto });
        return districtCityDto;
    }

    [Authorize(ErmanAppPermissions.DistrictCities.Create)]
    public async Task<DistrictCityDto> CreateAsync(CreateUpdateDistrictCityDto input)
    {
        var districtCity = await _districtCityManager.CreateAsync(
            input.StateProvinceId,
            input.Name,
            input.Population,
            input.Remarks,
            input.CountryCode,
            input.Latitude,
            input.Longitude
        );

        await _districtCityRepository.InsertAsync(districtCity);
        return ObjectMapper.Map<DistrictCity, DistrictCityDto>(districtCity);
    }

    [Authorize(ErmanAppPermissions.DistrictCities.Edit)]
    public async Task<DistrictCityDto> UpdateAsync(Guid id, CreateUpdateDistrictCityDto input)
    {
        var districtCity = await _districtCityRepository.GetAsync(id);

        districtCity.SetStateProvinceId(input.StateProvinceId);
        districtCity.SetName(input.Name);
        districtCity.SetPopulation(input.Population);
        districtCity.SetRemarks(input.Remarks);
        districtCity.SetCountryCode(input.CountryCode);
        districtCity.SetLatitude(input.Latitude);
        districtCity.SetLongitude(input.Longitude);

        await _districtCityRepository.UpdateAsync(districtCity);
        return ObjectMapper.Map<DistrictCity, DistrictCityDto>(districtCity);
    }

    [Authorize(ErmanAppPermissions.DistrictCities.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _districtCityRepository.DeleteAsync(id);
    }

    private async Task SetStateProvinceNamesAsync(IReadOnlyCollection<DistrictCityDto> districtCities)
    {
        if (districtCities.Count == 0)
        {
            return;
        }

        var stateProvinceIds = districtCities.Select(d => d.StateProvinceId).Distinct().ToList();
        var stateProvinces = await _stateProvinceRepository.GetListAsync(s => stateProvinceIds.Contains(s.Id));
        var stateProvinceLookup = stateProvinces.ToDictionary(s => s.Id, s => s.Name);

        foreach (var districtCity in districtCities)
        {
            districtCity.StateProvinceName = stateProvinceLookup.GetValueOrDefault(districtCity.StateProvinceId) ?? string.Empty;
        }
    }
}
