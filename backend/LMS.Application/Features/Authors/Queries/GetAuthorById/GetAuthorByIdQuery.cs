using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Queries.GetAuthorById;

public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDetailDto>;
