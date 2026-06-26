using AutoMapper;

namespace LMS.Application.Common.Mappings;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        // TODO: Book -> BookDto (flatten AuthorName)
        // TODO: Book -> BookDetailDto (with nested AuthorDto + HasActiveBorrowByCurrentMember)
    }
}
