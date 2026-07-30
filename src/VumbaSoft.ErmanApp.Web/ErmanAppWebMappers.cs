using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using VumbaSoft.ErmanApp.Authors;
using VumbaSoft.ErmanApp.Books;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Localities;
namespace VumbaSoft.ErmanApp.Web;
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppWebMappers : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);
    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppAuthorDtoToCreateUpdateAuthorDtoMapper : MapperBase<AuthorDto, CreateUpdateAuthorDto>
{
    public override partial CreateUpdateAuthorDto Map(AuthorDto source);
    public override partial void Map(AuthorDto source, CreateUpdateAuthorDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppContinentDtoToCreateUpdateContinentDtoMapper : MapperBase<ContinentDto, CreateUpdateContinentDto>
{
    public override partial CreateUpdateContinentDto Map(ContinentDto source);
    public override partial void Map(ContinentDto source, CreateUpdateContinentDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppSubcontinentDtoToCreateUpdateSubcontinentDtoMapper : MapperBase<SubcontinentDto, CreateUpdateSubcontinentDto>
{
    public override partial CreateUpdateSubcontinentDto Map(SubcontinentDto source);
    public override partial void Map(SubcontinentDto source, CreateUpdateSubcontinentDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppRegionDtoToCreateUpdateRegionDtoMapper : MapperBase<RegionDto, CreateUpdateRegionDto>
{
    public override partial CreateUpdateRegionDto Map(RegionDto source);
    public override partial void Map(RegionDto source, CreateUpdateRegionDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppCountryDtoToCreateUpdateCountryDtoMapper : MapperBase<CountryDto, CreateUpdateCountryDto>
{
    public override partial CreateUpdateCountryDto Map(CountryDto source);
    public override partial void Map(CountryDto source, CreateUpdateCountryDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppStateProvinceDtoToCreateUpdateStateProvinceDtoMapper : MapperBase<StateProvinceDto, CreateUpdateStateProvinceDto>
{
    public override partial CreateUpdateStateProvinceDto Map(StateProvinceDto source);
    public override partial void Map(StateProvinceDto source, CreateUpdateStateProvinceDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppDistrictCityDtoToCreateUpdateDistrictCityDtoMapper : MapperBase<DistrictCityDto, CreateUpdateDistrictCityDto>
{
    public override partial CreateUpdateDistrictCityDto Map(DistrictCityDto source);
    public override partial void Map(DistrictCityDto source, CreateUpdateDistrictCityDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppLocalityDtoToCreateUpdateLocalityDtoMapper : MapperBase<LocalityDto, CreateUpdateLocalityDto>
{
    public override partial CreateUpdateLocalityDto Map(LocalityDto source);
    public override partial void Map(LocalityDto source, CreateUpdateLocalityDto destination);
}
