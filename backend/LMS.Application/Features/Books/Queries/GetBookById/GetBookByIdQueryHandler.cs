using LMS.Application.Features.Books.DTOs;
using MediatR;

namespace LMS.Application.Features.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDetailDto>
{
    // TODO: serve from Redis key books:detail:{bookId} (TTL 10 min) on hit; else load (NotFound if missing),
    //       include author, compute HasActiveBorrowByCurrentMember, cache, return.
    public Task<BookDetailDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
