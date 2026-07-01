using AutoMapper;
using LMS.Application.Features.Books.DTOs;
using LMS.Domain.Entities;

namespace LMS.Application.Common.Mappings;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        // Flatten the author's name onto the list DTO.
        CreateMap<Book, BookDto>()
            .ForMember(d => d.AuthorName, o => o.MapFrom(s => s.Author.Name));

        // Nested AuthorDto comes from the Author navigation (mapped in AuthorMappingProfile).
        // HasActiveBorrowByCurrentMember is per-caller, so it is computed in the handler
        // outside the shared cached object — never mapped from the entity here.
        CreateMap<Book, BookDetailDto>()
            .ForMember(d => d.HasActiveBorrowByCurrentMember, o => o.Ignore());
    }
}
