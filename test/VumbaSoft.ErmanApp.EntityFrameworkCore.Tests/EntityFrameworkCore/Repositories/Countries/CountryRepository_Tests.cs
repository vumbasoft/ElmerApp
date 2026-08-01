using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Repositories.Countries;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class CountryRepository_Tests : ErmanAppEntityFrameworkCoreTestBase
{
    private readonly ICountryRepository _countryRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly IContinentRepository _continentRepository;

    public CountryRepository_Tests()
    {
        _countryRepository = GetRequiredService<ICountryRepository>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateRegionAsync(string suffix)
    {
        var continent = new Continent(Guid.NewGuid(), "Continent " + suffix);
        await _continentRepository.InsertAsync(continent, autoSave: true);

        var subcontinent = new Subcontinent(Guid.NewGuid(), continent.Id, "Subcontinent " + suffix);
        await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

        var region = new Region(Guid.NewGuid(), subcontinent.Id, "Region " + suffix);
        await _regionRepository.InsertAsync(region, autoSave: true);

        return region.Id;
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Country_With_Exact_Name()
    {
        var name = "Repo Country " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var regionId = await CreateRegionAsync(Guid.NewGuid().ToString());
            await _countryRepository.InsertAsync(new Country(Guid.NewGuid(), regionId, name, 5), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _countryRepository.FindByNameAsync(name);

            found.ShouldNotBeNull();
            found.Population.ShouldBe(5);
        });
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Null_When_Not_Found()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _countryRepository.FindByNameAsync("Non existent " + Guid.NewGuid());

            found.ShouldBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_Name()
    {
        var uniqueSuffix = Guid.NewGuid().ToString();
        var matchingName = "Filterable Country " + uniqueSuffix;
        var otherName = "Other Country " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var regionId = await CreateRegionAsync(uniqueSuffix);
            await _countryRepository.InsertAsync(new Country(Guid.NewGuid(), regionId, matchingName), autoSave: true);
            await _countryRepository.InsertAsync(new Country(Guid.NewGuid(), regionId, otherName), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _countryRepository.GetListAsync(filterText: uniqueSuffix);

            result.Count.ShouldBe(1);
            result[0].Name.ShouldBe(matchingName);
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_RegionId()
    {
        var suffix = Guid.NewGuid().ToString();

        Guid regionAId = default, regionBId = default;
        await WithUnitOfWorkAsync(async () =>
        {
            regionAId = await CreateRegionAsync("A " + suffix);
            regionBId = await CreateRegionAsync("B " + suffix);

            await _countryRepository.InsertAsync(new Country(Guid.NewGuid(), regionAId, "Country A " + suffix), autoSave: true);
            await _countryRepository.InsertAsync(new Country(Guid.NewGuid(), regionBId, "Country B " + suffix), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _countryRepository.GetListAsync(filterText: suffix, regionId: regionAId);

            result.Count.ShouldBe(1);
            result[0].RegionId.ShouldBe(regionAId);
        });
    }
}
