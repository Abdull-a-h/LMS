using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Queries.GetAuthorById;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDetailDto>
{
    // TODO: load author incl. books; NotFoundException if missing.
    public Task<AuthorDetailDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
