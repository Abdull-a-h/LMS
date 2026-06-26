using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Queries.GetAuthors;

public record GetAuthorsQuery : IRequest<IReadOnlyList<AuthorDto>>;
