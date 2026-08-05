using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public abstract class StateProvinceManager_Tests<TStartupModule> : ErmanAppDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
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

    protected StateProvinceManager_Tests()
    {
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

    private async Task<Guid> CreateCountryAsync()
    {
        Country? country = null;
        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);

            var subcontinent = await _subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

            var region = await _regionManager.CreateAsync(subcontinent.Id, "Region " + Guid.NewGuid());
            await _regionRepository.InsertAsync(region, autoSave: true);

            country = await _countryManager.CreateAsync(region.Id, "Country " + Guid.NewGuid());
            await _countryRepository.InsertAsync(country, autoSave: true);
        });
        return country!.Id;
    }

    [Fact]
    public async Task Should_Create_A_Valid_StateProvince()
    {
        var countryId = await CreateCountryAsync();

        var stateProvince = await _stateProvinceManager.CreateAsync(countryId, "Test StateProvince " + Guid.NewGuid(), 1000, "Some remarks", "RC", "SP");

        stateProvince.Id.ShouldNotBe(Guid.Empty);
        stateProvince.CountryId.ShouldBe(countryId);
        stateProvince.Population.ShouldBe(1000);
        stateProvince.Remarks.ShouldBe("Some remarks");
        stateProvince.RegionCode.ShouldBe("RC");
        stateProvince.StateProvinceCode.ShouldBe("SP");
    }

    [Fact]
    public async Task Should_Create_A_StateProvince_With_Only_Required_Fields()
    {
        var countryId = await CreateCountryAsync();

        var stateProvince = await _stateProvinceManager.CreateAsync(countryId, "Test StateProvince " + Guid.NewGuid());

        stateProvince.Population.ShouldBe(0);
        stateProvince.Remarks.ShouldBeNull();
        stateProvince.RegionCode.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_A_StateProvince_With_NonExistent_Country()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _stateProvinceManager.CreateAsync(Guid.NewGuid(), "Test StateProvince " + Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_StateProvince_With_Duplicate_Name()
    {
        var countryId = await CreateCountryAsync();
        var name = "Duplicate StateProvince " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var stateProvince = await _stateProvinceManager.CreateAsync(countryId, name);
            await _stateProvinceRepository.InsertAsync(stateProvince, autoSave: true);
        });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _stateProvinceManager.CreateAsync(countryId, name);
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_StateProvince_With_Empty_Name()
    {
        var countryId = await CreateCountryAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _stateProvinceManager.CreateAsync(countryId, "");
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_StateProvince_With_Negative_Population()
    {
        var countryId = await CreateCountryAsync();

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _stateProvinceManager.CreateAsync(countryId, "Test StateProvince " + Guid.NewGuid(), -1);
        });
    }
}
