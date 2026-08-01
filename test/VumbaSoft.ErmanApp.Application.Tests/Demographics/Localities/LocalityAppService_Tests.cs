using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace VumbaSoft.ErmanApp.Demographics.Localities;

public abstract class LocalityAppService_Tests<TStartupModule> : ErmanAppApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ILocalityAppService _localityAppService;
    private readonly DistrictCityManager _districtCityManager;
    private readonly IDistrictCityRepository _districtCityRepository;
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

    protected LocalityAppService_Tests()
    {
        _localityAppService = GetRequiredService<ILocalityAppService>();
        _districtCityManager = GetRequiredService<DistrictCityManager>();
        _districtCityRepository = GetRequiredService<IDistrictCityRepository>();
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

    private async Task<DistrictCity> CreateDistrictCityAsync()
    {
        DistrictCity? districtCity = null;
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

            var stateProvince = await _stateProvinceManager.CreateAsync(country.Id, "StateProvince " + Guid.NewGuid());
            await _stateProvinceRepository.InsertAsync(stateProvince, autoSave: true);

            districtCity = await _districtCityManager.CreateAsync(stateProvince.Id, "DistrictCity " + Guid.NewGuid());
            await _districtCityRepository.InsertAsync(districtCity, autoSave: true);
        });
        return districtCity!;
    }

    [Fact]
    public async Task Should_Create_A_Valid_Locality()
    {
        var districtCity = await CreateDistrictCityAsync();
        var name = "Test Locality " + Guid.NewGuid();

        var result = await _localityAppService.CreateAsync(new CreateUpdateLocalityDto
        {
            DistrictCityId = districtCity.Id,
            Name = name,
            Population = 1000,
            Remarks = "Some remarks",
            DistrictCityCode = "DC",
            LocalityCode = "LC",
            Latitude = 12.34m,
            Longitude = 56.78m
        });

        result.Id.ShouldNotBe(Guid.Empty);
        result.DistrictCityId.ShouldBe(districtCity.Id);
        result.Name.ShouldBe(name);
        result.DistrictCityCode.ShouldBe("DC");
        result.LocalityCode.ShouldBe("LC");
        result.Latitude.ShouldBe(12.34m);
        result.Longitude.ShouldBe(56.78m);
    }

    [Fact]
    public async Task Should_Not_Create_A_Locality_Without_Name()
    {
        var districtCity = await CreateDistrictCityAsync();

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _localityAppService.CreateAsync(new CreateUpdateLocalityDto { DistrictCityId = districtCity.Id, Name = "" });
        });

        exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Locality_With_NonExistent_DistrictCity()
    {
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _localityAppService.CreateAsync(new CreateUpdateLocalityDto
            {
                DistrictCityId = Guid.NewGuid(),
                Name = "Test Locality " + Guid.NewGuid()
            });
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Locality_With_Duplicate_Name()
    {
        var districtCity = await CreateDistrictCityAsync();
        var name = "Duplicate Locality " + Guid.NewGuid();
        await _localityAppService.CreateAsync(new CreateUpdateLocalityDto { DistrictCityId = districtCity.Id, Name = name });

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _localityAppService.CreateAsync(new CreateUpdateLocalityDto { DistrictCityId = districtCity.Id, Name = name });
        });
    }

    [Fact]
    public async Task Should_Get_List_Of_Localities()
    {
        var districtCity = await CreateDistrictCityAsync();
        var name = "Listed Locality " + Guid.NewGuid();
        var created = await _localityAppService.CreateAsync(new CreateUpdateLocalityDto { DistrictCityId = districtCity.Id, Name = name, Population = 42 });

        var result = await _localityAppService.GetListAsync(new GetLocalitiesInput { FilterText = name });

        result.Items.ShouldContain(l => l.Id == created.Id && l.Name == name && l.DistrictCityName == districtCity.Name);
    }

    [Fact]
    public async Task Should_Get_A_Locality_By_Id()
    {
        var districtCity = await CreateDistrictCityAsync();
        var created = await _localityAppService.CreateAsync(new CreateUpdateLocalityDto { DistrictCityId = districtCity.Id, Name = "Test Locality " + Guid.NewGuid(), Population = 7 });

        var result = await _localityAppService.GetAsync(created.Id);

        result.Name.ShouldBe(created.Name);
        result.Population.ShouldBe(7);
        result.DistrictCityName.ShouldBe(districtCity.Name);
    }

    [Fact]
    public async Task Should_Throw_When_Getting_A_Non_Existent_Locality()
    {
        await Assert.ThrowsAsync<EntityNotFoundException<Locality>>(async () =>
        {
            await _localityAppService.GetAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Should_Update_A_Locality()
    {
        var districtCity = await CreateDistrictCityAsync();
        var otherDistrictCity = await CreateDistrictCityAsync();
        var created = await _localityAppService.CreateAsync(new CreateUpdateLocalityDto { DistrictCityId = districtCity.Id, Name = "Test Locality " + Guid.NewGuid(), Population = 1 });
        var newName = "Updated Locality " + Guid.NewGuid();

        var updated = await _localityAppService.UpdateAsync(created.Id, new CreateUpdateLocalityDto
        {
            DistrictCityId = otherDistrictCity.Id,
            Name = newName,
            Population = 999,
            Remarks = "Updated remarks",
            DistrictCityCode = "UD",
            LocalityCode = "UL",
            Latitude = 1,
            Longitude = 2
        });

        updated.Name.ShouldBe(newName);
        updated.DistrictCityId.ShouldBe(otherDistrictCity.Id);
        updated.Population.ShouldBe(999);
        updated.DistrictCityCode.ShouldBe("UD");
        updated.LocalityCode.ShouldBe("UL");
    }

    [Fact]
    public async Task Should_Delete_A_Locality()
    {
        var districtCity = await CreateDistrictCityAsync();
        var created = await _localityAppService.CreateAsync(new CreateUpdateLocalityDto { DistrictCityId = districtCity.Id, Name = "Test Locality " + Guid.NewGuid() });

        await _localityAppService.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<EntityNotFoundException<Locality>>(async () =>
        {
            await _localityAppService.GetAsync(created.Id);
        });
    }
}
