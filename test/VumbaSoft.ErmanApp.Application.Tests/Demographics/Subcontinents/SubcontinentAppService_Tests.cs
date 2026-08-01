using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public abstract class SubcontinentAppService_Tests<TStartupModule> : ErmanAppApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ISubcontinentAppService _subcontinentAppService;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected SubcontinentAppService_Tests()
    {
        _subcontinentAppService = GetRequiredService<ISubcontinentAppService>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Continent> CreateContinentAsync()
    {
        Continent? continent = null;
        await WithUnitOfWorkAsync(async () =>
        {
            continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);
        });
        return continent!;
    }

    [Fact]
    public async Task Should_Create_A_Valid_Subcontinent()
    {
        var continent = await CreateContinentAsync();
        var name = "Test Subcontinent " + Guid.NewGuid();

        var result = await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto
        {
            ContinentId = continent.Id,
            Name = name,
            Population = 1000,
            Remarks = "Some remarks"
        });

        result.Id.ShouldNotBe(Guid.Empty);
        result.ContinentId.ShouldBe(continent.Id);
        result.Name.ShouldBe(name);
        result.Population.ShouldBe(1000);
        result.Remarks.ShouldBe("Some remarks");
    }

    [Fact]
    public async Task Should_Not_Create_A_Subcontinent_Without_Name()
    {
        var continent = await CreateContinentAsync();

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto { ContinentId = continent.Id, Name = "" });
        });

        exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Subcontinent_With_NonExistent_Continent()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto
            {
                ContinentId = Guid.NewGuid(),
                Name = "Test Subcontinent " + Guid.NewGuid()
            });
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Subcontinent_With_Duplicate_Name()
    {
        var continent = await CreateContinentAsync();
        var name = "Duplicate Subcontinent " + Guid.NewGuid();
        await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto { ContinentId = continent.Id, Name = name });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto { ContinentId = continent.Id, Name = name });
        });
    }

    [Fact]
    public async Task Should_Get_List_Of_Subcontinents()
    {
        var continent = await CreateContinentAsync();
        var name = "Listed Subcontinent " + Guid.NewGuid();
        var created = await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto { ContinentId = continent.Id, Name = name, Population = 42 });

        var result = await _subcontinentAppService.GetListAsync(new GetSubcontinentsInput { FilterText = name });

        result.Items.ShouldContain(s => s.Id == created.Id && s.Name == name && s.ContinentName == continent.Name);
    }

    [Fact]
    public async Task Should_Get_A_Subcontinent_By_Id()
    {
        var continent = await CreateContinentAsync();
        var created = await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto { ContinentId = continent.Id, Name = "Test Subcontinent " + Guid.NewGuid(), Population = 7 });

        var result = await _subcontinentAppService.GetAsync(created.Id);

        result.Name.ShouldBe(created.Name);
        result.Population.ShouldBe(7);
        result.ContinentName.ShouldBe(continent.Name);
    }

    [Fact]
    public async Task Should_Throw_When_Getting_A_Non_Existent_Subcontinent()
    {
        await Assert.ThrowsAsync<EntityNotFoundException<Subcontinent>>(async () =>
        {
            await _subcontinentAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Update_A_Subcontinent()
    {
        var continent = await CreateContinentAsync();
        var otherContinent = await CreateContinentAsync();
        var created = await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto { ContinentId = continent.Id, Name = "Test Subcontinent " + Guid.NewGuid(), Population = 1 });
        var newName = "Updated Subcontinent " + Guid.NewGuid();

        var updated = await _subcontinentAppService.UpdateAsync(created.Id, new CreateUpdateSubcontinentDto
        {
            ContinentId = otherContinent.Id,
            Name = newName,
            Population = 999,
            Remarks = "Updated remarks"
        });

        updated.Name.ShouldBe(newName);
        updated.ContinentId.ShouldBe(otherContinent.Id);
        updated.Population.ShouldBe(999);
        updated.Remarks.ShouldBe("Updated remarks");
    }

    [Fact]
    public async Task Should_Delete_A_Subcontinent()
    {
        var continent = await CreateContinentAsync();
        var created = await _subcontinentAppService.CreateAsync(new CreateUpdateSubcontinentDto { ContinentId = continent.Id, Name = "Test Subcontinent " + Guid.NewGuid() });

        await _subcontinentAppService.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<EntityNotFoundException<Subcontinent>>(async () =>
        {
            await _subcontinentAppService.GetAsync(created.Id);
        });
    }
}
