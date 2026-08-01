using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Repositories.Subcontinents;

[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class SubcontinentRepository_Tests : ErmanAppEntityFrameworkCoreTestBase
{
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly IContinentRepository _continentRepository;

    public SubcontinentRepository_Tests()
    {
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Subcontinent_With_Exact_Name()
    {
        var name = "Repo Subcontinent " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var continent = new Continent(Guid.NewGuid(), "Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);
            await _subcontinentRepository.InsertAsync(new Subcontinent(Guid.NewGuid(), continent.Id, name, 5), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _subcontinentRepository.FindByNameAsync(name);

            found.ShouldNotBeNull();
            found.Population.ShouldBe(5);
        });
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Null_When_Not_Found()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _subcontinentRepository.FindByNameAsync("Non existent " + Guid.NewGuid());

            found.ShouldBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_Name()
    {
        var uniqueSuffix = Guid.NewGuid().ToString();
        var matchingName = "Filterable Subcontinent " + uniqueSuffix;
        var otherName = "Other Subcontinent " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var continent = new Continent(Guid.NewGuid(), "Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);
            await _subcontinentRepository.InsertAsync(new Subcontinent(Guid.NewGuid(), continent.Id, matchingName), autoSave: true);
            await _subcontinentRepository.InsertAsync(new Subcontinent(Guid.NewGuid(), continent.Id, otherName), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _subcontinentRepository.GetListAsync(filterText: uniqueSuffix);

            result.Count.ShouldBe(1);
            result[0].Name.ShouldBe(matchingName);
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_ContinentId()
    {
        var suffix = Guid.NewGuid().ToString();

        Guid continentAId = default, continentBId = default;
        await WithUnitOfWorkAsync(async () =>
        {
            var continentA = new Continent(Guid.NewGuid(), "Continent A " + suffix);
            var continentB = new Continent(Guid.NewGuid(), "Continent B " + suffix);
            await _continentRepository.InsertAsync(continentA, autoSave: true);
            await _continentRepository.InsertAsync(continentB, autoSave: true);
            continentAId = continentA.Id;
            continentBId = continentB.Id;

            await _subcontinentRepository.InsertAsync(new Subcontinent(Guid.NewGuid(), continentAId, "Subcontinent A " + suffix), autoSave: true);
            await _subcontinentRepository.InsertAsync(new Subcontinent(Guid.NewGuid(), continentBId, "Subcontinent B " + suffix), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _subcontinentRepository.GetListAsync(filterText: suffix, continentId: continentAId);

            result.Count.ShouldBe(1);
            result[0].ContinentId.ShouldBe(continentAId);
        });
    }
}
