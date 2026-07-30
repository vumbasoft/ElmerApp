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

namespace VumbaSoft.ErmanApp;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppBookToBookDtoMapper : MapperBase<Book, BookDto>
{
    [MapperIgnoreTarget(nameof(BookDto.AuthorName))]
    public override partial BookDto Map(Book source);

    [MapperIgnoreTarget(nameof(BookDto.AuthorName))]
    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppCreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    public override partial Book Map(CreateUpdateBookDto source);

    public override partial void Map(CreateUpdateBookDto source, Book destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppAuthorToAuthorDtoMapper : MapperBase<Author, AuthorDto>
{
    public override partial AuthorDto Map(Author source);

    public override partial void Map(Author source, AuthorDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppCreateUpdateAuthorDtoToAuthorMapper : MapperBase<CreateUpdateAuthorDto, Author>
{
    public override partial Author Map(CreateUpdateAuthorDto source);

    public override partial void Map(CreateUpdateAuthorDto source, Author destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppAuthorToAuthorExcelDtoMapper : MapperBase<Author, AuthorExcelDto>
{
    public override partial AuthorExcelDto Map(Author source);

    public override partial void Map(Author source, AuthorExcelDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppContinentToContinentDtoMapper : MapperBase<Continent, ContinentDto>
{
    public override partial ContinentDto Map(Continent source);

    public override partial void Map(Continent source, ContinentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppSubcontinentToSubcontinentDtoMapper : MapperBase<Subcontinent, SubcontinentDto>
{
    [MapperIgnoreTarget(nameof(SubcontinentDto.ContinentName))]
    public override partial SubcontinentDto Map(Subcontinent source);

    [MapperIgnoreTarget(nameof(SubcontinentDto.ContinentName))]
    public override partial void Map(Subcontinent source, SubcontinentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppRegionToRegionDtoMapper : MapperBase<Region, RegionDto>
{
    [MapperIgnoreTarget(nameof(RegionDto.SubcontinentName))]
    public override partial RegionDto Map(Region source);

    [MapperIgnoreTarget(nameof(RegionDto.SubcontinentName))]
    public override partial void Map(Region source, RegionDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppCountryToCountryDtoMapper : MapperBase<Country, CountryDto>
{
    [MapperIgnoreTarget(nameof(CountryDto.RegionName))]
    public override partial CountryDto Map(Country source);

    [MapperIgnoreTarget(nameof(CountryDto.RegionName))]
    public override partial void Map(Country source, CountryDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppStateProvinceToStateProvinceDtoMapper : MapperBase<StateProvince, StateProvinceDto>
{
    [MapperIgnoreTarget(nameof(StateProvinceDto.CountryName))]
    public override partial StateProvinceDto Map(StateProvince source);

    [MapperIgnoreTarget(nameof(StateProvinceDto.CountryName))]
    public override partial void Map(StateProvince source, StateProvinceDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppDistrictCityToDistrictCityDtoMapper : MapperBase<DistrictCity, DistrictCityDto>
{
    [MapperIgnoreTarget(nameof(DistrictCityDto.StateProvinceName))]
    public override partial DistrictCityDto Map(DistrictCity source);

    [MapperIgnoreTarget(nameof(DistrictCityDto.StateProvinceName))]
    public override partial void Map(DistrictCity source, DistrictCityDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ErmanAppLocalityToLocalityDtoMapper : MapperBase<Locality, LocalityDto>
{
    [MapperIgnoreTarget(nameof(LocalityDto.DistrictCityName))]
    public override partial LocalityDto Map(Locality source);

    [MapperIgnoreTarget(nameof(LocalityDto.DistrictCityName))]
    public override partial void Map(Locality source, LocalityDto destination);
}
