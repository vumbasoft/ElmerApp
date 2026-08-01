using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public abstract class RegionAppService_Tests<TStartupModule> : ErmanAppApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IRegionAppService _regionAppService;
    private readonly SubcontinentManager _subcontinentManager;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected RegionAppService_Tests()
    {
        _regionAppService = GetRequiredService<IRegionAppService>();
        _subcontinentManager = GetRequiredService<SubcontinentManager>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Subcontinent> CreateSubcontinentAsync()
    {
        Subcontinent? subcontinent = null;
        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);

            subcontinent = await _subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);
        });
        return subcontinent!;
    }

    [Fact]
    public async Task Should_Create_A_Valid_Region()
    {
        var subcontinent = await CreateSubcontinentAsync();
        var name = "Test Region " + Guid.NewGuid();

        var result = await _regionAppService.CreateAsync(new CreateUpdateRegionDto
        {
            SubcontinentId = subcontinent.Id,
            Name = name,
            Population = 1000,
            Remarks = "Some remarks"
        });

        result.Id.ShouldNotBe(Guid.Empty);
        result.SubcontinentId.ShouldBe(subcontinent.Id);
        result.Name.ShouldBe(name);
        result.Population.ShouldBe(1000);
        result.Remarks.ShouldBe("Some remarks");
    }

    [Fact]
    public async Task Should_Not_Create_A_Region_Without_Name()
    {
        var subcontinent = await CreateSubcontinentAsync();

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _regionAppService.CreateAsync(new CreateUpdateRegionDto { SubcontinentId = subcontinent.Id, Name = "" });
        });

        exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Region_With_NonExistent_Subcontinent()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _regionAppService.CreateAsync(new CreateUpdateRegionDto
            {
                SubcontinentId = Guid.NewGuid(),
                Name = "Test Region " + Guid.NewGuid()
            });
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Region_With_Duplicate_Name()
    {
        var subcontinent = await CreateSubcontinentAsync();
        var name = "Duplicate Region " + Guid.NewGuid();
        await _regionAppService.CreateAsync(new CreateUpdateRegionDto { SubcontinentId = subcontinent.Id, Name = name });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _regionAppService.CreateAsync(new CreateUpdateRegionDto { SubcontinentId = subcontinent.Id, Name = name });
        });
    }

    [Fact]
    public async Task Should_Get_List_Of_Regions()
    {
        var subcontinent = await CreateSubcontinentAsync();
        var name = "Listed Region " + Guid.NewGuid();
        var created = await _regionAppService.CreateAsync(new CreateUpdateRegionDto { SubcontinentId = subcontinent.Id, Name = name, Population = 42 });

        var result = await _regionAppService.GetListAsync(new GetRegionsInput { FilterText = name });

        result.Items.ShouldContain(r => r.Id == created.Id && r.Name == name && r.SubcontinentName == subcontinent.Name);
    }

    [Fact]
    public async Task Should_Get_A_Region_By_Id()
    {
        var subcontinent = await CreateSubcontinentAsync();
        var created = await _regionAppService.CreateAsync(new CreateUpdateRegionDto { SubcontinentId = subcontinent.Id, Name = "Test Region " + Guid.NewGuid(), Population = 7 });

        var result = await _regionAppService.GetAsync(created.Id);

        result.Name.ShouldBe(created.Name);
        result.Population.ShouldBe(7);
        result.SubcontinentName.ShouldBe(subcontinent.Name);
    }

    [Fact]
    public async Task Should_Throw_When_Getting_A_Non_Existent_Region()
    {
        await Assert.ThrowsAsync<EntityNotFoundException<Region>>(async () =>
        {
            await _regionAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Update_A_Region()
    {
        var subcontinent = await CreateSubcontinentAsync();
        var otherSubcontinent = await CreateSubcontinentAsync();
        var created = await _regionAppService.CreateAsync(new CreateUpdateRegionDto { SubcontinentId = subcontinent.Id, Name = "Test Region " + Guid.NewGuid(), Population = 1 });
        var newName = "Updated Region " + Guid.NewGuid();

        var updated = await _regionAppService.UpdateAsync(created.Id, new CreateUpdateRegionDto
        {
            SubcontinentId = otherSubcontinent.Id,
            Name = newName,
            Population = 999,
            Remarks = "Updated remarks"
        });

        updated.Name.ShouldBe(newName);
        updated.SubcontinentId.ShouldBe(otherSubcontinent.Id);
        updated.Population.ShouldBe(999);
        updated.Remarks.ShouldBe("Updated remarks");
    }

    [Fact]
    public async Task Should_Delete_A_Region()
    {
        var subcontinent = await CreateSubcontinentAsync();
        var created = await _regionAppService.CreateAsync(new CreateUpdateRegionDto { SubcontinentId = subcontinent.Id, Name = "Test Region " + Guid.NewGuid() });

        await _regionAppService.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<EntityNotFoundException<Region>>(async () =>
        {
            await _regionAppService.GetAsync(created.Id);
        });
    }
}
