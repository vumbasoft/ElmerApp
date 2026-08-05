using System;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Regions;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public class CountryManager : DomainService
{
    private readonly ICountryRepository _countryRepository;
    private readonly IRegionRepository _regionRepository;

    public CountryManager(
        ICountryRepository countryRepository,
        IRegionRepository regionRepository)
    {
        _countryRepository = countryRepository;
        _regionRepository = regionRepository;
    }

    public async Task<Country> CreateAsync(
        Guid regionId,
        string name,
        long population = 0,
        string? remarks = null,
        string? formalName = null,
        string? nativeName = null,
        string? iso3 = null,
        string? iso2 = null,
        string? ccn3 = null,
        string? phoneCode = null,
        string? capital = null,
        string? currency = null,
        string? emoji = null,
        string? emojiU = null)
    {
        var region = await _regionRepository.FindAsync(regionId);
        if (region == null)
        {
            throw new UserFriendlyException($"Region with id '{regionId}' was not found!");
        }

        if (await _countryRepository.FindByNameAsync(name) != null)
        {
            throw new UserFriendlyException($"A country with name '{name}' already exists!");
        }

        if (population < 0)
        {
            throw new UserFriendlyException("Population cannot be less than zero.");
        }

        return new Country(
            GuidGenerator.Create(),
            regionId,
            name,
            population,
            remarks,
            formalName,
            nativeName,
            iso3,
            iso2,
            ccn3,
            phoneCode,
            capital,
            currency,
            emoji,
            emojiU
        );
    }
}
