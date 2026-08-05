using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public abstract class CountryManager_Tests<TStartupModule> : ErmanAppDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CountryManager _countryManager;
    private readonly ICountryRepository _countryRepository;
    private readonly RegionManager _regionManager;
    private readonly IRegionRepository _regionRepository;
    private readonly SubcontinentManager _subcontinentManager;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected CountryManager_Tests()
    {
        _countryManager = GetRequiredService<CountryManager>();
        _countryRepository = GetRequiredService<ICountryRepository>();
        _regionManager = GetRequiredService<RegionManager>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentManager = GetRequiredService<SubcontinentManager>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateRegionAsync()
    {
        Region? region = null;
        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);

            var subcontinent = await _subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

            region = await _regionManager.CreateAsync(subcontinent.Id, "Region " + Guid.NewGuid());
            await _regionRepository.InsertAsync(region, autoSave: true);
        });
        return region!.Id;
    }

    [Fact]
    public async Task Should_Create_A_Valid_Country()
    {
        var regionId = await CreateRegionAsync();

        var country = await _countryManager.CreateAsync(
            regionId,
            "Test Country " + Guid.NewGuid(),
            1000,
            "Some remarks",
            "Republic of Test",
            "Native Test",
            "TST",
            "TS",
            "123",
            "+1",
            "Test City",
            "TSD",
            "🇹🇸",
            "U+1F1F9 U+1F1F8"
        );

        country.Id.ShouldNotBe(Guid.Empty);
        country.RegionId.ShouldBe(regionId);
        country.Population.ShouldBe(1000);
        country.Remarks.ShouldBe("Some remarks");
        country.FormalName.ShouldBe("Republic of Test");
        country.ISO3.ShouldBe("TST");
    }

    [Fact]
    public async Task Should_Create_A_Country_With_Only_Required_Fields()
    {
        var regionId = await CreateRegionAsync();

        var country = await _countryManager.CreateAsync(regionId, "Test Country " + Guid.NewGuid());

        country.Population.ShouldBe(0);
        country.Remarks.ShouldBeNull();
        country.FormalName.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_A_Country_With_NonExistent_Region()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _countryManager.CreateAsync(Guid.NewGuid(), "Test Country " + Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Country_With_Duplicate_Name()
    {
        var regionId = await CreateRegionAsync();
        var name = "Duplicate Country " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var country = await _countryManager.CreateAsync(regionId, name);
            await _countryRepository.InsertAsync(country, autoSave: true);
        });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _countryManager.CreateAsync(regionId, name);
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Country_With_Empty_Name()
    {
        var regionId = await CreateRegionAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _countryManager.CreateAsync(regionId, "");
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Country_With_Negative_Population()
    {
        var regionId = await CreateRegionAsync();

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _countryManager.CreateAsync(regionId, "Test Country " + Guid.NewGuid(), -1);
        });
    }
}
