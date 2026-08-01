using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public abstract class ContinentAppService_Tests<TStartupModule> : ErmanAppApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IContinentAppService _continentAppService;

    protected ContinentAppService_Tests()
    {
        _continentAppService = GetRequiredService<IContinentAppService>();
    }

    [Fact]
    public async Task Should_Create_A_Valid_Continent()
    {
        var name = "Test Continent " + Guid.NewGuid();

        var result = await _continentAppService.CreateAsync(new CreateUpdateContinentDto
        {
            Name = name,
            Population = 1000,
            Remarks = "Some remarks"
        });

        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe(name);
        result.Population.ShouldBe(1000);
        result.Remarks.ShouldBe("Some remarks");
    }

    [Fact]
    public async Task Should_Not_Create_A_Continent_Without_Name()
    {
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _continentAppService.CreateAsync(new CreateUpdateContinentDto { Name = "", Population = 10 });
        });

        exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Continent_With_Duplicate_Name()
    {
        var name = "Duplicate Continent " + Guid.NewGuid();
        await _continentAppService.CreateAsync(new CreateUpdateContinentDto { Name = name });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _continentAppService.CreateAsync(new CreateUpdateContinentDto { Name = name });
        });
    }

    [Fact]
    public async Task Should_Get_List_Of_Continents()
    {
        var name = "Listed Continent " + Guid.NewGuid();
        var created = await _continentAppService.CreateAsync(new CreateUpdateContinentDto { Name = name, Population = 42 });

        var result = await _continentAppService.GetListAsync(new GetContinentsInput { FilterText = name });

        result.Items.ShouldContain(c => c.Id == created.Id && c.Name == name);
    }

    [Fact]
    public async Task Should_Get_A_Continent_By_Id()
    {
        var created = await _continentAppService.CreateAsync(new CreateUpdateContinentDto { Name = "Test Continent " + Guid.NewGuid(), Population = 7 });

        var result = await _continentAppService.GetAsync(created.Id);

        result.Name.ShouldBe(created.Name);
        result.Population.ShouldBe(7);
    }

    [Fact]
    public async Task Should_Throw_When_Getting_A_Non_Existent_Continent()
    {
        await Assert.ThrowsAsync<EntityNotFoundException<Continent>>(async () =>
        {
            await _continentAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Update_A_Continent()
    {
        var created = await _continentAppService.CreateAsync(new CreateUpdateContinentDto { Name = "Test Continent " + Guid.NewGuid(), Population = 1 });
        var newName = "Updated Continent " + Guid.NewGuid();

        var updated = await _continentAppService.UpdateAsync(created.Id, new CreateUpdateContinentDto
        {
            Name = newName,
            Population = 999,
            Remarks = "Updated remarks"
        });

        updated.Name.ShouldBe(newName);
        updated.Population.ShouldBe(999);
        updated.Remarks.ShouldBe("Updated remarks");

        var reloaded = await _continentAppService.GetAsync(created.Id);
        reloaded.Name.ShouldBe(newName);
    }

    [Fact]
    public async Task Should_Not_Update_A_Continent_With_Negative_Population()
    {
        var created = await _continentAppService.CreateAsync(new CreateUpdateContinentDto { Name = "Test Continent " + Guid.NewGuid() });

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _continentAppService.UpdateAsync(created.Id, new CreateUpdateContinentDto
            {
                Name = created.Name,
                Population = -5
            });
        });
    }

    [Fact]
    public async Task Should_Delete_A_Continent()
    {
        var created = await _continentAppService.CreateAsync(new CreateUpdateContinentDto { Name = "Test Continent " + Guid.NewGuid() });

        await _continentAppService.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<EntityNotFoundException<Continent>>(async () =>
        {
            await _continentAppService.GetAsync(created.Id);
        });
    }
}
