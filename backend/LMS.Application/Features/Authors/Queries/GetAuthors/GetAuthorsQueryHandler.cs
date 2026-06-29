using AutoMapper;
using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Queries.GetAuthors;

public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IReadOnlyList<AuthorDto>>
{
    private readonly IAuthorRepository _authors;
    private readonly IMapper _mapper;

    public GetAuthorsQueryHandler(IAuthorRepository authors, IMapper mapper)
    {
        _authors = authors;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _authors.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<AuthorDto>>(authors);
    }
}
