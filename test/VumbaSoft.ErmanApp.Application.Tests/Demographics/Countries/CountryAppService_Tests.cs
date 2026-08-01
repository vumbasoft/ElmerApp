using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Countries;

public abstract class CountryAppService_Tests<TStartupModule> : ErmanAppApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ICountryAppService _countryAppService;
    private readonly RegionManager _regionManager;
    private readonly IRegionRepository _regionRepository;
    private readonly SubcontinentManager _subcontinentManager;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected CountryAppService_Tests()
    {
        _countryAppService = GetRequiredService<ICountryAppService>();
        _regionManager = GetRequiredService<RegionManager>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentManager = GetRequiredService<SubcontinentManager>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Region> CreateRegionAsync()
    {
        Region? region = null;
        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);

            var subcontinent = await _subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

            region = await _regionManager.CreateAsync(subcontinent.Id, "Region " + Guid.NewGuid());
            await _regionRepository.InsertAsync(region, autoSave: true);
        });
        return region!;
    }

    [Fact]
    public async Task Should_Create_A_Valid_Country()
    {
        var region = await CreateRegionAsync();
        var name = "Test Country " + Guid.NewGuid();

        var result = await _countryAppService.CreateAsync(new CreateUpdateCountryDto
        {
            RegionId = region.Id,
            Name = name,
            Population = 1000,
            Remarks = "Some remarks",
            FormalName = "Republic of Test",
            NativeName = "Native Test",
            ISO3 = "TST",
            ISO2 = "TS",
            CCN3 = "123",
            PhoneCode = "+1",
            Capital = "Test City",
            Currency = "TSD",
            Emoji = "🇹🇸",
            EmojiU = "U+1F1F9 U+1F1F8"
        });

        result.Id.ShouldNotBe(Guid.Empty);
        result.RegionId.ShouldBe(region.Id);
        result.Name.ShouldBe(name);
        result.Population.ShouldBe(1000);
        result.FormalName.ShouldBe("Republic of Test");
        result.ISO3.ShouldBe("TST");
        result.Capital.ShouldBe("Test City");
    }

    [Fact]
    public async Task Should_Not_Create_A_Country_Without_Name()
    {
        var region = await CreateRegionAsync();

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _countryAppService.CreateAsync(new CreateUpdateCountryDto { RegionId = region.Id, Name = "" });
        });

        exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Country_With_NonExistent_Region()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _countryAppService.CreateAsync(new CreateUpdateCountryDto
            {
                RegionId = Guid.NewGuid(),
                Name = "Test Country " + Guid.NewGuid()
            });
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Country_With_Duplicate_Name()
    {
        var region = await CreateRegionAsync();
        var name = "Duplicate Country " + Guid.NewGuid();
        await _countryAppService.CreateAsync(new CreateUpdateCountryDto { RegionId = region.Id, Name = name });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _countryAppService.CreateAsync(new CreateUpdateCountryDto { RegionId = region.Id, Name = name });
        });
    }

    [Fact]
    public async Task Should_Get_List_Of_Countries()
    {
        var region = await CreateRegionAsync();
        var name = "Listed Country " + Guid.NewGuid();
        var created = await _countryAppService.CreateAsync(new CreateUpdateCountryDto { RegionId = region.Id, Name = name, Population = 42 });

        var result = await _countryAppService.GetListAsync(new GetCountriesInput { FilterText = name });

        result.Items.ShouldContain(c => c.Id == created.Id && c.Name == name && c.RegionName == region.Name);
    }

    [Fact]
    public async Task Should_Get_A_Country_By_Id()
    {
        var region = await CreateRegionAsync();
        var created = await _countryAppService.CreateAsync(new CreateUpdateCountryDto { RegionId = region.Id, Name = "Test Country " + Guid.NewGuid(), Population = 7 });

        var result = await _countryAppService.GetAsync(created.Id);

        result.Name.ShouldBe(created.Name);
        result.Population.ShouldBe(7);
        result.RegionName.ShouldBe(region.Name);
    }

    [Fact]
    public async Task Should_Throw_When_Getting_A_Non_Existent_Country()
    {
        await Assert.ThrowsAsync<EntityNotFoundException<Country>>(async () =>
        {
            await _countryAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Update_A_Country()
    {
        var region = await CreateRegionAsync();
        var otherRegion = await CreateRegionAsync();
        var created = await _countryAppService.CreateAsync(new CreateUpdateCountryDto { RegionId = region.Id, Name = "Test Country " + Guid.NewGuid(), Population = 1 });
        var newName = "Updated Country " + Guid.NewGuid();

        var updated = await _countryAppService.UpdateAsync(created.Id, new CreateUpdateCountryDto
        {
            RegionId = otherRegion.Id,
            Name = newName,
            Population = 999,
            Remarks = "Updated remarks",
            FormalName = "Updated Formal Name",
            ISO2 = "UP"
        });

        updated.Name.ShouldBe(newName);
        updated.RegionId.ShouldBe(otherRegion.Id);
        updated.Population.ShouldBe(999);
        updated.Remarks.ShouldBe("Updated remarks");
        updated.FormalName.ShouldBe("Updated Formal Name");
        updated.ISO2.ShouldBe("UP");
    }

    [Fact]
    public async Task Should_Delete_A_Country()
    {
        var region = await CreateRegionAsync();
        var created = await _countryAppService.CreateAsync(new CreateUpdateCountryDto { RegionId = region.Id, Name = "Test Country " + Guid.NewGuid() });

        await _countryAppService.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<EntityNotFoundException<Country>>(async () =>
        {
            await _countryAppService.GetAsync(created.Id);
        });
    }
}
