using System;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public class LocalityManager : DomainService
{
    private readonly ILocalityRepository _localityRepository;
    private readonly IDistrictCityRepository _districtCityRepository;

    public LocalityManager(
        ILocalityRepository localityRepository,
        IDistrictCityRepository districtCityRepository)
    {
        _localityRepository = localityRepository;
        _districtCityRepository = districtCityRepository;
    }

    public async Task<Locality> CreateAsync(
        Guid districtCityId,
        string name,
        long population = 0,
        string? remarks = null,
        string? districtCityCode = null,
        string? localityCode = null,
        decimal latitude = 0,
        decimal longitude = 0)
    {
        var districtCity = await _districtCityRepository.FindAsync(districtCityId);
        if (districtCity == null)
        {
            throw new UserFriendlyException($"DistrictCity with id '{districtCityId}' was not found!");
        }

        if (await _localityRepository.FindByNameAsync(name) != null)
        {
            throw new UserFriendlyException($"A locality with name '{name}' already exists!");
        }

        if (population < 0)
        {
            throw new UserFriendlyException("Population cannot be less than zero.");
        }

        return new Locality(
            GuidGenerator.Create(),
            districtCityId,
            name,
            population,
            remarks,
            districtCityCode,
            localityCode,
            latitude,
            longitude
        );
    }
}
