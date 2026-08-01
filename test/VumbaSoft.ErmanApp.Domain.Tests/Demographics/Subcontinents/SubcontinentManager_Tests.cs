using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public abstract class SubcontinentManager_Tests<TStartupModule> : ErmanAppDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly SubcontinentManager _subcontinentManager;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected SubcontinentManager_Tests()
    {
        _subcontinentManager = GetRequiredService<SubcontinentManager>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateContinentAsync()
    {
        Continent? continent = null;
        await WithUnitOfWorkAsync(async () =>
        {
            continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);
        });
        return continent!.Id;
    }

    [Fact]
    public async Task Should_Create_A_Valid_Subcontinent()
    {
        var continentId = await CreateContinentAsync();

        var subcontinent = await _subcontinentManager.CreateAsync(continentId, "Test Subcontinent " + Guid.NewGuid(), 1000, "Some remarks");

        subcontinent.Id.ShouldNotBe(Guid.Empty);
        subcontinent.ContinentId.ShouldBe(continentId);
        subcontinent.Population.ShouldBe(1000);
        subcontinent.Remarks.ShouldBe("Some remarks");
    }

    [Fact]
    public async Task Should_Create_A_Subcontinent_Without_Remarks()
    {
        var continentId = await CreateContinentAsync();

        var subcontinent = await _subcontinentManager.CreateAsync(continentId, "Test Subcontinent " + Guid.NewGuid());

        subcontinent.Population.ShouldBe(0);
        subcontinent.Remarks.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_A_Subcontinent_With_NonExistent_Continent()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _subcontinentManager.CreateAsync(Guid.NewGuid(), "Test Subcontinent " + Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Subcontinent_With_Duplicate_Name()
    {
        var continentId = await CreateContinentAsync();
        var name = "Duplicate Subcontinent " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var subcontinent = await _subcontinentManager.CreateAsync(continentId, name);
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);
        });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _subcontinentManager.CreateAsync(continentId, name);
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Subcontinent_With_Empty_Name()
    {
        var continentId = await CreateContinentAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _subcontinentManager.CreateAsync(continentId, "");
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Subcontinent_With_Negative_Population()
    {
        var continentId = await CreateContinentAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _subcontinentManager.CreateAsync(continentId, "Test Subcontinent " + Guid.NewGuid(), -1);
        });
    }
}
