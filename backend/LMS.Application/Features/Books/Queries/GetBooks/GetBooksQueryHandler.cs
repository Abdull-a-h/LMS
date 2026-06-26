using LMS.Application.Common.Models;
using LMS.Application.Features.Books.DTOs;
using MediatR;

namespace LMS.Application.Features.Books.Queries.GetBooks;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PagedResult<BookDto>>
{
    // TODO: serve from Redis key books:list:{authorId}:{keyword}:{page}:{pageSize} (TTL 5 min) on hit;
    //       else query DB, map, cache, return. Log cache hit/miss at Debug.
    public Task<PagedResult<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
