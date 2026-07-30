using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

[Authorize(ErmanAppPermissions.Countries.Default)]
public class CountryAppService : ApplicationService, ICountryAppService
{
    private readonly ICountryRepository _countryRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly CountryManager _countryManager;

    public CountryAppService(
        ICountryRepository countryRepository,
        IRegionRepository regionRepository,
        CountryManager countryManager)
    {
        _countryRepository = countryRepository;
        _regionRepository = regionRepository;
        _countryManager = countryManager;
    }

    public async Task<PagedResultDto<CountryDto>> GetListAsync(GetCountriesInput input)
    {
        var totalCount = await _countryRepository.GetCountAsync();
        var countries = await _countryRepository.GetListAsync(
            input.FilterText,
            input.RegionId,
            input.MaxResultCount,
            input.SkipCount,
            input.Sorting
        );

        var countryDtos = ObjectMapper.Map<List<Country>, List<CountryDto>>(countries);
        await SetRegionNamesAsync(countryDtos);

        return new PagedResultDto<CountryDto>(totalCount, countryDtos);
    }

    public async Task<CountryDto> GetAsync(Guid id)
    {
        var country = await _countryRepository.GetAsync(id);
        var countryDto = ObjectMapper.Map<Country, CountryDto>(country);
        await SetRegionNamesAsync(new List<CountryDto> { countryDto });
        return countryDto;
    }

    [Authorize(ErmanAppPermissions.Countries.Create)]
    public async Task<CountryDto> CreateAsync(CreateUpdateCountryDto input)
    {
        var country = await _countryManager.CreateAsync(
            input.RegionId,
            input.Name,
            input.Population,
            input.Remarks,
            input.FormalName,
            input.NativeName,
            input.ISO3,
            input.ISO2,
            input.CCN3,
            input.PhoneCode,
            input.Capital,
            input.Currency,
            input.Emoji,
            input.EmojiU
        );

        await _countryRepository.InsertAsync(country);
        return ObjectMapper.Map<Country, CountryDto>(country);
    }

    [Authorize(ErmanAppPermissions.Countries.Edit)]
    public async Task<CountryDto> UpdateAsync(Guid id, CreateUpdateCountryDto input)
    {
        var country = await _countryRepository.GetAsync(id);

        country.SetRegionId(input.RegionId);
        country.SetName(input.Name);
        country.SetPopulation(input.Population);
        country.SetRemarks(input.Remarks);
        country.SetFormalName(input.FormalName);
        country.SetNativeName(input.NativeName);
        country.SetISO3(input.ISO3);
        country.SetISO2(input.ISO2);
        country.SetCCN3(input.CCN3);
        country.SetPhoneCode(input.PhoneCode);
        country.SetCapital(input.Capital);
        country.SetCurrency(input.Currency);
        country.SetEmoji(input.Emoji);
        country.SetEmojiU(input.EmojiU);

        await _countryRepository.UpdateAsync(country);
        return ObjectMapper.Map<Country, CountryDto>(country);
    }

    [Authorize(ErmanAppPermissions.Countries.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _countryRepository.DeleteAsync(id);
    }

    private async Task SetRegionNamesAsync(IReadOnlyCollection<CountryDto> countries)
    {
        if (countries.Count == 0)
        {
            return;
        }

        var regionIds = countries.Select(c => c.RegionId).Distinct().ToList();
        var regions = await _regionRepository.GetListAsync(r => regionIds.Contains(r.Id));
        var regionLookup = regions.ToDictionary(r => r.Id, r => r.Name);

        foreach (var country in countries)
        {
            country.RegionName = regionLookup.GetValueOrDefault(country.RegionId) ?? string.Empty;
        }
    }
}
