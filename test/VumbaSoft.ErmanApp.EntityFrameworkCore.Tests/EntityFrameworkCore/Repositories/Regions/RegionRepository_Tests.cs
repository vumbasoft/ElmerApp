using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Repositories.Regions;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class RegionRepository_Tests : ErmanAppEntityFrameworkCoreTestBase
{
    private readonly IRegionRepository _regionRepository;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly IContinentRepository _continentRepository;

    public RegionRepository_Tests()
    {
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateSubcontinentAsync(string suffix)
    {
        var continent = new Continent(Guid.NewGuid(), "Continent " + suffix);
        await _continentRepository.InsertAsync(continent, autoSave: true);

        var subcontinent = new Subcontinent(Guid.NewGuid(), continent.Id, "Subcontinent " + suffix);
        await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

        return subcontinent.Id;
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Region_With_Exact_Name()
    {
        var name = "Repo Region " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var subcontinentId = await CreateSubcontinentAsync(Guid.NewGuid().ToString());
            await _regionRepository.InsertAsync(new Region(Guid.NewGuid(), subcontinentId, name, 5), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _regionRepository.FindByNameAsync(name);

            found.ShouldNotBeNull();
            found.Population.ShouldBe(5);
        });
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Null_When_Not_Found()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _regionRepository.FindByNameAsync("Non existent " + Guid.NewGuid());

            found.ShouldBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_Name()
    {
        var uniqueSuffix = Guid.NewGuid().ToString();
        var matchingName = "Filterable Region " + uniqueSuffix;
        var otherName = "Other Region " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var subcontinentId = await CreateSubcontinentAsync(uniqueSuffix);
            await _regionRepository.InsertAsync(new Region(Guid.NewGuid(), subcontinentId, matchingName), autoSave: true);
            await _regionRepository.InsertAsync(new Region(Guid.NewGuid(), subcontinentId, otherName), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _regionRepository.GetListAsync(filterText: uniqueSuffix);

            result.Count.ShouldBe(1);
            result[0].Name.ShouldBe(matchingName);
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_SubcontinentId()
    {
        var suffix = Guid.NewGuid().ToString();

        Guid subcontinentAId = default, subcontinentBId = default;
        await WithUnitOfWorkAsync(async () =>
        {
            subcontinentAId = await CreateSubcontinentAsync("A " + suffix);
            subcontinentBId = await CreateSubcontinentAsync("B " + suffix);

            await _regionRepository.InsertAsync(new Region(Guid.NewGuid(), subcontinentAId, "Region A " + suffix), autoSave: true);
            await _regionRepository.InsertAsync(new Region(Guid.NewGuid(), subcontinentBId, "Region B " + suffix), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _regionRepository.GetListAsync(filterText: suffix, subcontinentId: subcontinentAId);

            result.Count.ShouldBe(1);
            result[0].SubcontinentId.ShouldBe(subcontinentAId);
        });
    }
}
