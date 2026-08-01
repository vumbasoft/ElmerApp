using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public abstract class DistrictCityManager_Tests<TStartupModule> : ErmanAppDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly DistrictCityManager _districtCityManager;
    private readonly IDistrictCityRepository _districtCityRepository;
    private readonly StateProvinceManager _stateProvinceManager;
    private readonly IStateProvinceRepository _stateProvinceRepository;
    private readonly CountryManager _countryManager;
    private readonly ICountryRepository _countryRepository;
    private readonly RegionManager _regionManager;
    private readonly IRegionRepository _regionRepository;
    private readonly SubcontinentManager _subcontinentManager;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected DistrictCityManager_Tests()
    {
        _districtCityManager = GetRequiredService<DistrictCityManager>();
        _districtCityRepository = GetRequiredService<IDistrictCityRepository>();
        _stateProvinceManager = GetRequiredService<StateProvinceManager>();
        _stateProvinceRepository = GetRequiredService<IStateProvinceRepository>();
        _countryManager = GetRequiredService<CountryManager>();
        _countryRepository = GetRequiredService<ICountryRepository>();
        _regionManager = GetRequiredService<RegionManager>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentManager = GetRequiredService<SubcontinentManager>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateStateProvinceAsync()
    {
        StateProvince? stateProvince = null;
        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);

            var subcontinent = await _subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

            var region = await _regionManager.CreateAsync(subcontinent.Id, "Region " + Guid.NewGuid());
            await _regionRepository.InsertAsync(region, autoSave: true);

            var country = await _countryManager.CreateAsync(region.Id, "Country " + Guid.NewGuid());
            await _countryRepository.InsertAsync(country, autoSave: true);

            stateProvince = await _stateProvinceManager.CreateAsync(country.Id, "StateProvince " + Guid.NewGuid());
            await _stateProvinceRepository.InsertAsync(stateProvince, autoSave: true);
        });
        return stateProvince!.Id;
    }

    [Fact]
    public async Task Should_Create_A_Valid_DistrictCity()
    {
        var stateProvinceId = await CreateStateProvinceAsync();

        var districtCity = await _districtCityManager.CreateAsync(stateProvinceId, "Test DistrictCity " + Guid.NewGuid(), 1000, "Some remarks", "TC", 12.34m, 56.78m);

        districtCity.Id.ShouldNotBe(Guid.Empty);
        districtCity.StateProvinceId.ShouldBe(stateProvinceId);
        districtCity.Population.ShouldBe(1000);
        districtCity.CountryCode.ShouldBe("TC");
        districtCity.Latitude.ShouldBe(12.34m);
        districtCity.Longitude.ShouldBe(56.78m);
    }

    [Fact]
    public async Task Should_Create_A_DistrictCity_With_Only_Required_Fields()
    {
        var stateProvinceId = await CreateStateProvinceAsync();

        var districtCity = await _districtCityManager.CreateAsync(stateProvinceId, "Test DistrictCity " + Guid.NewGuid());

        districtCity.Population.ShouldBe(0);
        districtCity.Remarks.ShouldBeNull();
        districtCity.CountryCode.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_A_DistrictCity_With_NonExistent_StateProvince()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _districtCityManager.CreateAsync(Guid.NewGuid(), "Test DistrictCity " + Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_DistrictCity_With_Duplicate_Name()
    {
        var stateProvinceId = await CreateStateProvinceAsync();
        var name = "Duplicate DistrictCity " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var districtCity = await _districtCityManager.CreateAsync(stateProvinceId, name);
            await _districtCityRepository.InsertAsync(districtCity, autoSave: true);
        });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _districtCityManager.CreateAsync(stateProvinceId, name);
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_DistrictCity_With_Empty_Name()
    {
        var stateProvinceId = await CreateStateProvinceAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _districtCityManager.CreateAsync(stateProvinceId, "");
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_DistrictCity_With_Negative_Population()
    {
        var stateProvinceId = await CreateStateProvinceAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _districtCityManager.CreateAsync(stateProvinceId, "Test DistrictCity " + Guid.NewGuid(), -1);
        });
    }
}
