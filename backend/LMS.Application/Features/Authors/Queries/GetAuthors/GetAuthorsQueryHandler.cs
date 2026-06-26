using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Queries.GetAuthors;

public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IReadOnlyList<AuthorDto>>
{
    public Task<IReadOnlyList<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
