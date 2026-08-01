using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using Xunit;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore.Repositories.Continents;

/* Only test custom repository methods here (FindByNameAsync / GetListAsync) -
 * generic IRepository<> members are ABP framework code and already covered by ABP itself. */
[Collection(ErmanAppTestConsts.CollectionDefinitionName)]
public class ContinentRepository_Tests : ErmanAppEntityFrameworkCoreTestBase
{
    private readonly IContinentRepository _continentRepository;

    public ContinentRepository_Tests()
    {
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Continent_With_Exact_Name()
    {
        var name = "Repo Continent " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _continentRepository.InsertAsync(new Continent(Guid.NewGuid(), name, 5), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _continentRepository.FindByNameAsync(name);

            found.ShouldNotBeNull();
            found.Population.ShouldBe(5);
        });
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Null_When_Not_Found()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var found = await _continentRepository.FindByNameAsync("Non existent " + Guid.NewGuid());

            found.ShouldBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Filter_By_Name()
    {
        var uniqueSuffix = Guid.NewGuid().ToString();
        var matchingName = "Filterable Continent " + uniqueSuffix;
        var otherName = "Other Continent " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _continentRepository.InsertAsync(new Continent(Guid.NewGuid(), matchingName), autoSave: true);
            await _continentRepository.InsertAsync(new Continent(Guid.NewGuid(), otherName), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _continentRepository.GetListAsync(filterText: uniqueSuffix);

            result.Count.ShouldBe(1);
            result[0].Name.ShouldBe(matchingName);
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Respect_Paging()
    {
        var prefix = "Paged Continent " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            for (var i = 0; i < 3; i++)
            {
                await _continentRepository.InsertAsync(new Continent(Guid.NewGuid(), $"{prefix} {i}"), autoSave: true);
            }
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var page = await _continentRepository.GetListAsync(filterText: prefix, maxResultCount: 2, skipCount: 0, sorting: "Name asc");

            page.Count.ShouldBe(2);
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Sort_By_Requested_Column()
    {
        var prefix = "Sorted Continent " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _continentRepository.InsertAsync(new Continent(Guid.NewGuid(), $"{prefix} B", 2), autoSave: true);
            await _continentRepository.InsertAsync(new Continent(Guid.NewGuid(), $"{prefix} A", 1), autoSave: true);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _continentRepository.GetListAsync(filterText: prefix, sorting: "Name asc");

            result.Count.ShouldBe(2);
            result[0].Name.ShouldBe($"{prefix} A");
            result[1].Name.ShouldBe($"{prefix} B");
        });
    }
}
