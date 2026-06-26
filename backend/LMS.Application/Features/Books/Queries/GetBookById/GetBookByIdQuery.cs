using LMS.Application.Features.Books.DTOs;
using MediatR;

namespace LMS.Application.Features.Books.Queries.GetBookById;

public record GetBookByIdQuery(Guid Id) : IRequest<BookDetailDto>;
