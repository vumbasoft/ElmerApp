using System;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public class DistrictCityManager : DomainService
{
    private readonly IDistrictCityRepository _districtCityRepository;
    private readonly IStateProvinceRepository _stateProvinceRepository;

    public DistrictCityManager(
        IDistrictCityRepository districtCityRepository,
        IStateProvinceRepository stateProvinceRepository)
    {
        _districtCityRepository = districtCityRepository;
        _stateProvinceRepository = stateProvinceRepository;
    }

    public async Task<DistrictCity> CreateAsync(
        Guid stateProvinceId,
        string name,
        long population = 0,
        string? remarks = null,
        string? countryCode = null,
        decimal latitude = 0,
        decimal longitude = 0)
    {
        var stateProvince = await _stateProvinceRepository.FindAsync(stateProvinceId);
        if (stateProvince == null)
        {
            throw new UserFriendlyException($"StateProvince with id '{stateProvinceId}' was not found!");
        }

        if (await _districtCityRepository.FindByNameAsync(name) != null)
        {
            throw new UserFriendlyException($"A district/city with name '{name}' already exists!");
        }

        return new DistrictCity(
            GuidGenerator.Create(),
            stateProvinceId,
            name,
            population,
            remarks,
            countryCode,
            latitude,
            longitude
        );
    }
}
