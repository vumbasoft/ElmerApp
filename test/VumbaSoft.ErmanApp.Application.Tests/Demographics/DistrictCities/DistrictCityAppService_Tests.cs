using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.DistrictCities;

public abstract class DistrictCityAppService_Tests<TStartupModule> : ErmanAppApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IDistrictCityAppService _districtCityAppService;
    private readonly StateProvinceManager _stateProvinceManager;
    private readonly IStateProvinceRepository _stateProvinceRepository;
    private readonly CountryManager _countryManager;
    private readonly ICountryRepository _countryRepository;
    private readonly RegionManager _regionManager;
    private readonly IRegionRepository _regionRepository;
    private readonly SubcontinentManager _subcontinentManager;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly ContinentManager _continentManager;
    private readonly IContinentRepository _continentRepository;

    protected DistrictCityAppService_Tests()
    {
        _districtCityAppService = GetRequiredService<IDistrictCityAppService>();
        _stateProvinceManager = GetRequiredService<StateProvinceManager>();
        _stateProvinceRepository = GetRequiredService<IStateProvinceRepository>();
        _countryManager = GetRequiredService<CountryManager>();
        _countryRepository = GetRequiredService<ICountryRepository>();
        _regionManager = GetRequiredService<RegionManager>();
        _regionRepository = GetRequiredService<IRegionRepository>();
        _subcontinentManager = GetRequiredService<SubcontinentManager>();
        _subcontinentRepository = GetRequiredService<ISubcontinentRepository>();
        _continentManager = GetRequiredService<ContinentManager>();
        _continentRepository = GetRequiredService<IContinentRepository>();
    }

    private async Task<StateProvince> CreateStateProvinceAsync()
    {
        StateProvince? stateProvince = null;
        await WithUnitOfWorkAsync(async () =>
        {
            var continent = await _continentManager.CreateAsync("Continent " + Guid.NewGuid());
            await _continentRepository.InsertAsync(continent, autoSave: true);

            var subcontinent = await _subcontinentManager.CreateAsync(continent.Id, "Subcontinent " + Guid.NewGuid());
            await _subcontinentRepository.InsertAsync(subcontinent, autoSave: true);

            var region = await _regionManager.CreateAsync(subcontinent.Id, "Region " + Guid.NewGuid());
            await _regionRepository.InsertAsync(region, autoSave: true);

            var country = await _countryManager.CreateAsync(region.Id, "Country " + Guid.NewGuid());
            await _countryRepository.InsertAsync(country, autoSave: true);

            stateProvince = await _stateProvinceManager.CreateAsync(country.Id, "StateProvince " + Guid.NewGuid());
            await _stateProvinceRepository.InsertAsync(stateProvince, autoSave: true);
        });
        return stateProvince!;
    }

    [Fact]
    public async Task Should_Create_A_Valid_DistrictCity()
    {
        var stateProvince = await CreateStateProvinceAsync();
        var name = "Test DistrictCity " + Guid.NewGuid();

        var result = await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto
        {
            StateProvinceId = stateProvince.Id,
            Name = name,
            Population = 1000,
            Remarks = "Some remarks",
            CountryCode = "TC",
            Latitude = 12.34m,
            Longitude = 56.78m
        });

        result.Id.ShouldNotBe(Guid.Empty);
        result.StateProvinceId.ShouldBe(stateProvince.Id);
        result.Name.ShouldBe(name);
        result.CountryCode.ShouldBe("TC");
        result.Latitude.ShouldBe(12.34m);
        result.Longitude.ShouldBe(56.78m);
    }

    [Fact]
    public async Task Should_Not_Create_A_DistrictCity_Without_Name()
    {
        var stateProvince = await CreateStateProvinceAsync();

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto { StateProvinceId = stateProvince.Id, Name = "" });
        });

        exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_DistrictCity_With_NonExistent_StateProvince()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto
            {
                StateProvinceId = Guid.NewGuid(),
                Name = "Test DistrictCity " + Guid.NewGuid()
            });
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_DistrictCity_With_Duplicate_Name()
    {
        var stateProvince = await CreateStateProvinceAsync();
        var name = "Duplicate DistrictCity " + Guid.NewGuid();
        await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto { StateProvinceId = stateProvince.Id, Name = name });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto { StateProvinceId = stateProvince.Id, Name = name });
        });
    }

    [Fact]
    public async Task Should_Get_List_Of_DistrictCities()
    {
        var stateProvince = await CreateStateProvinceAsync();
        var name = "Listed DistrictCity " + Guid.NewGuid();
        var created = await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto { StateProvinceId = stateProvince.Id, Name = name, Population = 42 });

        var result = await _districtCityAppService.GetListAsync(new GetDistrictCitiesInput { FilterText = name });

        result.Items.ShouldContain(d => d.Id == created.Id && d.Name == name && d.StateProvinceName == stateProvince.Name);
    }

    [Fact]
    public async Task Should_Get_A_DistrictCity_By_Id()
    {
        var stateProvince = await CreateStateProvinceAsync();
        var created = await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto { StateProvinceId = stateProvince.Id, Name = "Test DistrictCity " + Guid.NewGuid(), Population = 7 });

        var result = await _districtCityAppService.GetAsync(created.Id);

        result.Name.ShouldBe(created.Name);
        result.Population.ShouldBe(7);
        result.StateProvinceName.ShouldBe(stateProvince.Name);
    }

    [Fact]
    public async Task Should_Throw_When_Getting_A_Non_Existent_DistrictCity()
    {
        await Assert.ThrowsAsync<EntityNotFoundException<DistrictCity>>(async () =>
        {
            await _districtCityAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Update_A_DistrictCity()
    {
        var stateProvince = await CreateStateProvinceAsync();
        var otherStateProvince = await CreateStateProvinceAsync();
        var created = await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto { StateProvinceId = stateProvince.Id, Name = "Test DistrictCity " + Guid.NewGuid(), Population = 1 });
        var newName = "Updated DistrictCity " + Guid.NewGuid();

        var updated = await _districtCityAppService.UpdateAsync(created.Id, new CreateUpdateDistrictCityDto
        {
            StateProvinceId = otherStateProvince.Id,
            Name = newName,
            Population = 999,
            Remarks = "Updated remarks",
            CountryCode = "UP",
            Latitude = 1,
            Longitude = 2
        });

        updated.Name.ShouldBe(newName);
        updated.StateProvinceId.ShouldBe(otherStateProvince.Id);
        updated.Population.ShouldBe(999);
        updated.CountryCode.ShouldBe("UP");
    }

    [Fact]
    public async Task Should_Delete_A_DistrictCity()
    {
        var stateProvince = await CreateStateProvinceAsync();
        var created = await _districtCityAppService.CreateAsync(new CreateUpdateDistrictCityDto { StateProvinceId = stateProvince.Id, Name = "Test DistrictCity " + Guid.NewGuid() });

        await _districtCityAppService.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<EntityNotFoundException<DistrictCity>>(async () =>
        {
            await _districtCityAppService.GetAsync(created.Id);
        });
    }
}
