using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Localities;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Repositories.Localities;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class LocalityRepository_Tests : ErmanAppEntityFrameworkCoreTestBase
{
    private readonly ILocalityRepository _localityRepository;
    private readonly IDistrictCityRepository _districtCityRepository;
    private readonly IStateProvinceRepository _stateProvinceRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly IContinentRepository _continentRepository;

    public LocalityRepository_Tests()
    {
        _localityRepository = GetRequiredService<ILocalityRepository>();
        _districtCityRepository = GetRequiredService<IDistrictCityRepository>();
        _stateProvinceRepository = GetRequiredService<IStateProvinceRepository>();
        _countryRepository = GetRequiredService<ICountryRepository>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateDistrictCityAsync(string suffix)
    {
        var continent = new Continent(Guid.NewGuid(), "Continent " + suffix);
        await _continentRepository.InsertAsync(continent, autoSave: true);

        var subcontinent = new Subcontinent(Guid.NewGuid(), continent.Id, "Subcontinent " + suffix);
        await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

        var region = new Region(Guid.NewGuid(), subcontinent.Id, "Region " + suffix);
        await _regionRepository.InsertAsync(region, autoSave: true);

        var country = new Country(Guid.NewGuid(), region.Id, "Country " + suffix);
        await _countryRepository.InsertAsync(country, autoSave: true);

        var stateProvince = new StateProvince(Guid.NewGuid(), country.Id, "StateProvince " + suffix);
        await _stateProvinceRepository.InsertAsync(stateProvince, autoSave: true);

        var districtCity = new DistrictCity(Guid.NewGuid(), stateProvince.Id, "DistrictCity " + suffix);
        await _districtCityRepository.InsertAsync(districtCity, autoSave: true);

        return districtCity.Id;
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Locality_With_Exact_Name()
    {
        var name = "Repo Locality " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var districtCityId = await CreateDistrictCityAsync(Guid.NewGuid().ToString());
            await _localityRepository.InsertAsync(new Locality(Guid.NewGuid(), districtCityId, name, 5), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _localityRepository.FindByNameAsync(name);

            found.ShouldNotBeNull();
            found.Population.ShouldBe(5);
        });
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Null_When_Not_Found()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _localityRepository.FindByNameAsync("Non existent " + Guid.NewGuid());

            found.ShouldBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_Name()
    {
        var uniqueSuffix = Guid.NewGuid().ToString();
        var matchingName = "Filterable Locality " + uniqueSuffix;
        var otherName = "Other Locality " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var districtCityId = await CreateDistrictCityAsync(uniqueSuffix);
            await _localityRepository.InsertAsync(new Locality(Guid.NewGuid(), districtCityId, matchingName), autoSave: true);
            await _localityRepository.InsertAsync(new Locality(Guid.NewGuid(), districtCityId, otherName), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _localityRepository.GetListAsync(filterText: uniqueSuffix);

            result.Count.ShouldBe(1);
            result[0].Name.ShouldBe(matchingName);
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_DistrictCityId()
    {
        var suffix = Guid.NewGuid().ToString();

        Guid districtCityAId = default, districtCityBId = default;
        await WithUnitOfWorkAsync(async () =>
        {
            districtCityAId = await CreateDistrictCityAsync("A " + suffix);
            districtCityBId = await CreateDistrictCityAsync("B " + suffix);

            await _localityRepository.InsertAsync(new Locality(Guid.NewGuid(), districtCityAId, "Locality A " + suffix), autoSave: true);
            await _localityRepository.InsertAsync(new Locality(Guid.NewGuid(), districtCityBId, "Locality B " + suffix), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _localityRepository.GetListAsync(filterText: suffix, districtCityId: districtCityAId);

            result.Count.ShouldBe(1);
            result[0].DistrictCityId.ShouldBe(districtCityAId);
        });
    }
}
