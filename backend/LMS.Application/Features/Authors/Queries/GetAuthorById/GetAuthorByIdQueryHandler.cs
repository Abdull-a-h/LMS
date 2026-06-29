using AutoMapper;
using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Authors.DTOs;
using LMS.Domain.Exceptions;
using MediatR;

namespace LMS.Application.Features.Authors.Queries.GetAuthorById;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDetailDto>
{
    private readonly IAuthorRepository _authors;
    private readonly IMapper _mapper;

    public GetAuthorByIdQueryHandler(IAuthorRepository authors, IMapper mapper)
    {
        _authors = authors;
        _mapper = mapper;
    }

    public async Task<AuthorDetailDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        // Includes the author's active books for the nested BookSummaryDto list.
        var author = await _authors.GetByIdAsync(request.Id, cancellationToken);
        if (author is null)
        {
            throw new NotFoundException(nameof(LMS.Domain.Entities.Author), request.Id);
        }

        return _mapper.Map<AuthorDetailDto>(author);
    }
}
