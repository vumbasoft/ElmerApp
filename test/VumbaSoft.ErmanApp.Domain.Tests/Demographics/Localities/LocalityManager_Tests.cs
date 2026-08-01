using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public abstract class LocalityManager_Tests<TStartupModule> : ErmanAppDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly LocalityManager _localityManager;
    private readonly ILocalityRepository _localityRepository;
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

    protected LocalityManager_Tests()
    {
        _localityManager = GetRequiredService<LocalityManager>();
        _localityRepository = GetRequiredService<ILocalityRepository>();
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

    private async Task<Guid> CreateDistrictCityAsync()
    {
        DistrictCity? districtCity = null;
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

            var stateProvince = await _stateProvinceManager.CreateAsync(country.Id, "StateProvince " + Guid.NewGuid());
            await _stateProvinceRepository.InsertAsync(stateProvince, autoSave: true);

            districtCity = await _districtCityManager.CreateAsync(stateProvince.Id, "DistrictCity " + Guid.NewGuid());
            await _districtCityRepository.InsertAsync(districtCity, autoSave: true);
        });
        return districtCity!.Id;
    }

    [Fact]
    public async Task Should_Create_A_Valid_Locality()
    {
        var districtCityId = await CreateDistrictCityAsync();

        var locality = await _localityManager.CreateAsync(districtCityId, "Test Locality " + Guid.NewGuid(), 1000, "Some remarks", "DC", "LC", 12.34m, 56.78m);

        locality.Id.ShouldNotBe(Guid.Empty);
        locality.DistrictCityId.ShouldBe(districtCityId);
        locality.Population.ShouldBe(1000);
        locality.DistrictCityCode.ShouldBe("DC");
        locality.LocalityCode.ShouldBe("LC");
        locality.Latitude.ShouldBe(12.34m);
        locality.Longitude.ShouldBe(56.78m);
    }

    [Fact]
    public async Task Should_Create_A_Locality_With_Only_Required_Fields()
    {
        var districtCityId = await CreateDistrictCityAsync();

        var locality = await _localityManager.CreateAsync(districtCityId, "Test Locality " + Guid.NewGuid());

        locality.Population.ShouldBe(0);
        locality.Remarks.ShouldBeNull();
        locality.LocalityCode.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_A_Locality_With_NonExistent_DistrictCity()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _localityManager.CreateAsync(Guid.NewGuid(), "Test Locality " + Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Locality_With_Duplicate_Name()
    {
        var districtCityId = await CreateDistrictCityAsync();
        var name = "Duplicate Locality " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var locality = await _localityManager.CreateAsync(districtCityId, name);
            await _localityRepository.InsertAsync(locality, autoSave: true);
        });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _localityManager.CreateAsync(districtCityId, name);
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Locality_With_Empty_Name()
    {
        var districtCityId = await CreateDistrictCityAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _localityManager.CreateAsync(districtCityId, "");
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Locality_With_Negative_Population()
    {
        var districtCityId = await CreateDistrictCityAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _localityManager.CreateAsync(districtCityId, "Test Locality " + Guid.NewGuid(), -1);
        });
    }
}
