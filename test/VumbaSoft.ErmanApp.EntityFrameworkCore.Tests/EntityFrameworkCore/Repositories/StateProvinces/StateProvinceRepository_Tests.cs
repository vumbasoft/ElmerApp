using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Repositories.StateProvinces;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class StateProvinceRepository_Tests : ErmanAppEntityFrameworkCoreTestBase
{
    private readonly IStateProvinceRepository _stateProvinceRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly IContinentRepository _continentRepository;

    public StateProvinceRepository_Tests()
    {
        _stateProvinceRepository = GetRequiredService<IStateProvinceRepository>();
        _countryRepository = GetRequiredService<ICountryRepository>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateCountryAsync(string suffix)
    {
        var continent = new Continent(Guid.NewGuid(), "Continent " + suffix);
        await _continentRepository.InsertAsync(continent, autoSave: true);

        var subcontinent = new Subcontinent(Guid.NewGuid(), continent.Id, "Subcontinent " + suffix);
        await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

        var region = new Region(Guid.NewGuid(), subcontinent.Id, "Region " + suffix);
        await _regionRepository.InsertAsync(region, autoSave: true);

        var country = new Country(Guid.NewGuid(), region.Id, "Country " + suffix);
        await _countryRepository.InsertAsync(country, autoSave: true);

        return country.Id;
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_StateProvince_With_Exact_Name()
    {
        var name = "Repo StateProvince " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var countryId = await CreateCountryAsync(Guid.NewGuid().ToString());
            await _stateProvinceRepository.InsertAsync(new StateProvince(Guid.NewGuid(), countryId, name, 5), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _stateProvinceRepository.FindByNameAsync(name);

            found.ShouldNotBeNull();
            found.Population.ShouldBe(5);
        });
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Null_When_Not_Found()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _stateProvinceRepository.FindByNameAsync("Non existent " + Guid.NewGuid());

            found.ShouldBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_Name()
    {
        var uniqueSuffix = Guid.NewGuid().ToString();
        var matchingName = "Filterable StateProvince " + uniqueSuffix;
        var otherName = "Other StateProvince " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var countryId = await CreateCountryAsync(uniqueSuffix);
            await _stateProvinceRepository.InsertAsync(new StateProvince(Guid.NewGuid(), countryId, matchingName), autoSave: true);
            await _stateProvinceRepository.InsertAsync(new StateProvince(Guid.NewGuid(), countryId, otherName), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _stateProvinceRepository.GetListAsync(filterText: uniqueSuffix);

            result.Count.ShouldBe(1);
            result[0].Name.ShouldBe(matchingName);
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_CountryId()
    {
        var suffix = Guid.NewGuid().ToString();

        Guid countryAId = default, countryBId = default;
        await WithUnitOfWorkAsync(async () =>
        {
            countryAId = await CreateCountryAsync("A " + suffix);
            countryBId = await CreateCountryAsync("B " + suffix);

            await _stateProvinceRepository.InsertAsync(new StateProvince(Guid.NewGuid(), countryAId, "StateProvince A " + suffix), autoSave: true);
            await _stateProvinceRepository.InsertAsync(new StateProvince(Guid.NewGuid(), countryBId, "StateProvince B " + suffix), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _stateProvinceRepository.GetListAsync(filterText: suffix, countryId: countryAId);

            result.Count.ShouldBe(1);
            result[0].CountryId.ShouldBe(countryAId);
        });
    }
}
