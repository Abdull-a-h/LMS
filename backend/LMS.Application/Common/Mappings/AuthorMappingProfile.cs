using AutoMapper;
using LMS.Application.Features.Authors.DTOs;
using LMS.Domain.Entities;

namespace LMS.Application.Common.Mappings;

public class AuthorMappingProfile : Profile
{
    public AuthorMappingProfile()
    {
        CreateMap<Author, AuthorDto>();

        // Books is populated from the navigation property; only active books are loaded
        // (Book's IsActive query filter), so the nested list reflects available titles.
        CreateMap<Author, AuthorDetailDto>();

        CreateMap<Book, BookSummaryDto>();
    }
}
