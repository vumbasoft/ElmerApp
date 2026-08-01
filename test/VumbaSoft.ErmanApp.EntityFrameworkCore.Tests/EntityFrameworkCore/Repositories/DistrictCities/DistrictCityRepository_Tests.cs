using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Repositories.DistrictCities;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class DistrictCityRepository_Tests : ErmanAppEntityFrameworkCoreTestBase
{
    private readonly IDistrictCityRepository _districtCityRepository;
    private readonly IStateProvinceRepository _stateProvinceRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly IContinentRepository _continentRepository;

    public DistrictCityRepository_Tests()
    {
        _districtCityRepository = GetRequiredService<IDistrictCityRepository>();
        _stateProvinceRepository = GetRequiredService<IStateProvinceRepository>();
        _countryRepository = GetRequiredService<ICountryRepository>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateStateProvinceAsync(string suffix)
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

        return stateProvince.Id;
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_DistrictCity_With_Exact_Name()
    {
        var name = "Repo DistrictCity " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var stateProvinceId = await CreateStateProvinceAsync(Guid.NewGuid().ToString());
            await _districtCityRepository.InsertAsync(new DistrictCity(Guid.NewGuid(), stateProvinceId, name, 5), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _districtCityRepository.FindByNameAsync(name);

            found.ShouldNotBeNull();
            found.Population.ShouldBe(5);
        });
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Null_When_Not_Found()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _districtCityRepository.FindByNameAsync("Non existent " + Guid.NewGuid());

            found.ShouldBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_Name()
    {
        var uniqueSuffix = Guid.NewGuid().ToString();
        var matchingName = "Filterable DistrictCity " + uniqueSuffix;
        var otherName = "Other DistrictCity " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var stateProvinceId = await CreateStateProvinceAsync(uniqueSuffix);
            await _districtCityRepository.InsertAsync(new DistrictCity(Guid.NewGuid(), stateProvinceId, matchingName), autoSave: true);
            await _districtCityRepository.InsertAsync(new DistrictCity(Guid.NewGuid(), stateProvinceId, otherName), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _districtCityRepository.GetListAsync(filterText: uniqueSuffix);

            result.Count.ShouldBe(1);
            result[0].Name.ShouldBe(matchingName);
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_StateProvinceId()
    {
        var suffix = Guid.NewGuid().ToString();

        Guid stateProvinceAId = default, stateProvinceBId = default;
        await WithUnitOfWorkAsync(async () =>
        {
            stateProvinceAId = await CreateStateProvinceAsync("A " + suffix);
            stateProvinceBId = await CreateStateProvinceAsync("B " + suffix);

            await _districtCityRepository.InsertAsync(new DistrictCity(Guid.NewGuid(), stateProvinceAId, "DistrictCity A " + suffix), autoSave: true);
            await _districtCityRepository.InsertAsync(new DistrictCity(Guid.NewGuid(), stateProvinceBId, "DistrictCity B " + suffix), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _districtCityRepository.GetListAsync(filterText: suffix, stateProvinceId: stateProvinceAId);

            result.Count.ShouldBe(1);
            result[0].StateProvinceId.ShouldBe(stateProvinceAId);
        });
    }
}
