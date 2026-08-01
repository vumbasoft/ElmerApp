using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public abstract class ContinentManager_Tests<TStartupModule> : ErmanAppDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected ContinentManager_Tests()
    {
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    [Fact]
    public async Task Should_Create_A_Valid_Continent()
    {
        var continent = await _continentManager.CreateAsync("Test Continent " + Guid.NewGuid(), 1000, "Some remarks");

        continent.Id.ShouldNotBe(Guid.Empty);
        continent.Population.ShouldBe(1000);
        continent.Remarks.ShouldBe("Some remarks");
    }

    [Fact]
    public async Task Should_Create_A_Continent_Without_Remarks()
    {
        var continent = await _continentManager.CreateAsync("Test Continent " + Guid.NewGuid());

        continent.Population.ShouldBe(0);
        continent.Remarks.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_A_Continent_With_Duplicate_Name()
    {
        var name = "Duplicate Continent " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync(name, 100);
            await _continentRepository.InsertAsync(continent, autoSave: true);
        });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _continentManager.CreateAsync(name, 200);
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Continent_With_Empty_Name()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _continentManager.CreateAsync("");
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Continent_With_Negative_Population()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _continentManager.CreateAsync("Test Continent " + Guid.NewGuid(), -1);
        });
    }
}
