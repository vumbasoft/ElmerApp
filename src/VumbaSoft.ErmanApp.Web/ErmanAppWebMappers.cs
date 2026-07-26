using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using VumbaSoft.ErmanApp.Authors;
using VumbaSoft.ErmanApp.Books;
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
