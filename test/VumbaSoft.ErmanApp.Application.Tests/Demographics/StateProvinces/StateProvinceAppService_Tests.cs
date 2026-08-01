using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.StateProvinces;

public abstract class StateProvinceAppService_Tests<TStartupModule> : ErmanAppApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IStateProvinceAppService _stateProvinceAppService;
    private readonly CountryManager _countryManager;
    private readonly ICountryRepository _countryRepository;
    private readonly RegionManager _regionManager;
    private readonly IRegionRepository _regionRepository;
    private readonly SubcontinentManager _subcontinentManager;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected StateProvinceAppService_Tests()
    {
        _stateProvinceAppService = GetRequiredService<IStateProvinceAppService>();
        _countryManager = GetRequiredService<CountryManager>();
        _countryRepository = GetRequiredService<ICountryRepository>();
        _regionManager = GetRequiredService<RegionManager>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentManager = GetRequiredService<SubcontinentManager>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<Country> CreateCountryAsync()
    {
        Country? country = null;
        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);

            var subcontinent = await _subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

            var region = await _regionManager.CreateAsync(subcontinent.Id, "Region " + Guid.NewGuid());
            await _regionRepository.InsertAsync(region, autoSave: true);

            country = await _countryManager.CreateAsync(region.Id, "Country " + Guid.NewGuid());
            await _countryRepository.InsertAsync(country, autoSave: true);
        });
        return country!;
    }

    [Fact]
    public async Task Should_Create_A_Valid_StateProvince()
    {
        var country = await CreateCountryAsync();
        var name = "Test StateProvince " + Guid.NewGuid();

        var result = await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto
        {
            CountryId = country.Id,
            Name = name,
            Population = 1000,
            Remarks = "Some remarks",
            RegionCode = "RC",
            StateProvinceCode = "SP"
        });

        result.Id.ShouldNotBe(Guid.Empty);
        result.CountryId.ShouldBe(country.Id);
        result.Name.ShouldBe(name);
        result.Population.ShouldBe(1000);
        result.RegionCode.ShouldBe("RC");
        result.StateProvinceCode.ShouldBe("SP");
    }

    [Fact]
    public async Task Should_Not_Create_A_StateProvince_Without_Name()
    {
        var country = await CreateCountryAsync();

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto { CountryId = country.Id, Name = "" });
        });

        exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_StateProvince_With_NonExistent_Country()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto
            {
                CountryId = Guid.NewGuid(),
                Name = "Test StateProvince " + Guid.NewGuid()
            });
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_StateProvince_With_Duplicate_Name()
    {
        var country = await CreateCountryAsync();
        var name = "Duplicate StateProvince " + Guid.NewGuid();
        await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto { CountryId = country.Id, Name = name });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto { CountryId = country.Id, Name = name });
        });
    }

    [Fact]
    public async Task Should_Get_List_Of_StateProvinces()
    {
        var country = await CreateCountryAsync();
        var name = "Listed StateProvince " + Guid.NewGuid();
        var created = await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto { CountryId = country.Id, Name = name, Population = 42 });

        var result = await _stateProvinceAppService.GetListAsync(new GetStateProvincesInput { FilterText = name });

        result.Items.ShouldContain(s => s.Id == created.Id && s.Name == name && s.CountryName == country.Name);
    }

    [Fact]
    public async Task Should_Get_A_StateProvince_By_Id()
    {
        var country = await CreateCountryAsync();
        var created = await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto { CountryId = country.Id, Name = "Test StateProvince " + Guid.NewGuid(), Population = 7 });

        var result = await _stateProvinceAppService.GetAsync(created.Id);

        result.Name.ShouldBe(created.Name);
        result.Population.ShouldBe(7);
        result.CountryName.ShouldBe(country.Name);
    }

    [Fact]
    public async Task Should_Throw_When_Getting_A_Non_Existent_StateProvince()
    {
        await Assert.ThrowsAsync<EntityNotFoundException<StateProvince>>(async () =>
        {
            await _stateProvinceAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Update_A_StateProvince()
    {
        var country = await CreateCountryAsync();
        var otherCountry = await CreateCountryAsync();
        var created = await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto { CountryId = country.Id, Name = "Test StateProvince " + Guid.NewGuid(), Population = 1 });
        var newName = "Updated StateProvince " + Guid.NewGuid();

        var updated = await _stateProvinceAppService.UpdateAsync(created.Id, new CreateUpdateStateProvinceDto
        {
            CountryId = otherCountry.Id,
            Name = newName,
            Population = 999,
            Remarks = "Updated remarks",
            RegionCode = "UR",
            StateProvinceCode = "UP"
        });

        updated.Name.ShouldBe(newName);
        updated.CountryId.ShouldBe(otherCountry.Id);
        updated.Population.ShouldBe(999);
        updated.RegionCode.ShouldBe("UR");
        updated.StateProvinceCode.ShouldBe("UP");
    }

    [Fact]
    public async Task Should_Delete_A_StateProvince()
    {
        var country = await CreateCountryAsync();
        var created = await _stateProvinceAppService.CreateAsync(new CreateUpdateStateProvinceDto { CountryId = country.Id, Name = "Test StateProvince " + Guid.NewGuid() });

        await _stateProvinceAppService.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<EntityNotFoundException<StateProvince>>(async () =>
        {
            await _stateProvinceAppService.GetAsync(created.Id);
        });
    }
}
