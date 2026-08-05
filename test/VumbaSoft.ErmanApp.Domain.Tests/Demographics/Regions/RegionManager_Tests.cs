using System;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public abstract class RegionManager_Tests<TStartupModule> : ErmanAppDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly RegionManager _regionManager;
    private readonly IRegionRepository _regionRepository;
    private readonly SubcontinentManager _subcontinentManager;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected RegionManager_Tests()
    {
        _regionManager = GetRequiredService<RegionManager>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentManager = GetRequiredService<SubcontinentManager>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Guid> CreateSubcontinentAsync()
    {
        Subcontinent? subcontinent = null;
        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);

            subcontinent = await _subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);
        });
        return subcontinent!.Id;
    }

    [Fact]
    public async Task Should_Create_A_Valid_Region()
    {
        var subcontinentId = await CreateSubcontinentAsync();

        var region = await _regionManager.CreateAsync(subcontinentId, "Test Region " + Guid.NewGuid(), 1000, "Some remarks");

        region.Id.ShouldNotBe(Guid.Empty);
        region.SubcontinentId.ShouldBe(subcontinentId);
        region.Population.ShouldBe(1000);
        region.Remarks.ShouldBe("Some remarks");
    }

    [Fact]
    public async Task Should_Create_A_Region_Without_Remarks()
    {
        var subcontinentId = await CreateSubcontinentAsync();

        var region = await _regionManager.CreateAsync(subcontinentId, "Test Region " + Guid.NewGuid());

        region.Population.ShouldBe(0);
        region.Remarks.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_A_Region_With_NonExistent_Subcontinent()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _regionManager.CreateAsync(Guid.NewGuid(), "Test Region " + Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Region_With_Duplicate_Name()
    {
        var subcontinentId = await CreateSubcontinentAsync();
        var name = "Duplicate Region " + Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            var region = await _regionManager.CreateAsync(subcontinentId, name);
            await _regionRepository.InsertAsync(region, autoSave: true);
        });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _regionManager.CreateAsync(subcontinentId, name);
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Region_With_Empty_Name()
    {
        var subcontinentId = await CreateSubcontinentAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _regionManager.CreateAsync(subcontinentId, "");
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Region_With_Negative_Population()
    {
        var subcontinentId = await CreateSubcontinentAsync();

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _regionManager.CreateAsync(subcontinentId, "Test Region " + Guid.NewGuid(), -1);
        });
    }
}
