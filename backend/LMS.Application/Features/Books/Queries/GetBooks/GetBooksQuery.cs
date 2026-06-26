using LMS.Application.Common.Models;
using LMS.Application.Features.Books.DTOs;
using MediatR;

namespace LMS.Application.Features.Books.Queries.GetBooks;

public record GetBooksQuery(Guid? AuthorId, string? Keyword, int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<BookDto>>;
