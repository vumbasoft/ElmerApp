using System;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Countries;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public class StateProvinceManager : DomainService
{
    private readonly IStateProvinceRepository _stateProvinceRepository;
    private readonly ICountryRepository _countryRepository;

    public StateProvinceManager(
        IStateProvinceRepository stateProvinceRepository,
        ICountryRepository countryRepository)
    {
        _stateProvinceRepository = stateProvinceRepository;
        _countryRepository = countryRepository;
    }

    public async Task<StateProvince> CreateAsync(
        Guid countryId,
        string name,
        long population = 0,
        string? remarks = null,
        string? regionCode = null,
        string? stateProvinceCode = null)
    {
        var country = await _countryRepository.FindAsync(countryId);
        if (country == null)
        {
            throw new UserFriendlyException($"Country with id '{countryId}' was not found!");
        }

        if (await _stateProvinceRepository.FindByNameAsync(name) != null)
        {
            throw new UserFriendlyException($"A state/province with name '{name}' already exists!");
        }

        if (population < 0)
        {
            throw new UserFriendlyException("Population cannot be less than zero.");
        }

        return new StateProvince(
            GuidGenerator.Create(),
            countryId,
            name,
            population,
            remarks,
            regionCode,
            stateProvinceCode
        );
    }
}
